using System;
using System.Collections.Generic;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Interfaces;
using InterativaSystem.Views.Grid;
using InterativaSystem.Views.HUD;
using InterativaSystem.Views.HUD.Mosaic;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Models
{
    public class MosaicData : InfoModel
    {
        //public GridCanvas2D Grid;

        protected GridData Grid;
        public List<Sprite> Sprites;

        private MosaicController _mosaicController;

        void Awake()
        {
            Type = ModelTypes.Mosaic;
        }

        //protected List<GridPiece> pieces;
        protected List<GridPoint> pieces;
        
        protected int actual;

        protected override void OnStart()
        {
            base.OnStart();

            _mosaicController = _bootstrap.GetController(ControllerTypes.Mosaic) as MosaicController;
            Grid = _bootstrap.GetModel(ModelTypes.Grid) as GridData;

            if (Grid.gridGenerated)
                GeneretaMosaic();
            else
                Grid.GridGenerated += GeneretaMosaic;
        }

        private void GeneretaMosaic()
        {
            pieces = new List<GridPoint>(Grid.grid.Points);
            pieces.Shuffle(_mosaicController.Seed);

            pieces.RemoveAll(x => !x.isValid);
            for (int x = 0; x < pieces.Count; x++)
            {
                var temp = new GameObject("obj:" + x);
                temp.transform.SetParent(_mosaicController.IniRoot);

                var img = temp.AddComponent<MosaicView>();
                img.SetColor(new Color(1, 1, 1, 0));
                img.HighlightPos = _mosaicController.HighlightPos;
                img.OutPos = _mosaicController.OutPos;
                img.Root = _mosaicController.Root;
                img.iniPos = pieces[x].worldPosition;
                img.Initialize();

                temp.transform.localEulerAngles = Vector3.zero;
                temp.transform.localScale = Vector3.one;
                temp.transform.position = pieces[x].worldPosition;
                ((RectTransform)temp.transform).sizeDelta = new Vector2(Grid.grid.PointSize, Grid.grid.PointSize);

                pieces[x].objScript = img;
            }

        }

        void OnLoaded(List<Texture2D> pics)
        {
            if (Sprites == null)
                Sprites = new List<Sprite>();

            for (int i = 0, n = Sprites.Count; i < pics.Count; i++)
            {
                Sprites.Add(pics[i].ToSprite());

                if(pieces.Count > (n + i))
                    ((MosaicView)pieces[n + i].objScript).Animate(Sprites.Last(), i);
            }
        }

        public void LoadDatas()
        {
            _IOController.TryLoad(true, OnLoaded);
        }
    }
}