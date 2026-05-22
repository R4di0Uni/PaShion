using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Services.Multiplayer;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Collections.Generic;
using System.Threading.Tasks;

public class PCHostManager : MonoBehaviour
{
    public int maxPlayers = 8;
    public TMPro.TextMeshProUGUI joinCodeText;

    private IHostSession hostSession;

    async void Start()
    {
        while (!NetworkBootstrapper.Instance.IsReady)
            await Task.Delay(100);

        await StartHost();
    }

    async Task StartHost()
    {
        try
        {
            // Step 1 — Create Relay allocation
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(maxPlayers);
            string relayCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Relay code: " + relayCode);

            // Step 2 — Configure transport
            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.SetRelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );
            Debug.Log("Transport configured.");

            // Step 3 — Start NGO host and wait for it to be ready
            if (!NetworkManager.Singleton.IsHost && !NetworkManager.Singleton.IsServer)
            {
                NetworkManager.Singleton.StartHost();

                // Wait until host is actually listening
                float timeout = 5f;
                float elapsed = 0f;
                while (!NetworkManager.Singleton.IsHost && elapsed < timeout)
                {
                    await Task.Delay(100);
                    elapsed += 0.1f;
                }

                if (!NetworkManager.Singleton.IsHost)
                {
                    Debug.LogError("Host failed to start within timeout.");
                    return;
                }

                Debug.Log("Host confirmed listening.");
            }

            // Step 4 — NOW create session with relay code
            // Host is definitely running at this point
            var sessionOptions = new SessionOptions
            {
                MaxPlayers = maxPlayers,
                IsPrivate = false,
                SessionProperties = new Dictionary<string, SessionProperty>
            {
                { "relayCode", new SessionProperty(relayCode, VisibilityPropertyOptions.Public) }
            }
            };

            hostSession = (await MultiplayerService.Instance.CreateSessionAsync(sessionOptions)).AsHost();
            Debug.Log("Session created: " + hostSession.Id);
            Debug.Log("Relay code stored in session: " + relayCode);

            if (joinCodeText != null)
                joinCodeText.text = "Code: " + relayCode;
        }
        catch (System.Exception e)
        {
            Debug.LogError("StartHost failed: " + e.Message);
            Debug.LogError("Stack trace: " + e.StackTrace);
        }
    }

    async void OnDestroy()
    {
        if (hostSession != null)
            await hostSession.LeaveAsync();
    }
}