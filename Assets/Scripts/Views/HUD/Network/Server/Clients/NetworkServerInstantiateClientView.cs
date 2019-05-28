using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Network.Server.Clients
{
    public class NetworkServerInstantiateClientView : GenericView
    {
        private NetworkServerController _serverController;
        private ResourcesDataBase _resources;

        public NetworkInstanceType Type;

        private List<NetworkClientPrefabStructure> _instances;


        protected override void OnStart()
        {
            _serverController = _controller as NetworkServerController;

            _resources = _bootstrap.GetModel(ModelTypes.Resources) as ResourcesDataBase;

            _instances = new List<NetworkClientPrefabStructure>();

            _serverController.OnAddClient += InstantiateClient;
            _serverController.OnClientStatsChange += InstantiateClient;
            _serverController.OnRemoveClient += RemoveClient;
        }

        private void RemoveClient(NetworkClientObject client)
        {
            var delete = _instances.FindAll(x => x.GetUid() == client.uid);
            
            for (int i = 0, n=delete.Count; i < n; i++)
            {
                Destroy(delete[i].gameObject);
            }

            _instances.RemoveAll(x => x.GetUid() == client.uid);
        }

        private void InstantiateClient(NetworkClientObject client)
        {
            if(client.networkType != Type)return;

            var session = _serverController.GetSessionId();

            if (!string.IsNullOrEmpty(client.lastUid) && _instances.Exists(x => x.GetUid() == client.lastUid))
            {
                _instances.Find(x => x.GetUid() == client.lastUid).SetUid(client.uid);
            }
            else if (_instances.Exists(x => x.GetUid() == client.uid))
            {
                _instances.Find(x => x.GetUid() == client.uid).UpdateFields();
                //UnityEngine.Debug.LogError("EEEE");
            }
            else //if(_serverController.GetInSession(client.uid))
            {
                var temp = Instantiate(Resources.Load<GameObject>(_resources.Prefabs.Find(x => x.category == PrefabCategory.UIStructure).name));

                temp.transform.SetParent(this.transform);
                temp.transform.localScale = Vector3.one;
                temp.transform.localPosition = Vector3.zero;

                _instances.Add(temp.GetComponent<NetworkClientPrefabStructure>());

                _instances.Last().Type = Type;
                _instances.Last().SetUid(client.uid);
                _instances.Last().Initialize();
            }
        }
    }
}