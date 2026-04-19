using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class PaintController_old : MonoBehaviour
{
    public bool isPainting;
    public Color currentColor = Color.blue;
    public float brushSize = .1f;

    public Camera arCamera;

    void Update()
    {
        if (!isPainting) return;

        Ray ray = new Ray(arCamera.transform.position, arCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, ~0))
        {
            PaintableObject paintable = hit.collider.GetComponent<PaintableObject>();


            if (paintable != null)
            {
                MobileDebug.Instance.Log("HIT: " + hit.collider.name);
                paintable.Paint(hit, currentColor, brushSize);
            }
        }
    }
}

