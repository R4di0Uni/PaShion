using extOSC;
using UnityEngine;
using System.Collections;

public class OSCTrigger : MonoBehaviour
{
    public OSCTransmitter transmitter;

    void Start()
    {
        transmitter.RemoteHost = "127.0.0.1"; // same PC
        transmitter.RemotePort = 6000;
    }

    public void SendTrigger()
    {
        StartCoroutine(TriggerPulse());
    }

    IEnumerator TriggerPulse()
    {
        // Send ON
        OSCMessage onMsg = new OSCMessage("/trigger");
        onMsg.AddValue(OSCValue.Int(1));
        transmitter.Send(onMsg);

        Debug.Log("Trigger ON sent");

        yield return new WaitForSeconds(0.05f);

        // Send OFF
        OSCMessage offMsg = new OSCMessage("/trigger");
        offMsg.AddValue(OSCValue.Int(0));
        transmitter.Send(offMsg);

        Debug.Log("Trigger OFF sent");
    }
}