using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Views.HUD.Network.Server.Page
{
    public class NetworkPageControlButton : ButtonView
    {
        [Space]
        public PageButtonOption option;
        [Space]
        public int Especific;
        public PageType Type;
        [Space]
        public NetworkInstanceType SendTo = NetworkInstanceType.Null;

        private NetworkServerController _newNetworkServer;
        
        protected override void OnStart()
        {
            _newNetworkServer = _controller as NetworkServerController;
        }

        protected override void OnClick()
        {
            base.OnClick();

            switch (option)
            {
                case PageButtonOption.Next:
                    if (SendTo != NetworkInstanceType.Null)
                        _newNetworkServer.SendMessageToType("NetworkGoToNextPage", "", SendTo);
                    else
                        _newNetworkServer.SendMessageToAll("NetworkGoToNextPage", "");
                    break;
                case PageButtonOption.Previous:
                    if (SendTo != NetworkInstanceType.Null)
                        _newNetworkServer.SendMessageToType("NetworkGoToPreviousPage", "", SendTo);
                    else
                        _newNetworkServer.SendMessageToAll("NetworkGoToPreviousPage", "");
                    break;
                case PageButtonOption.Especific:
                    if (SendTo != NetworkInstanceType.Null)
                        _newNetworkServer.SendMessageToType("NetworkOpenPageById", JsonConvert.SerializeObject(Especific), SendTo);
                    else
                        _newNetworkServer.SendMessageToAll("NetworkOpenPageById", JsonConvert.SerializeObject(Especific));
                    break;
                case PageButtonOption.Type:
                    if (SendTo != NetworkInstanceType.Null)
                        _newNetworkServer.SendMessageToType("NetworkOpenPageByType", JsonConvert.SerializeObject(Type), SendTo);
                    else
                        _newNetworkServer.SendMessageToAll("NetworkOpenPageByType", JsonConvert.SerializeObject(Type));
                    break;
            }
        }
    }
}