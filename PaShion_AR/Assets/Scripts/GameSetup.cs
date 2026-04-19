using UnityEngine;

public class GameSetup : MonoBehaviour
{
    public PCHostManager pcHostManager;
    public AndroidClientManager androidClientManager;

    void Start()
    {
    #if UNITY_ANDROID
            androidClientManager.enabled = true;
            pcHostManager.enabled = false;
    #else
            pcHostManager.enabled = true;
            androidClientManager.enabled = false;
    #endif
    }
}