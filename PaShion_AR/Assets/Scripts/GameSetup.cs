using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public PCHostManager pcHostManager;
    public AndroidClientManager androidClientManager;
    public QRTracker qrTracker;

    void Start()
    {
    #if UNITY_ANDROID
            androidClientManager.enabled = true;
            pcHostManager.enabled = false;

            qrTracker.enabled = true;

    #else
            pcHostManager.enabled = true;
            androidClientManager.enabled = false;

            qrTracker.enabled = false;
            qrTracker.sceneObject.SetActive(true);
    #endif
    }
}