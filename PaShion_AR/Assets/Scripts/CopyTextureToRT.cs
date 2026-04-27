using UnityEngine;

public class CopyTextureToRT : MonoBehaviour
{
    public Renderer mannequinRenderer;
    public RenderTexture outputRT;

    private Material runtimeMaterial;

    void Start()
    {
        runtimeMaterial = mannequinRenderer.material;
    }

    void Update()
    {
        if (runtimeMaterial != null && outputRT != null)
        {
            Texture sourceTexture = runtimeMaterial.mainTexture;

            if (sourceTexture != null)
            {
                Graphics.Blit(sourceTexture, outputRT);
            }
        }
    }

    // Reset (clears what TouchDesigner sees)
    public void ResetRenderTexture()
    {
        if (outputRT == null) return;

        RenderTexture.active = outputRT;
        GL.Clear(true, true, Color.white);
        RenderTexture.active = null;

        Debug.Log("RT Reset");
    }
}