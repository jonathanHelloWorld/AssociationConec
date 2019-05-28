namespace InterativaSystem.Views.HUD.Movie
{
    public class AutoPlayMovie : MovieTextureView
    {
#if UNITY_EDITOR_WIN
        protected override void OnStart()
        {
            base.OnStart();

            Play();
        }
#endif
    }
}