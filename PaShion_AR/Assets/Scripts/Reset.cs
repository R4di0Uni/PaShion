using UnityEngine;

public class Reset : MonoBehaviour
{
    [SerializeField] OSCTrigger trigger;
    [SerializeField] CopyTextureToRT textureCopier;
    [SerializeField] PaintableObject paintable;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            trigger.SendTrigger();
            Debug.Log("AI Trigger sent (T)");
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearAll();
        }
    }

    void ClearAll()
    {
        Debug.Log("Clearing textures...");

        textureCopier.isResetting = true;

        paintable.Clear(Color.white);

        textureCopier.ResetRenderTexture();

        Invoke(nameof(ResumeCopy), 0.05f);
    }

    void ResumeCopy()
    {
        textureCopier.isResetting = false;
        Debug.Log("Copy resumed");
    }
}