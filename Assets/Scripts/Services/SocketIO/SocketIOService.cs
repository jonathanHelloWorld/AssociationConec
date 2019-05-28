using SocketIO;
using System.Collections.Generic;
using UnityEngine;

namespace InterativaSystem.Services
{
    [RequireComponent(typeof(SocketIOComponent))]
    public class SocketIOService : GenericService
    {
        protected SocketIOComponent _socketManager;
        protected Controllers.GenericController.SimpleEvent OnOpen, OnError, OnClose;

        protected override void OnAwake()
        {
            _socketManager = GetComponent<SocketIOComponent>();
        }

        protected override void OnStart()
        {
            base.OnStart();

            _socketManager.On("open", Open);
            _socketManager.On("error", Error);
            _socketManager.On("close", Close);

            _bootstrap.AppStarted += () =>
                {
                    if (_socketManager.autoConnect)
                        _socketManager.Connect();
                };
        }

        public virtual void Open(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);

            if (OnOpen != null)
                OnOpen();
        }

        public virtual void Error(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);

            if (OnError != null)
                OnError();
        }

        public virtual void Close(SocketIOEvent e)
        {
            Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);

            if (OnClose != null)
                OnClose();
        }
    }
}