using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class QRTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager imageManager;

    [SerializeField]
    private GameObject modelPrefab;

    private GameObject spawnedObject;

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
            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(
                    modelPrefab,
                    trackedImage.transform.position,
                    trackedImage.transform.rotation
                );

                spawnedObject.transform.SetParent(trackedImage.transform);
            }
        }

        foreach (var trackedImage in args.updated)
        {
            if (spawnedObject != null)
            {
                spawnedObject.transform.position =
                    trackedImage.transform.position;

                spawnedObject.transform.rotation =
                    trackedImage.transform.rotation;
            }
        }
    }
}