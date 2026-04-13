using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDrawing : MonoBehaviour
{
    public RenderTexture drawingTexture;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearTexture();
        }
    }

    void ClearTexture()
    {
        RenderTexture.active = drawingTexture;
        GL.Clear(true, true, Color.clear);
        RenderTexture.active = null;
    }
}
