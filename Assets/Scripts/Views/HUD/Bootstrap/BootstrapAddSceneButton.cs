namespace InterativaSystem.Views.HUD.Bootstrap
{
    public class BootstrapAddSceneButton : ButtonView
    {
        public int SceneId;

        protected override void OnStart()
        {
            base.OnStart();

            _bootstrap.OnSceneLoaded += Hide;
        }

        void Hide()
        {
            gameObject.SetActive(false);
        }

        protected override void OnClick()
        {
            base.OnClick();

            _bootstrap.AddScene(SceneId);
        }
    }
}