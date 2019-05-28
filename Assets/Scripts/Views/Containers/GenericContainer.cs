using UnityEngine;
using System.Collections;
using InterativaSystem.Views;
using InterativaSystem.Controllers;
using UnityEngine.UI;
using InterativaSystem.Interfaces;

namespace InterativaSystem.Views
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Mask))]
    public class GenericContainer : GenericView
    {
        //GenericController _controller;

        protected override void OnStart()
        {
            base.OnStart();

            ((IContainer)_controller).Container = transform;
        }
    }
}