using System;
using System.Collections;
using GaspDL;
using UnityEngine;

public class BackdropHandler : Singleton<BackdropHandler>
{
    public CanvasGroup backdrop;

    private void Start()
    {
        backdrop.alpha = 1;
        Release(1f);
    }

    public void Require(float time, Action callback = null)
    {
        StartCoroutine(BackdropFade(time, 1, callback));
    }
    
    public void Release(float time, Action callback = null)
    {
        StartCoroutine(BackdropFade(time, 0, callback));
    }
    
    private IEnumerator BackdropFade(float time, float target, Action callback = null)
    {
        float t = 0;
        float current = backdrop.alpha;
        while (t < time)
        {
            t += Time.deltaTime;
            backdrop.alpha = Mathf.Lerp(current, target, t / time);
            yield return null;
        }
        
        callback?.Invoke();
    }
}
