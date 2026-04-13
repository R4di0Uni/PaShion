using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class EnableColourWheel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    public GameObject colourWheel;
    RectTransform buttonRect;
    RectTransform imageRect;

    private PaintController paintController;

    void Awake()
    {
        buttonRect = GetComponent<RectTransform>();
        imageRect = colourWheel.GetComponent<RectTransform>();

        paintController = FindAnyObjectByType<PaintController>();

        colourWheel.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        colourWheel.SetActive(true);

        paintController.isPainting = true;

        // Center image on button
        //imageRect.position = buttonRect.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        colourWheel.SetActive(false);

        paintController.isPainting = false;
    }
}