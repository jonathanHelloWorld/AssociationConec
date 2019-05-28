using System.Collections.Generic;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Views.ControllableCharacters.Actor
{
    public class ActorHead : Actor
    {
        //0 - Axis - Head
        //1 2 - Blinks
        //3 4 - EyeBrows
        //5 6 7 8 - Expressions

        public Transform HeadRootAxis;
        public Vector2 HeadMaxRotation;
        public Animator animator;

        protected override void OnAxis(ControlerButton button, Vector2 axis)
        {
            base.OnAxis(button, axis);

            if (button == ControlerButton.MainAxis)
                HeadRootAxis.localEulerAngles = new Vector3(axis.y * HeadMaxRotation.y, axis.x * HeadMaxRotation.x, 0);
        }

        protected override void OnPress(ControlerButton button)
        {
            base.OnPress(button);
            
            if (animator == null) return;

            animator.SetTrigger(button.ToString());
        }
    }
}