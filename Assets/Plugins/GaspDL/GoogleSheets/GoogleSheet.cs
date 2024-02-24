using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
#if UNITY_2017_2_OR_NEWER
using UnityEngine.Networking;
#endif
namespace GaspDL.GoogleSheet
{ 
    public static class GoogleDownload
    {
        public static IEnumerator DownloadSheet(string docsId, string sheetId, Action<string> done, GoogleDriveDownloadFormat format = GoogleDriveDownloadFormat.Csv, Func<float, bool> progressbar = null)
        {
            if (progressbar != null && progressbar(0))
            {
                done(null);
                yield break;
            }

            var url = string.Format("https://docs.google.com/spreadsheets/d/{0}/export?format={2}&gid={1}", docsId, sheetId, Enum.GetName(typeof(GoogleDriveDownloadFormat), format).ToLower());
#if UNITY_2017_2_OR_NEWER
            var www = UnityWebRequest.Get(url);
            www.SendWebRequest();
#elif UNITY_5_5_OR_NEWER
            var www = UnityWebRequest.Get(url);
            www.Send();
#else
            var www = new WWW(url);
#endif
            while (!www.isDone)
            {
#if UNITY_5_5_OR_NEWER
                var progress = www.downloadProgress;
#else
                var progress = www.progress;
#endif
                if (progressbar != null && progressbar(progress))
                {
                    done(null);
                    yield break;
                }
                yield return null;
            }

            if (progressbar != null && progressbar(1))
            {
                done(null);
                yield break;
            }

#if UNITY_5_5_OR_NEWER
            var text = www.downloadHandler.text;
#else
            var text = www.text;
#endif

            if (text.StartsWith("<!"))
            {
                Debug.LogError("Google sheet could not be downloaded.\nURL:" + url + "\n" + text);
                done(null);
                yield break;
            }

            done(text);
        }
#if UNITY_EDITOR
        public static void DownloadGoogleSheet(GoogleSheetDocument doc)
        {
            EditorUtility.DisplayCancelableProgressBar("Download", "Downloading...", 0);

            var iterator = DownloadSheet(doc.DocsId, doc.SheetId, t => DownloadComplete(t, doc), doc.Format, DisplayDownloadProgressbar);
            while (iterator.MoveNext())
            { }
        }

        private static void DownloadComplete(string text, GoogleSheetDocument doc)
        {
            if (string.IsNullOrEmpty(text))
            {
                Debug.LogError("Could not download google sheet");
                return;
            }

            var path = doc.TextAsset != null ? AssetDatabase.GetAssetPath(doc.TextAsset) : null;

            if (string.IsNullOrEmpty(path))
            {
                path = EditorUtility.SaveFilePanelInProject("Save Localization", "", "txt", "Please enter a file name to save the csv to", path);
            }
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            File.WriteAllText(path, text);

            AssetDatabase.ImportAsset(path);

            doc.TextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            EditorUtility.SetDirty(doc.TextAsset);
            AssetDatabase.SaveAssets();
        }

        private static bool DisplayDownloadProgressbar(float progress)
        {
            if (progress < 1)
            {
                return EditorUtility.DisplayCancelableProgressBar("Download Localization", "Downloading...", progress);
            }

            EditorUtility.ClearProgressBar();
            return false;
        }
#endif
    }

}