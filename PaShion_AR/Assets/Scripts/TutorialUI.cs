using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance { get; private set; }

    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;

    bool tutorial1done = false;
    bool tutorial2done = false;
    bool tutorial3done = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
#if UNITY_ANDROID
        tutorial1.SetActive(true);
#else
        gameObject.SetActive(false);
#endif
    }

    public void TutorialCompleted(int n)
    {
        switch (n)
        {
            case 1:
                if (tutorial1done) break;
                tutorial1done = true;
                tutorial1.SetActive(false);
                tutorial2.SetActive(true);
                break;
            case 2:
                if (tutorial2done) break;
                tutorial2done = true;
                tutorial2.SetActive(false);
                tutorial3.SetActive(true);
                break;
            case 3:
                if (tutorial3done) break;
                tutorial3done = true;
                tutorial3.SetActive(false);
                gameObject.SetActive(false);
                break;
        }
    }
}