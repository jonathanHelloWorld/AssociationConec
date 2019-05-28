using UnityEngine;
using System.Collections;
using Assets.Scripts.Views.HUD;
using InterativaSystem.Controllers;
using InterativaSystem.Models;

public class ScoreTypeText : DynamicText
{
    //TODO Yuri: criar models e views para representar itens coletaveis de tipos diferentes

    private ScoreController _scoreController;
    public int type = 0;
    public float multiplier = 1;

#if HAS_SERVER
    public bool useFixedid;
    public string fixedId;
#endif

    protected override void OnStart()
    {
        _scoreController = _controller as ScoreController;
        _scoreController.OnUpdateScore += UpdateData;
    }

    protected override void UpdateData(ScoreValue value)
    {
        string nText = (value.types[type] * multiplier).ToString(format);
        UpdateText(nText);
    }
}
