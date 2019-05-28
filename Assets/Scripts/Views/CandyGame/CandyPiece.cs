using UnityEngine;
using UnityEngine.UI;
using System;
using InterativaSystem.Controllers;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.Grid;
using InterativaSystem;
using InterativaSystem.Enums;
using System.Linq;
using TouchScript;

namespace Interativa.Views.CandyGame
{
    public enum CandyType
    {
        Normal,
        Vertical,
        Horizontal,
        Mega
    }

    public class CandyPiece : PressView
    {
        /*public RectTransform rectTransform;
        public Image image;
        public GridPiece gridPiece;

        public int uniqueId = 0;
        public int id = 0;

        public CandyType candyType;

        CandyController _candyController;
        ITouch touch;

        // Use this for initialization
        protected override void OnStart()
        {
            base.OnStart();

            _candyController = _bootstrap.GetController(ControllerTypes.CandyGame) as CandyController;

            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();

            candyType = CandyType.Normal;

            _pressGesture.Pressed += PressedHandler;
            _releaseGesture.Released += ReleaseHandler;
        }

        void PressedHandler(object sender, EventArgs e)
        {
            if (_candyController.autoRunning || _candyController.isSelecting) return;

            touch = _pressGesture.ActiveTouches.Last();

            StartCoroutine(_candyController.PlayAudio(_candyController.pieceClickSfx, 0));
            _candyController.mouseStartPos = touch.Position;
            _candyController.touch = touch;
            _candyController.candyStart = this;
        }

        void ReleaseHandler(object sender, EventArgs e)
        {
            touch = null;
            if (_candyController.autoRunning) return;

            if (_candyController.candyStart == this)
                _candyController.CheckMatch();
        }

        public void KillPiece(float yPos)
        {
            Vector3 pos = transform.localPosition;
            pos.y = yPos;

            gameObject.SetActive(false);
            transform.localPosition = pos;
        }

        public void ResetPiece(float yPos)
        {
            KillPiece(yPos);
        }*/
    }
}