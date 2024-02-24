using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretPassage : MonoBehaviour
{
    [SerializeField] Tilemap _tilemapRenderer;
    [SerializeField] private Color _fadedColor;

    private void OnTriggerEnter2D(Collider2D other)
    {
        StopAllCoroutines();
        StartCoroutine(FadeColor(_tilemapRenderer.color, _fadedColor, 0.3f));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        StopAllCoroutines();
        StartCoroutine(FadeColor(_tilemapRenderer.color, Color.white, 0.3f));
    }
    
    public void EnterSecret()
    {
        StopAllCoroutines();
        StartCoroutine(FadeColor(_tilemapRenderer.color, _fadedColor, 0.3f));
    }
    
    public void ExitSecret()
    {
        StopAllCoroutines();
        StartCoroutine(FadeColor(_tilemapRenderer.color, Color.white, 0.3f));
    }

    IEnumerator FadeColor(Color start, Color end, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            _tilemapRenderer.color = Color.Lerp(start, end, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _tilemapRenderer.color = end;
    }
}
