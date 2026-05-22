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
                Debug.Log("Querying sessions...");
                var results = await MultiplayerService.Instance.QuerySessionsAsync(new QuerySessionsOptions());
                Debug.Log("Sessions found: " + results.Sessions.Count);

                if (results.Sessions.Count > 0)
                {
                    string sessionId = results.Sessions[0].Id;
                    Debug.Log("Attempting to join session: " + sessionId);

                    session = await MultiplayerService.Instance.JoinSessionByIdAsync(sessionId);
                    Debug.Log("Session joined successfully.");

                    if (session == null)
                    {
                        Debug.LogError("Session is null after join!");
                        await Task.Delay((int)(retryInterval * 1000));
                        continue;
                    }

                    Debug.Log("Checking for relayCode property...");
                    if (!session.Properties.ContainsKey("relayCode"))
                    {
                        Debug.LogError("No relayCode in session properties!");
                        await Task.Delay((int)(retryInterval * 1000));
                        continue;
                    }

                    string relayCode = session.Properties["relayCode"].Value;
                    Debug.Log("Relay code retrieved: " + relayCode);

                    // Validate it's not empty
                    if (string.IsNullOrEmpty(relayCode))
                    {
                        Debug.LogError("Relay code is empty!");
                        await Task.Delay((int)(retryInterval * 1000));
                        continue;
                    }

                    var joinAllocation = await RelayService.Instance.JoinAllocationAsync(relayCode);
                    Debug.Log("Relay allocation joined.");

                    var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
                    Debug.Log("Transport: " + (transport == null ? "NULL" : "found"));

                    transport.SetClientRelayData(
                        joinAllocation.RelayServer.IpV4,
                        (ushort)joinAllocation.RelayServer.Port,
                        joinAllocation.AllocationIdBytes,
                        joinAllocation.Key,
                        joinAllocation.ConnectionData,
                        joinAllocation.HostConnectionData
                    );
                    Debug.Log("Transport configured.");

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
                Debug.LogError("Stack trace: " + e.StackTrace);
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