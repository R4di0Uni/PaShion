using UnityEngine;

public class PaintController : MonoBehaviour
{
    public bool isPainting;
    public Color currentColor = Color.blue;

    private float brushScreenRadius = 150f;
    public int brushSamples = 30;
    public float sampleRadius = 10f;
    public RectTransform uiCircleAim;
    public Camera arCamera;
    public PaintableObject paintable;

    private void Start()
    {
        brushScreenRadius = uiCircleAim.sizeDelta.x * 0.5f - sampleRadius / 2;
    }

    void Update()
    {
        if (!isPainting) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        for (int i = 0; i < brushSamples; i++)
        {
            Vector2 offset = Random.insideUnitCircle * brushScreenRadius;
            Vector2 screenPoint = screenCenter + offset;
            Ray ray = arCamera.ScreenPointToRay(screenPoint);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.collider.GetComponent<PaintableObject>() != null)
                    paintable = hit.collider.GetComponent<PaintableObject>();
                    paintable.Paint(hit, currentColor, sampleRadius);
            }
        }

        //if(paintable != null) { paintable.FlushIfDirty(); }
    }
}