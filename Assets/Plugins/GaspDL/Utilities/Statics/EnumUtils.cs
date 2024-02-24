using System;
using System.Collections;
using UnityEngine;

namespace GaspDL.Utils
{
    public static class EnumUtils
    {
        public static IEnumerator FadeOpacityInTime(SpriteRenderer sprite, float time, float end)
        {
            if (sprite == null) yield break;

            float t = 0;
            float start = sprite.color.a;
            Color c = sprite.color;

            while (sprite.color.a != end)
            {
                t += Time.deltaTime;
                sprite.color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, Mathf.Clamp(t / time, 0, 1)));
                yield return null;
            }
        }

        public static IEnumerator FadeOpacityInTime(TextMesh text, float time, float end)
        {
            if (text == null) yield break;

            float t = 0;
            float start = text.color.a;
            Color c = text.color;

            while (text.color.a != end)
            {
                t += Time.deltaTime;
                text.color = new Color(c.r, c.g, c.b, Mathf.Lerp(start, end, Mathf.Clamp(t / time, 0, 1)));
                yield return null;
            }
        }

        public static IEnumerator CooldownToAction(float time, Action action)
        {
            yield return new WaitForSeconds(time);
            action.Invoke();
        }
    }
}

