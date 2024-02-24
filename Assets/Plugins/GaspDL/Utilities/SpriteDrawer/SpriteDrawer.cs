using UnityEngine;
using UnityEngine.Serialization;

namespace GaspDL.Renderer
{
    public class SpriteDrawer : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private Sprite _sprite;
        private Texture2D _spriteText;
        public int tileCountWidth, tileCountHeight;
        [FormerlySerializedAs("TileSizeX")] [Min(1)] public int tileSizeX;
        [FormerlySerializedAs("TileSizeY")] [Min(1)] public int tileSizeY;

        public Color backgroundColor;

        public bool updateInEditor;

        private void OnValidate()
        {
            if (!updateInEditor) return;

            SetupSprite();
            ClearSprite();
            _spriteText.Apply();
        }

        private void Start()
        {
            SetupSprite();

            ClearSprite();
            _spriteText.Apply();
        }

        private void LateUpdate()
        {
            Apply();
            ClearSprite();
        }

        private void SetupSprite()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }

            _sprite = Sprite.Create(new Texture2D(tileCountWidth * tileSizeX, tileCountHeight * tileSizeY), new Rect(0, 0, tileCountWidth * tileSizeX, tileCountHeight * tileSizeY), new Vector2(0.5f, 0.5f), 32);
            _spriteRenderer.sprite = _sprite;
            _spriteText = _sprite.texture;
            _spriteText.filterMode = FilterMode.Point;
        }

        public void ClearSprite()
        {
            for (int w = 0; w < tileCountWidth; w++)
            {
                for (int h = 0; h < tileCountHeight; h++)
                {
                    DrawPixel(w, h, backgroundColor);
                }
            }
        }

        public void DrawPixel(int x, int y, Color color)
        {
            DrawPixel(new Vector2Int(x, y), color);
        }
        
        public void DrawPixel(Vector2 position, Color color)
        {
            DrawPixel(new Vector2Int((int)position.x, (int)position.y), color);
        }

        public void DrawPixel(Vector2Int position, Color color)
        {
            for (int i = 0; i < tileSizeX; i++)
            {
                for (int j = 0; j < tileSizeY; j++)
                {
                    _spriteText.SetPixel(position.x * tileSizeX + i, position.y * tileSizeY + j, color);
                }
            }
        }

        public void DrawLine(Vector2 startPos, Vector2 endPos, Color color)
        {
            int x = (int)startPos.x;
            int y = (int)startPos.y;
            int x2 = (int)endPos.x;
            int y2 = (int)endPos.y;

            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Mathf.Abs(w);
            int shortest = Mathf.Abs(h);
            if (!(longest > shortest))
            {
                longest = Mathf.Abs(h);
                shortest = Mathf.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                DrawPixel(x, y, color);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }

        }

        public void Apply()
        {
            _spriteText.Apply();
        }

    }
}

