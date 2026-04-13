using UnityEngine;
using UnityEngine.UI;

public class MobileDebug : MonoBehaviour
{
    public static MobileDebug Instance;
    public Text debugText;

    void Awake()
    {
        Instance = this;
        debugText = GetComponent<Text>();
    }

    public void Log(string msg)
    {
        debugText.text = msg;
    }
}