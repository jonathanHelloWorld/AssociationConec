using UnityEngine;
using System.Collections;
using InterativaSystem.Views.HUD;
using InterativaSystem.Controllers;

public class AssociationImageById : ImageView
{

    public Sprite[] newSprites;
    private AssociationController _associationController;

    protected override void OnStart()
    {
        base.OnStart();

        _associationController = _controller as AssociationController;
        _associationController.OnContentChosed += AssociationImage;
    }

    void AssociationImage(int id)
    {
        _image.sprite = newSprites[id];
    }
}
