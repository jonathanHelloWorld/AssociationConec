using System.Collections.Generic;
using InterativaSystem.Controllers;
using InterativaSystem.Controllers.Network;
using InterativaSystem.Models;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.HUD.Ranking
{
    public class RankingImageShowPosition : ImageView
    {
        private RankingController _rankingController;
        private bool playAudioOnce;

        public bool PlaySoundForWinner = true;

        public Sprite[] Positions;

        protected override void OnStart()
        {
            _rankingController = _controller as RankingController;
            _rankingController.OnScoreUpdate += OnUpdateImage;
        }
        protected override void ResetView()
        {
            playAudioOnce = false;
        }

        protected void OnUpdateImage(List<ScoreValue> listvalues)
        {
#if HAS_SERVER
            int index;

            if (_rankingController.TryGetPosition(out index))
            {
                if (!playAudioOnce && PlaySoundForWinner && index == 0 && _sfxController != null)
                {
                    playAudioOnce = true;
                    _sfxController.PlaySound("Win");
                }

                _image.color = Color.white;
                _image.sprite = Positions[index];
            }
            else
            {
                _image.color = new Color(0, 0, 0, 0);
            }
#endif
        }
    }
}