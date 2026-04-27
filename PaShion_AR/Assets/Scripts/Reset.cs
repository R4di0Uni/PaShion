using UnityEngine;
using System.Collections;

public class Reset : MonoBehaviour
{
    [SerializeField] OSCTrigger trigger;
    [SerializeField] RenderTexture renderTexture;
    [SerializeField] CopyTextureToRT textureCopier;
    [SerializeField] PaintableObject paintable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(ResetFlow());
        }

      
    }

    IEnumerator ResetFlow()
    {
        paintable.Clear(Color.white); // or black

        trigger.SendTrigger();

        yield return null;
    }

    IEnumerator FadeOut()
    {
        float duration = 1.5f;
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float alpha = t / duration;

            FadeRenderTexture(alpha);

            yield return null;
        }
    }

    void FadeRenderTexture(float alpha)
    {
        RenderTexture current = RenderTexture.active;
        RenderTexture.active = renderTexture;

        GL.PushMatrix();
        GL.LoadOrtho();

        Material mat = new Material(Shader.Find("Hidden/Internal-Colored"));
        mat.SetPass(0);

        GL.Begin(GL.QUADS);
        GL.Color(new Color(1, 1, 1, alpha)); // fade to white

        GL.Vertex3(0, 0, 0);
        GL.Vertex3(1, 0, 0);
        GL.Vertex3(1, 1, 0);
        GL.Vertex3(0, 1, 0);

        GL.End();
        GL.PopMatrix();

        RenderTexture.active = current;
    }
}