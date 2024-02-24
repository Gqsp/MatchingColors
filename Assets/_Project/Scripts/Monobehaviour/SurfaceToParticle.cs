using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

[Serializable]
public class SurfaceToParticle
{
    [SerializeField] int edgeThickness;
    public bool GetPixelColors(Vector2 origin, Vector2 direction, LayerMask layer, ref Color[] colors, out Vector2 hitPos)
    {
        RaycastHit2D rayHit = Physics2D.Raycast(origin, direction, 2, layer);
        hitPos = Vector2.zero;
        if (rayHit)
        {
            hitPos = rayHit.point;
            if (rayHit.transform.gameObject.TryGetComponent<Tilemap>(out var tilemap))
            {
                var sprite = FindSprite(hitPos + direction.normalized * 0.1f, tilemap);
                if (sprite == null) return false;
                GetColors(sprite, rayHit.normal, ref colors);

                for (int i = 0; i < colors.Length; i++)
                {
                    colors[i] *= tilemap.color;
                }
                
                return true;
            }
            return false;
        }

        return false;
    }

    public bool GetPixelColor(Vector2 origin, Vector2 direction, LayerMask layer, ref Color color, out Vector2 hitPos)
    {
        RaycastHit2D rayHit = Physics2D.Raycast(origin, direction, 2, layer);
        hitPos = Vector2.zero;
        if (rayHit)
        {
            hitPos = rayHit.point;
            if (rayHit.transform.gameObject.TryGetComponent<Tilemap>(out var tilemap))
            {
                var sprite = FindSprite(hitPos + direction.normalized * 0.1f, tilemap);
                if (sprite == null) return false;
                color = GetRandomPixelOnEdge(sprite, rayHit.normal) * tilemap.color;
                
                return true;
            }
            return false;
        }

        return false;
    }

    Sprite FindSprite(Vector2 position, Tilemap map)
    {
        Vector3Int gridPosition = new Vector3Int(Mathf.FloorToInt(position.x - map.transform.position.x), Mathf.FloorToInt(position.y - map.transform.position.y), 0);
        return map.GetSprite(gridPosition);
    }

    Color GetRandomPixelOnEdge(Sprite sprite, Vector2 edge)
    {
        int x; int y;

        if (Mathf.Abs(edge.x) <= 0.1f)
        {
            x = Random.Range(0, (int)sprite.textureRect.size.x);
        } else
        {
            x = (int)Mathf.Clamp01(Mathf.Round(edge.x)) * ((int)sprite.textureRect.size.x - 1);
        }

        if (Mathf.Abs(edge.y) <= 0.1f)
        {
            y = Random.Range(0, (int)sprite.textureRect.size.y);
        } else
        {
            y = (int)Mathf.Clamp01(Mathf.Round(edge.y)) * ((int)sprite.textureRect.size.y - 1);
        }

        x += Random.Range(0, edgeThickness + 1) * (int)-edge.x;
        y += Random.Range(0, edgeThickness + 1) * (int)-edge.y;

        x += (int)sprite.textureRect.position.x;
        y += (int)sprite.textureRect.position.y;

        return sprite.texture.GetPixel(x, y);
    }

    void GetColors(Sprite sprite, Vector2 edge, ref Color[] colors)
    {
        
        int x = 0; int y = 0;
        bool randomizeX = false;
        bool randomizeY = false;
        if (Mathf.Abs(edge.x) <= 0.1f)
        {
            randomizeX = true;
        }
        else
        {
            x = (int)Mathf.Clamp01(Mathf.Round(edge.x)) * ((int)sprite.textureRect.size.x - 1);
        }

        if (Mathf.Abs(edge.y) <= 0.1f)
        {
            randomizeY = true;
        }
        else
        {
            y = (int)Mathf.Clamp01(Mathf.Round(edge.y)) * ((int)sprite.textureRect.size.y - 1);
        }

        x += Random.Range(0, edgeThickness + 1) * (int)-edge.x;
        y += Random.Range(0, edgeThickness + 1) * (int)-edge.y;

        x += (int)sprite.textureRect.position.x;
        y += (int)sprite.textureRect.position.y;

        var length = colors.Length;
        for (int i = 0; i < length; i++)
        {
            if (randomizeX)
            {
                x = Random.Range(0, (int)sprite.textureRect.size.x);
                x += (int)sprite.textureRect.position.x;
            }

            if (randomizeY) 
            {
                y = Random.Range(0, (int)sprite.textureRect.size.y);
                y += (int)sprite.textureRect.position.y;
            }
            //Debug.Log((x - (int)sprite.textureRect.position.x) + " " + (y - (int)sprite.textureRect.position.y));
            colors[i] = sprite.texture.GetPixel(x, y);
        }
    }
}
