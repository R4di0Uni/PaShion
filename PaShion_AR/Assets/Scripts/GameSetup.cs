using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public PCHostManager pcHostManager;
    public AndroidClientManager androidClientManager;
    public QRTracker QRtracker;

    void Start()
    {
    #if UNITY_ANDROID
            androidClientManager.enabled = true;
            pcHostManager.enabled = false;

            QRtracker.enabled = true;

    #else
            pcHostManager.enabled = true;
            androidClientManager.enabled = false;

            QRtracker.enabled = false;
            QRtracker.sceneObject.SetActive(true);
    #endif
    }
}