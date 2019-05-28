using InterativaSystem.Services;
using UnityEngine;

namespace InterativaSystem.Views.HUD.WebService
{
    public class ButtonGetFromWebService : ButtonView
    {
        private Services.WebService _webService;

        public AccessTypes AccessType;

        protected override void OnStart()
        {
            _webService = _bootstrap.GetService(ServicesTypes.WebService) as Services.WebService;
        }

        protected override void OnClick()
        {
            base.OnClick();
#if HAS_WEBSERVICE
            var form = new WWWForm();
            
            _webService.GetJson(AccessType, Result);
#endif
        }

        void Result(WebObject value)
        {
            UnityEngine.Debug.Log(value.user.email);
        }
    }
}