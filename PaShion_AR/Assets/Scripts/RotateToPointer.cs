using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RotateToPointer : MonoBehaviour
{
    public Image[] images;
    public float deadZoneRadius = 50f;

    RectTransform rectTransform;
    CanvasGroup canvasGroup;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (!gameObject.activeSelf) return;

        Vector2 pointerPosition = Vector2.zero;

        // Check if touchscreen exists
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        {
            pointerPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.isPressed)
        {
            pointerPosition = Mouse.current.position.ReadValue();
        }
        else
        {
            return;
        }

        Vector2 center = rectTransform.position;
        Vector2 direction = pointerPosition - center;

        float distance = direction.magnitude;

        // Check if inside to hide
        if (distance < deadZoneRadius)
        {
            canvasGroup.alpha = 0f;
            return;
        }

        // If outside to show
        canvasGroup.alpha = 1f;

        // Calculates rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rectTransform.rotation = Quaternion.Euler(0, 0, angle);

        // MOVED TO SEPERATE SCRIPT
        //// Update color
        //float hue = (angle / 360f);
        //hue = (hue + 0.6667f) % 1f;

        //Color color = Color.HSVToRGB(hue, 1f, 1f);
        //color.a = 1f;

        //// Update each image color;
        //foreach (var img in images)
        //{
        //    if (img != null)
        //        img.color = color;
        //}
    }
}