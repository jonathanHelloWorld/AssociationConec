using InterativaSystem.Controllers;

namespace InterativaSystem.Interfaces
{
    public interface IController
    {
        event GenericController.SimpleEvent Reset, ResetDependencies, OnGameStart, OnGamePrepare, OnGamePause, OnGameResume, OnGameEnd, ObstacleCollision, OnInitializationEnd;
        event GenericController.IntEvent CallGenericAction, GenericActionEnded;

        ControllerTypes Type { get; }

        bool IsGameStarted{ get; }
        bool IsGameRunning { get; }

        float GameTime { get; }

        void PrepareGame();
        void EndGame();
        void StartGame();
        void PauseGame();
        void ResumeGame();
        void ResetGame();

        void CallAction(int id);
        void ActionEnded(int id);
    }
}