using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using InterativaSystem.Controllers;

namespace InterativaSystem.Services
{
    public class UDPReceive : GenericService
    {
        public int port; 
        private string lastReceivedUDPPacket = "";

        private Thread receiveThread;
        private UdpClient client;

        public event GenericController.StringEvent OnPacketReceive;

#if HAS_SERVER
#endif

        void Awake()
        {
            //Mandatory set for every service
            Type = ServicesTypes.UDPRead;
        }

        protected override void OnStart()
        {
            Init();
        }
        public string getLastPacket()
        {
            return lastReceivedUDPPacket;
        }

        private void Init()
        {
            receiveThread = new Thread( new ThreadStart(ReceiveData));

            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log("UDP reading started on port " + port);
        }
        
        public string GetLastUdpPacket()
        {
            return lastReceivedUDPPacket;
        }
        private void OnDisable()
        {
            if (receiveThread != null)
                receiveThread.Abort();
            client.Close();
        }
        private void OnApplicationQuit()
        {
            if (receiveThread != null)
                receiveThread.Abort();
            client.Close();
        }

#region Off mainThread Methods
        private void ReceiveData()
        {
            try
            {
                client = new UdpClient(port);
            }
            catch (Exception e)
            {

                Debug.Log(e.ToString());
                throw;
            }

            while (true)
            {
                try
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 8091);
                    byte[] data = client.Receive(ref endPoint);
                    lastReceivedUDPPacket = Encoding.ASCII.GetString(data);

                    if (OnPacketReceive != null) OnPacketReceive(lastReceivedUDPPacket);
                }
                catch (Exception err)
                {
                    Debug.Log(err.ToString());
                }
            }
        }
#endregion
    }
}