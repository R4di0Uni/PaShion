using UnityEngine;
using UnityEngine.UI;

public class RotationToColour : MonoBehaviour
{
    public Image[] images;
    public Color selectedColor = Color.blue;
    private PaintController paintController;

    private void Start()
    {
        paintController = FindAnyObjectByType<PaintController>();
    }

    void Update()
    {
        float angle = transform.eulerAngles.z;

        // Map 0-360 degrees to 0-1 hue
        float hue = angle / 360f;

        // Offset so 0° = blue
        hue = (hue + 0.6667f) % 1f;

        selectedColor = Color.HSVToRGB(hue, 1f, 1f);
        selectedColor.a = 1f;

        paintController.currentColor = selectedColor;

        for (int i = 0; i < images.Length; i++)
        {
            if (images[i] != null)
                images[i].color = selectedColor;
        }
    }
}