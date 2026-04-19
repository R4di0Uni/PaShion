using UnityEngine;

public class PaintableObject_old : MonoBehaviour
{
    Texture2D paintTexture;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        Texture2D baseTex = rend.material.mainTexture as Texture2D;

        paintTexture = new Texture2D(baseTex.width, baseTex.height, TextureFormat.RGBA32, false);
        paintTexture.SetPixels(baseTex.GetPixels());
        paintTexture.Apply();

        rend.material.mainTexture = paintTexture; 
    }

    public void Paint(RaycastHit hit, Color color, float brushSize)
    {
        MobileDebug.Instance.Log("Name: " + gameObject.name + " \n UV: " + hit.textureCoord + " \n Triangle Index: " + hit.triangleIndex);
        Vector2 uv = hit.textureCoord;

        Debug.Log(paintTexture.isReadable);
        int x = (int)(uv.x * paintTexture.width);
        int y = (int)(uv.y * paintTexture.height);
        
        DrawCircle(x, y, (int)brushSize, color);
    }

    void DrawCircle(int cx, int cy, int radius, Color color)
    {
        for (int x = -radius; x <= radius; x++)
        {
            for (int y = -radius; y <= radius; y++)
            {
                if (x * x + y * y <= radius * radius)
                {
                    int px = cx + x;
                    int py = cy + y;

                    if (px >= 0 && px < paintTexture.width &&
                        py >= 0 && py < paintTexture.height)
                    {
                        paintTexture.SetPixel(px, py, color);
                    }
                }
            }
        }

        paintTexture.Apply();
    }
}
