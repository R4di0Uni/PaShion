using UnityEngine;
using Unity.Services.Relay;
using Unity.Services.Multiplayer;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using System.Threading.Tasks;

public class AndroidClientManager : MonoBehaviour
{
    public float retryInterval = 3f;
    private ISession session;

    async void Start()
    {
        while (!NetworkBootstrapper.Instance.IsReady)
            await Task.Delay(100);

        await FindAndJoin();
    }

    async Task FindAndJoin()
    {
        while (true)
        {
            try
            {
                var results = await MultiplayerService.Instance.QuerySessionsAsync(
                    new QuerySessionsOptions()
                );

                Debug.Log("Sessions found: " + results.Sessions.Count);

                if (results.Sessions.Count > 0)
                {
                    string sessionId = results.Sessions[0].Id;
                    Debug.Log("Joining session: " + sessionId);

                    // Step 1 — Join the lobby session
                    session = await MultiplayerService.Instance.JoinSessionByIdAsync(sessionId);
                    Debug.Log("Session joined.");

                    // Step 2 — Read relay code stored by PC host
                    if (!session.Properties.ContainsKey("relayCode"))
                    {
                        Debug.LogError("Session has no relayCode property!");
                        await Task.Delay((int)(retryInterval * 1000));
                        continue;
                    }

                    string relayCode = session.Properties["relayCode"].Value;
                    Debug.Log("Relay code: " + relayCode);

                    // Step 3 — Join Relay allocation using the code
                    var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);
                    Debug.Log("Relay allocation joined.");

                    // Step 4 — Configure UnityTransport with Relay data
                    var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                    transport.SetClientRelayData(
                        joinAllocation.RelayServer.IpV4,
                        (ushort)joinAllocation.RelayServer.Port,
                        joinAllocation.AllocationIdBytes,
                        joinAllocation.Key,
                        joinAllocation.ConnectionData,
                        joinAllocation.HostConnectionData
                    );
                    Debug.Log("Transport configured.");

                    // Step 5 — Start NGO client
                    NetworkManager.Singleton.StartClient();
                    Debug.Log("Client started.");
                    return;
                }
                else
                {
                    Debug.Log("No session found, retrying in " + retryInterval + "s...");
                    await Task.Delay((int)(retryInterval * 1000));
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError("FindAndJoin error: " + e.Message);
                await Task.Delay((int)(retryInterval * 1000));
            }
        }
    }

    async void OnDestroy()
    {
        if (session != null)
            await session.LeaveAsync();
    }
}