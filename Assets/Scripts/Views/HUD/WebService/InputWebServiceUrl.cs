using InterativaSystem.Controllers.Network;

namespace InterativaSystem.Views.HUD.WebService
{
    public class InputWebServiceUrl :InputView
    {
        private Services.WebService _webService;

        protected override void OnStart()
        {
            _webService = _bootstrap.GetService(ServicesTypes.WebService) as Services.WebService;

            input.text = _webService.GetUrl();
        }

        protected override void EndEdit(string value)
        {
            _webService.SetUrl(value);
        }
    }
}