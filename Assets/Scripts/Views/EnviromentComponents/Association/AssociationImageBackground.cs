using UnityEngine;
using System.Collections;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.EnviromentComponents;

public class AssociationImageBackground : ImageView {

    public AssociationObjectContent aoc;
    public Sprite[] newSpriteBackground;

    protected override void OnUpdate()
    {
        base.OnUpdate();

        _image.sprite = newSpriteBackground[aoc.Id];
    }
}
