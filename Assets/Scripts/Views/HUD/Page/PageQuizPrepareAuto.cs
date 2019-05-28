using UnityEngine;
using System.Collections;
using InterativaSystem.Views.HUD;
using InterativaSystem.Controllers;
using InterativaSystem;

public class PageQuizPrepareAuto : DoOnPageAuto
{
    QuizController _quizController;
    public ControllerTypes gameControllerType;

    public bool HasTypes;
    public int Type;

    protected override void OnStart()
    {
        base.OnStart();

        _quizController = _bootstrap.GetController(gameControllerType) as QuizController;
    }

    protected override void DoSomething()
    {
        if (HasTypes)
            _quizController.PrepareGame(Type);
        else
            _quizController.PrepareGame();
    }
}
