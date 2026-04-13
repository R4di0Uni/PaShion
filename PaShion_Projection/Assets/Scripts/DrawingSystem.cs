using UnityEngine;
using UnityEngine.UI;

public class DrawingSystem : MonoBehaviour
{
    public RenderTexture drawingTexture;
    public Material drawMaterial;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 localPoint;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                Input.mousePosition,
                null,
                out localPoint))
            {
                Rect rect = rectTransform.rect;

                if (rect.Contains(localPoint))
                {
                    float x = (localPoint.x - rect.x) / rect.width * drawingTexture.width;
                    float y = drawingTexture.height - ((localPoint.y - rect.y) / rect.height * drawingTexture.height);
                    Draw(x, y);
                }
            }
        }
    }

    void Draw(float x, float y)
    {
        RenderTexture.active = drawingTexture;

        GL.PushMatrix();
        GL.LoadPixelMatrix(0, drawingTexture.width, drawingTexture.height, 0);

        drawMaterial.SetPass(0);

        float size = 20;

        GL.Begin(GL.QUADS);
        GL.Vertex3(x - size, y - size, 0);
        GL.Vertex3(x + size, y - size, 0);
        GL.Vertex3(x + size, y + size, 0);
        GL.Vertex3(x - size, y + size, 0);
        GL.End();

        GL.PopMatrix();

        RenderTexture.active = null;
    }
}