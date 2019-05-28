using InterativaSystem.Services;
using UnityEngine;

namespace InterativaSystem.Views.DebugObject
{
    public class DebugUDPReceived : GenericView
    {
        private UDPReceive receiver;
        private UDPSender sender;

        protected override void OnStart()
        {
            base.OnStart();

            receiver = _bootstrap.GetService(ServicesTypes.UDPRead) as UDPReceive;
            receiver.OnPacketReceive += PrintDebug;
            
            sender = _bootstrap.GetService(ServicesTypes.UDPSend) as UDPSender;
            sender.SendPacket("Hello World");
        }

        private void PrintDebug(string value)
        {
            Debug.Log(value);
        }
    }
}