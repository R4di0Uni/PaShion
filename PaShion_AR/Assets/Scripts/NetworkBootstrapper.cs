using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class NetworkBootstrapper : MonoBehaviour
{
    public static NetworkBootstrapper Instance { get; private set; }
    public bool IsReady { get; private set; } = false;

    async void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        await InitializeServices();
    }

    async Task InitializeServices()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

            Debug.Log("UGS ready. Player ID: " + AuthenticationService.Instance.PlayerId);
            IsReady = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("UGS init failed: " + e.Message);
        }
    }
}