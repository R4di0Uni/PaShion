using UnityEngine;
using Unity.Netcode;

public class PaintableObject : NetworkBehaviour
{
    Texture2D paintTexture;
    Color32[] pixelBuffer;
    Renderer rend;

    bool isDirty = false;
    int dirtyMinX, dirtyMinY, dirtyMaxX, dirtyMaxY;

    void Start()
    {
        rend = GetComponent<Renderer>();
        Texture2D baseTex = rend.material.mainTexture as Texture2D;

        paintTexture = new Texture2D(baseTex.width, baseTex.height, TextureFormat.RGBA32, false);
        paintTexture.SetPixels(baseTex.GetPixels());
        paintTexture.Apply();
        rend.material.mainTexture = paintTexture;

        pixelBuffer = paintTexture.GetPixels32();
    }
    void Update()
    {
        FlushIfDirty();
    }

    // Android PaintController calls this
    public void Paint(RaycastHit hit, Color color, float brushSize)
    {
        Paint(hit.textureCoord, color, brushSize);
    }

    // Sends command to host, which broadcasts to everyone
    public void Paint(Vector2 uv, Color color, float brushSize)
    {
        Color32 c = (Color32)color;
        PaintServerRpc(uv, c.r, c.g, c.b, c.a, brushSize);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void PaintServerRpc(Vector2 uv, byte r, byte g, byte b, byte a, float brushSize)
    {
        Debug.Log("ServerRpc received on host.");
        // Broadcast to all clients including host
        PaintClientRpc(uv, r, g, b, a, brushSize);
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void ClearServerRpc(byte r, byte g, byte b, byte a)
    {
        ClearClientRpc(r, g, b, a);
    }

    [ClientRpc]
    void PaintClientRpc(Vector2 uv, byte r, byte g, byte b, byte a, float brushSize)
    {
        Debug.Log("ClientRpc received. paintTexture null? " + (paintTexture == null));
        Color32 color = new Color32(r, g, b, a);
        int x = (int)(uv.x * paintTexture.width);
        int y = (int)(uv.y * paintTexture.height);
        DrawCircle(x, y, (int)brushSize, color);
    }
    [ClientRpc]
    void ClearClientRpc(byte r, byte g, byte b, byte a)
    {
        Color32 color = new Color32(r, g, b, a);
        ClearTexture(color);
    }



    void DrawCircle(int cx, int cy, int radius, Color32 color)
    {
        int w = paintTexture.width;
        int h = paintTexture.height;

        int xMin = Mathf.Max(0, cx - radius);
        int xMax = Mathf.Min(w - 1, cx + radius);
        int yMin = Mathf.Max(0, cy - radius);
        int yMax = Mathf.Min(h - 1, cy + radius);

        for (int x = xMin; x <= xMax; x++)
        {
            for (int y = yMin; y <= yMax; y++)
            {
                int dx = x - cx, dy = y - cy;
                if (dx * dx + dy * dy <= radius * radius)
                    pixelBuffer[y * w + x] = color;
            }
        }

        if (!isDirty)
        {
            dirtyMinX = xMin; dirtyMinY = yMin;
            dirtyMaxX = xMax; dirtyMaxY = yMax;
            isDirty = true;
        }
        else
        {
            dirtyMinX = Mathf.Min(dirtyMinX, xMin);
            dirtyMinY = Mathf.Min(dirtyMinY, yMin);
            dirtyMaxX = Mathf.Max(dirtyMaxX, xMax);
            dirtyMaxY = Mathf.Max(dirtyMaxY, yMax);
        }
    }

    public void ClearTexture(Color color)
    {
        if (paintTexture == null || pixelBuffer == null) return;

        Color32 c = (Color32)color;

        for (int i = 0; i < pixelBuffer.Length; i++)
        {
            pixelBuffer[i] = c;
        }

        paintTexture.SetPixels32(pixelBuffer);
        paintTexture.Apply(false);

        isDirty = false;

        Debug.Log("Texture cleared locally");
    }

    public void Clear(Color color)
    {
        Color32 c = (Color32)color;
        ClearServerRpc(c.r, c.g, c.b, c.a);
    }

    public void FlushIfDirty()
    {
        if (!isDirty) return;
        paintTexture.SetPixels32(pixelBuffer);
        paintTexture.Apply(false);
        isDirty = false;
    }
}