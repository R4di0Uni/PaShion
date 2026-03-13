using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawingSystem : MonoBehaviour
{
    public RenderTexture drawingTexture;
    public Material drawMaterial;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;

            RenderTexture.active = drawingTexture;

            GL.PushMatrix();
            GL.LoadPixelMatrix(0, drawingTexture.width, drawingTexture.height, 0);

            drawMaterial.SetPass(0);

            GL.Begin(GL.QUADS);

            float size = 20;

            GL.Vertex3(pos.x - size, pos.y - size, 0);
            GL.Vertex3(pos.x + size, pos.y - size, 0);
            GL.Vertex3(pos.x + size, pos.y + size, 0);
            GL.Vertex3(pos.x - size, pos.y + size, 0);

            GL.End();
            GL.PopMatrix();

            RenderTexture.active = null;
        }
    }
}
