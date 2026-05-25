using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class QRTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager imageManager;

    public GameObject sceneObject;

    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 rotationOffset = Vector3.zero;
    [SerializeField] private Vector3 scaleOffset = Vector3.one;

    private void OnEnable()
    {
        imageManager.trackablesChanged.AddListener(OnTrackedImagesChanged);
    }

    private void OnDisable()
    {
        imageManager.trackablesChanged.RemoveListener(OnTrackedImagesChanged);
    }

    private void OnTrackedImagesChanged(
        ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var trackedImage in args.added)
        {
            sceneObject.SetActive(true); 
            ApplyTransform(trackedImage); 
            TutorialUI.Instance.TutorialCompleted(1);
            //sceneObject.transform.position =
            //    trackedImage.transform.TransformPoint(positionOffset); 

            //sceneObject.transform.rotation =
            //    Quaternion.Euler(rotationOffset); 
            //sceneObject.transform.SetParent(
            //    null
            //);
        }

        foreach (var trackedImage in args.updated)
        {
            //if (sceneObject.activeSelf)
            //{
            //    sceneObject.transform.position =
            //        trackedImage.transform.TransformPoint(positionOffset);
            //}

            ApplyTransform(trackedImage);
        }
    }
    private void ApplyTransform(ARTrackedImage trackedImage)
    {
        Transform img = trackedImage.transform;

        // This point is already stable in world space — you proved it works
        Vector3 targetPosition = img.TransformPoint(positionOffset);

        // The direction from the QR to that offset point is a stable world-space vector
        Vector3 directionFromQR = targetPosition - img.position; 
        directionFromQR.y = 0; // flatten onto XZ plane
        directionFromQR.Normalize();

        // Use it to build a stable Y rotation
        Quaternion baseRotation = Quaternion.LookRotation(directionFromQR, Vector3.up);

        sceneObject.transform.position = targetPosition;
        sceneObject.transform.rotation = baseRotation * Quaternion.Euler(rotationOffset);
        //sceneObject.transform.localScale = scaleOffset;
    }
}