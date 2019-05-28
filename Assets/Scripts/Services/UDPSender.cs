using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using InterativaSystem.Controllers;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Services
{
    public class UDPSender : GenericService
    {
        public int port;
        private string ip = "127.0.0.1";
        
        private UdpClient client;

#if HAS_SERVER
#endif

        void Awake()
        {
            //Mandatory set for every service
            Type = ServicesTypes.UDPSend;
        }

        protected override void OnStart() { }

        public void SendPacket(string data)
        {
            client = new UdpClient();

            try
            {
                var packet = Encoding.ASCII.GetBytes(data);

                client.Send(packet, packet.Length, ip, port);
                DebugLog("Data: " + data + "\n Sended.");

                client.Close();
            }
            catch (Exception e)
            {
                DebugLog(e.ToString());
            }
        }
        private void OnDisable()
        {
            //client.Close();
        }
        private void OnApplicationQuit()
        {
            //client.Close();
        }
    }
}