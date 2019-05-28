using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using InterativaSystem.Views.HUD;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using UnityEngine;
using UnityEngine.UI;

namespace InterativaSystem.Views.EnviromentComponents
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(BoxCollider))]
    [RequireComponent(typeof(Transformer2D))]
    [RequireComponent(typeof(PanGesture))]
    public class AssociationObject : GenericView
    {
        private AssociationController _associationController;

        public int Type;

        public int Id;
        public List<int> Ids;

        [HideInInspector]
        public int QuestionId;

        [Space]

        public bool IsStatic;

        public Text text;

        protected Vector3 _iniPos;
        protected Vector3 _iniEuler;
        protected PanGesture _pg;
        protected bool _isOver;
        protected bool _isPanning;

        [HideInInspector]
        public bool IsAssociated;
        protected bool _once;
        protected GameObject _lastOver;
        protected GameObject _lastAssocForFeedback;

        protected override void OnAwake()
        {
            base.OnAwake();
            _iniPos = transform.position;
            _iniEuler = transform.eulerAngles;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _associationController = _controller as AssociationController;

            if (!IsStatic)
                _associationController.AddAssociationObject(this);

            _associationController.OnGameStart += Started;
            _associationController.Reset += ResetView;
            _associationController.OnGameEnd += Ended;

            var bx = GetComponent<BoxCollider>();
            var porcent = 0.8f;
            if (IsStatic)
                bx.size = new Vector3(60, 20, 1);
            else
                bx.size = new Vector3(GetComponent<RectTransform>().rect.width * porcent, GetComponent<RectTransform>().rect.height * porcent, 1);

            EnableStatic();
        }

        IEnumerator _Initialize(AssociationPiece piece)
        {
            var rb = GetComponent<Rigidbody>();
            rb.useGravity = false;
            rb.WakeUp();
            rb.isKinematic = true;
            yield return null;
            
            var tranf = GetComponent<Transformer2D>();
            tranf.Speed = 25;

            _pg = GetComponent<PanGesture>();

            if (!string.IsNullOrEmpty(piece.sprite))
            {
                var image = GetComponent<Image>();
                if (!image) image = gameObject.AddComponent<Image>();

                image.sprite = Resources.Load<Sprite>(piece.sprite);
            }

            Id = piece.id;

            IsStatic = piece.isStatic;

            _controllerType = ControllerTypes.Association;


            Initialize();
        }

        public virtual void Initialize(AssociationPiece piece)
        {
			//StartCoroutine(_Initialize(piece));
			//return;
			Debug.Log("dsds");
            var rb = GetComponent<Rigidbody>();
            if (!rb) rb = gameObject.AddComponent<Rigidbody>();
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.WakeUp();

            var bx = GetComponent<BoxCollider>();
            if (!bx) bx = gameObject.AddComponent<BoxCollider>();

            var tranf = GetComponent<Transformer2D>();
            if (!tranf) tranf = gameObject.AddComponent<Transformer2D>();
            tranf.Speed = 25;

            _pg = GetComponent<PanGesture>();
            if (!_pg) _pg = gameObject.AddComponent<PanGesture>();

            if (!string.IsNullOrEmpty(piece.sprite))
            {
                var image = GetComponent<Image>();
                if (!image) image = gameObject.AddComponent<Image>();

                image.sprite = Resources.Load<Sprite>(piece.sprite);
                image.SetNativeSize();

                if (piece.useSize)
                {
                    image.type = Image.Type.Sliced;
                }

                if (piece.isStatic)
                    image.color = new Color(1,1,1,1);
            }

            if (piece.useSize)
            {
                var rect = GetComponent<RectTransform>();
                rect.sizeDelta = piece.size;
            }

            if (!string.IsNullOrEmpty(piece.text))
            {
                //text.text = piece.text;
            }

            Id = piece.id;
            Ids = new List<int>(piece.ids);

            IsStatic = piece.isStatic;

            _controllerType = ControllerTypes.Association;

            Initialize();
        }

        protected override void OnDestroied()
        {
            base.OnDestroied();

            if (_associationController == null) return;

            _associationController.OnGameStart -= Started;
            _associationController.Reset -= ResetView;
            _associationController.OnGameEnd -= Ended;
        }

        protected override void ResetView()
        {
            transform.DOPause();

            base.ResetView();
            IsAssociated = false;
            GetComponent<BoxCollider>().enabled = true;

            if (_lastOver != null && _lastOver.GetComponent<ColliderView>() != null)
                _lastOver.GetComponent<ColliderView>().enabled = true;

            if (!IsStatic)
            {
                transform.position = _iniPos;
                transform.eulerAngles = _iniEuler;

                GetComponent<PanGesture>().enabled = true;
                GetComponent<Transformer2D>().enabled = true;
                _lastOver = null;
            }
        }

        protected virtual void EnableStatic()
        {
            if (IsStatic)
            {
                GetComponent<PanGesture>().enabled = false;
                GetComponent<Transformer2D>().enabled = false;
                GetComponent<BoxCollider>().isTrigger = true;
            }
            else
            {
                _pg = GetComponent<PanGesture>();
                _pg.PanStarted += StartPan;
                _pg.PanCompleted += EndPan;
                _iniPos = transform.position;

                if(_associationController.WaitToShowFeedback)
                    _associationController.OnAssociationFeedback += CheckAssociation;
            }
        }

        protected virtual void StartPan(object sender, EventArgs e)
        {
            _isPanning = true;
            _once = true;
        }
        protected virtual void EndPan(object sender, EventArgs e)
        {
            _isPanning = false;

            if (!_once)
            {
                return;
            }

            _once = false;

            if (!_isOver)
            {
                Return();
                return;
            }

            if (_associationController.WaitToShowFeedback)
            {
                GetComponent<RectTransform>().DOMove(_lastOver.transform.position, 0.3f).SetEase(Ease.OutCubic).Play();
                GetComponent<PanGesture>().enabled = false;
                GetComponent<Transformer2D>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;

                if (_lastOver.GetComponent<ColliderView>() != null)
                    _lastOver.GetComponent<ColliderView>().enabled = false;
                if (_lastOver.GetComponent<Collider>() != null)
                    _lastOver.GetComponent<Collider>().enabled = false;

                _lastAssocForFeedback = _lastOver;

                _associationController.WaitAssociated();
            }
            else if(Id == _lastOver.GetComponent<AssociationObject>().Id || _lastOver.GetComponent<AssociationObject>().Ids.Contains(Id))
            {
                IsAssociated = true;

                _associationController.Associated(true);

                GetComponent<RectTransform>().DOMove(_lastOver.transform.position, 0.3f).SetEase(Ease.OutCubic).Play();
                GetComponent<PanGesture>().enabled = false;
                GetComponent<Transformer2D>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;
                
                if (_lastOver.GetComponent<ColliderView>() != null)
                    _lastOver.GetComponent<ColliderView>().enabled = false;

                _lastOver.GetComponent<BoxCollider>().enabled = false;
                Associated();
            }
            else
            {
                Return();
            }
        }

        void CheckAssociation()
        {
            if(_lastAssocForFeedback == null) return;

            if (Id == _lastAssocForFeedback.GetComponent<AssociationObject>().Id || _lastAssocForFeedback.GetComponent<AssociationObject>().Ids.Contains(Id))
            {
                IsAssociated = true;

                _associationController.Associated(true);

                GetComponent<RectTransform>().DOMove(_lastAssocForFeedback.transform.position, 0.3f).SetEase(Ease.OutCubic).Play();
                GetComponent<PanGesture>().enabled = false;
                GetComponent<Transformer2D>().enabled = false;
                GetComponent<BoxCollider>().enabled = false;

                if (_lastAssocForFeedback.GetComponent<ColliderView>() != null)
                    _lastAssocForFeedback.GetComponent<ColliderView>().enabled = false;

                _lastAssocForFeedback.GetComponent<BoxCollider>().enabled = false;
                Associated();
            }
            else
            {
                GetComponent<PanGesture>().enabled = true;
                GetComponent<Transformer2D>().enabled = true;
                GetComponent<BoxCollider>().enabled = true;

                if (_lastAssocForFeedback.GetComponent<ColliderView>() != null)
                    _lastAssocForFeedback.GetComponent<ColliderView>().enabled = true;

                if (_lastAssocForFeedback.GetComponent<Collider>() != null)
                    _lastAssocForFeedback.GetComponent<Collider>().enabled = true;

                _associationController.WaitReturn();
                Return();
            }

            _lastAssocForFeedback = null;
        }

        protected virtual void Associated(){ }

        void Return()
        {
            if (!_associationController.WaitToShowFeedback)
            {
                _associationController.Associated(false);
            }

            GetComponent<RectTransform>()
                .DOMove(_iniPos, 0.3f)
                .SetEase(Ease.OutCubic)
                .Play();
        }

        void OnTriggerEnter(Collider other)
        {
            if (!_isPanning) return;

            if (other.GetComponent<AssociationObject>() != null && other.GetComponent<AssociationObject>().IsStatic)
            {
                _lastOver = other.gameObject;
                _isOver = true;
                _once = true;
            }
        }
        void OnTriggerExit(Collider other)
        {
            _lastOver = null;
            _isOver = false;
        }

        void Started()
        {

        }

        void Ended()
        {
            if (IsAssociated || IsStatic) return;

            //transform.DOLocalMoveY(-1000, 1.8f).SetDelay(0.6f).Play();
            //transform.DOLocalRotate(Vector3.up * 900, 1.6f).SetDelay(0.6f).SetRelative().Play();
        }
    }
}