using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace InterativaSystem.Views.HUD.WebService
{
    public class ButtonAddRegistryUsserWebService : ButtonView
    {
        private Services.WebService _webService;

        private RegisterController _register;

        public List<string> RegistryFields; 

        protected override void OnStart()
        {
            _register = _controller as RegisterController;
            _webService = _bootstrap.GetService(ServicesTypes.WebService) as Services.WebService;
        }

        protected override void OnClick()
        {
            base.OnClick();
#if HAS_WEBSERVICE
            var headers = new Dictionary<string, string>();
            string value;
            for (int i = 0, n = RegistryFields.Count; i < n; i++)
            {
                if (_register.TryGetRegistryValue(RegistryFields[i], out value, true))
                {
                    headers.Add(RegistryFields[i], value);
                }
            }

            _webService.GetToken(AccessTypes.User, headers, Result);
#endif
        }

        void Result(WebObject webres)
        {
            UnityEngine.Debug.Log("Id: " + webres.user.id);
            _register.AddRegisterValue("webId", webres.user.id, false);
        }
    }
}