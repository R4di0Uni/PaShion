using UnityEngine;
using UnityEngine.InputSystem;

public class Reset : MonoBehaviour
{
    [SerializeField] OSCTrigger trigger;
    [SerializeField] CopyTextureToRT textureCopier;
    [SerializeField] PaintableObject paintable;

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame)
        {
            trigger.SendTrigger();
            Debug.Log("AI Trigger sent (T)");
        }

        if (Keyboard.current.cKey.wasPressedThisFrame)
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