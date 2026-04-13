using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PaintController: MonoBehaviour
{
    public bool isPainting;
    public Color currentColor = Color.blue; 
    
    private float brushScreenRadius = 150f; // Screen radius
    public int brushSamples = 30; // Number of rays to paint
    public float sampleRadius = 10f;

    public RectTransform uiCircleAim;
    public Camera arCamera;

    private void Start()
    {
        brushScreenRadius = uiCircleAim.sizeDelta.x * 0.5f - sampleRadius/2;
    }

    void Update()
    {
        if (!isPainting) return;

        Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);

        for (int i = 0; i < brushSamples; i++)
        {
            //float angle = i * Mathf.PI * 2 / brushSamples;
            //Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * brushScreenRadius;
            Vector2 offset = Random.insideUnitCircle * brushScreenRadius;

            Vector2 screenPoint = screenCenter + offset;

            Ray ray = arCamera.ScreenPointToRay(screenPoint);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                PaintableObject paintable = hit.collider.GetComponent<PaintableObject>();

                if (paintable != null)
                {
                    paintable.Paint(hit, currentColor, sampleRadius); // small radius
                }
            }
        }
    }
}