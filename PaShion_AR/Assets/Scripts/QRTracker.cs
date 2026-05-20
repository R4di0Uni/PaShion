using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class QRTracker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager imageManager;

    [SerializeField]
    private GameObject modelPrefab;

    [SerializeField] private Vector3 positionOffset = Vector3.zero;
    [SerializeField] private Vector3 scaleOffset = Vector3.one;

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
                    trackedImage.transform.position + positionOffset,
                    trackedImage.transform.rotation
                );
                spawnedObject.transform.SetParent(trackedImage.transform);
                //spawnedObject.transform.localScale = scaleOffset;
            }
        }

        foreach (var trackedImage in args.updated)
        {
            if (spawnedObject != null)
            {
                spawnedObject.transform.position =
                    trackedImage.transform.position + positionOffset;

                spawnedObject.transform.rotation =
                    trackedImage.transform.rotation;
            }
        }
    }
}