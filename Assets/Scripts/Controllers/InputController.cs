using System;
using System.Collections.Generic;
using InterativaSystem.Controllers.Sound;
using TouchScript.Gestures;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    public enum ControlerButton
    {
        Up,
        Down,
        Left,
        Right,
        Home,
        End,
        MainAxis,
        SecondaryAxis,
        MainAxisModifier,
        EyebrowLeft,
        EyebrowsRight,
        BlinkLeft,
        BlinkRight,
        CameraZoomToggle,
        CameraPositionToggle,
        FacialExpressionHappy,
        FacialExpressionSad,
        FacialExpressionAngry,
        FacialExpressionLaugh,
    }

    [AddComponentMenu("ModularSystem/Controllers/Input Controller")]
    public class InputController : GenericController
    {
#if HAS_CONTROLLERINPUT
        [Header("Controller Input Settings")]
        public bool AllowMultpleJoysticks;
        public int MaxPlayers = 1;
        public delegate void ControllerEvent(ControlerButton button, int index);
        public delegate void ControllerAxisEvent(ControlerButton button, Vector2 axis, int index);

        public event ControllerEvent OnJoystickPrees, OnJoystickRelease;
        public event ControllerAxisEvent OnJoystickAxis;
#endif

        [Header("Settings")] public bool ListenToAccelerometer;

        public List<float> FloatControllers;

        public void AddFloatController()
        {
            FloatControllers.Add(0);
        }

        public List<bool> ToogleControllers;

        //Platform Defined variables
#if UNITY_EDITOR
#endif
#if UNITY_IOS
#endif
#if UNITY_ANDROID
#endif
#if UNITY_STANDALONE
#endif

        private PanGesture _panGesture;
        private TapGesture _tapGesture;
        private PressGesture _pressGesture;
        private ReleaseGesture _releaseGesture;
        private FlickGesture _flickGesture;

        private List<PressGesture> _pressGestures;
        private List<ReleaseGesture> _releaseGestures;

        public event VectorEvent OnDeviceTurn , OnPan , OnMovementInput, OnFlick;
        public event SimpleEvent OnTap , OnPress , OnRelease;
        public event IntEvent OnPressMult , OnReleasesMult;

        protected override void OnStart()
        {
            if (!_bootstrap.IsMobile && !_bootstrap.IsEditor)
            {
                Debug.Log("Disabling Accelerometer");
                ListenToAccelerometer = false;
            }

            _bgm = _bootstrap.GetController(ControllerTypes.SoundBGM) as BGMController;
            _pressGestures = new List<PressGesture>();
            _releaseGestures = new List<ReleaseGesture>();
        }

        public void AddPanGesture(PanGesture panGesture)
        {
            _panGesture = panGesture;
            _panGesture.Panned += Panned;
        }

        public void AddFlickGesture(FlickGesture flickGesture)
        {
            _flickGesture = flickGesture;
            _flickGesture.Flicked += Flicked;
        }

        public void AddTapGesture(TapGesture tapGesture)
        {
            _tapGesture = tapGesture;
            _tapGesture.Tapped += Tapped;
        }

        public void AddPressGesture(PressGesture pressGesture)
        {
            if (_pressGesture == null)
            {
                _pressGesture = pressGesture;
                _pressGesture.Pressed += Pressed;
            }
            else
            {
                if (_pressGestures.Count == 0)
                    _pressGestures.Add(_pressGesture);

                _pressGestures.Add(pressGesture);

                pressGesture.Pressed += Pressed;
            }
        }

        public void AddPressGesture(PressGesture pressGesture, int index)
        {
            if (_pressGesture == null)
            {
                _pressGesture = pressGesture;
                _pressGesture.Pressed += Pressed;
            }
            else
            {
                if (_pressGestures.Count == 0)
                    _pressGestures.Add(_pressGesture);

                var oldList = new List<PressGesture>(_pressGestures);

                if (index == 0)
                {
                    _pressGestures = new List<PressGesture>();
                    _pressGestures.Add(pressGesture);
                    for (int i = 0; i < oldList.Count; i++)
                    {
                        _pressGestures.Add(oldList[i]);
                    }
                }
                else if (index < _pressGestures.Count)
                {
                    var n = oldList.Count;
                    _pressGestures = new List<PressGesture>();
                    for (int i = 0; i < index; i++)
                    {
                        _pressGestures.Add(oldList[i]);
                    }
                    _pressGestures.Add(pressGesture);
                    for (int i = index; i < n; i++)
                    {
                        _pressGestures.Add(oldList[i]);
                    }
                }
                else
                {
                    _pressGestures.Add(pressGesture);
                }

                pressGesture.Pressed += Pressed;
            }
        }

        public void AddReleaseGesture(ReleaseGesture releaseGesture, int index)
        {
            if (_releaseGesture == null)
            {
                _releaseGesture = releaseGesture;
                _releaseGesture.Released += Released;
            }
            else
            {
                if (_releaseGestures.Count == 0)
                    _releaseGestures.Add(_releaseGesture);

                var oldList = new List<ReleaseGesture>(_releaseGestures);

                if (index == 0)
                {
                    _releaseGestures = new List<ReleaseGesture>();
                    _releaseGestures.Add(releaseGesture);
                    for (int i = 0; i < oldList.Count; i++)
                    {
                        _releaseGestures.Add(oldList[i]);
                    }
                }
                else if (index < _releaseGestures.Count)
                {
                    var n = oldList.Count;
                    _releaseGestures = new List<ReleaseGesture>();
                    for (int i = 0; i < index; i++)
                    {
                        _releaseGestures.Add(oldList[i]);
                    }
                    _releaseGestures.Add(releaseGesture);
                    for (int i = index; i < n; i++)
                    {
                        _releaseGestures.Add(oldList[i]);
                    }
                }
                else
                {
                    _releaseGestures.Add(releaseGesture);
                }


                _releaseGestures.Add(releaseGesture);

                releaseGesture.Released += Released;
            }
            //_releaseGesture = releaseGesture;
            //_releaseGesture.Released += Released;
        }

        public void AddReleaseGesture(ReleaseGesture releaseGesture)
        {
            if (_releaseGesture == null)
            {
                _releaseGesture = releaseGesture;
                _releaseGesture.Released += Released;
            }
            else
            {
                if (_releaseGestures.Count == 0)
                    _releaseGestures.Add(_releaseGesture);

                _releaseGestures.Add(releaseGesture);

                releaseGesture.Released += Released;
            }
        }

        private void Panned(object sender, EventArgs e)
        {
            if (OnPan != null) OnPan(_panGesture.LocalDeltaPosition);
        }
        private void Flicked(object sender, EventArgs e)
        {
            if (OnFlick != null) OnFlick(_flickGesture.ScreenFlickVector);
        }

        private void Tapped(object sender, EventArgs e)
        {
            if (OnTap != null) OnTap();
        }

        private void Pressed(object sender, EventArgs e)
        {
            var pg = sender as PressGesture;
            if (_pressGestures.Count == 0 && OnPress != null) OnPress();
            else if (OnPressMult != null) OnPressMult(_pressGestures.FindIndex(x => x == pg));
        }

        private void Released(object sender, EventArgs e)
        {
            var rg = sender as ReleaseGesture;
            if (OnRelease != null) OnRelease();
            if (_releaseGestures.Count == 0 && OnRelease != null) OnRelease();
            else if (OnReleasesMult != null) OnReleasesMult(_releaseGestures.FindIndex(x => x == rg));
        }

        #region MonoBehaviour Methods

        private void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.Input;

            ToogleControllers.Add(false);
            ToogleControllers.Add(false);
        }

        private void Update()
        {
            CheckKeyboard();
#if HAS_CONTROLLERINPUT
            CheckButton();
#endif
            CheckAxis();

            if (ListenToAccelerometer)
                GetDeviceAccelerometer();
        }

        #endregion

        #region Real Time Listening

        private void CheckAxis()
        {
            var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (OnMovementInput != null) OnMovementInput(move);

#if HAS_CONTROLLERINPUT
            var priAxis = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

            if (OnJoystickAxis != null) OnJoystickAxis(ControlerButton.MainAxis, priAxis, controllerIndex);
#endif
        }

        private void GetDeviceAccelerometer()
        {
            Vector3 vector;

#if (UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL)
            vector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
#elif (UNITY_IOS || UNITY_ANDROID)
            vector = Input.acceleration;
#endif
            if (OnDeviceTurn != null) OnDeviceTurn(vector);
        }

        private BGMController _bgm;

        private void CheckKeyboard()
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                ToogleControllers[1] = !ToogleControllers[1];

                if (_bgm.IsPlaying)
                    _bgm.StopSound();
                else
                    _bgm.PlaySound();
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                ToogleControllers[0] = !ToogleControllers[0];
            }
        }

#if HAS_CONTROLLERINPUT
        private int controllerIndex;
        private void CheckButton()
        {
            var bts = Enum.GetNames(typeof(ControlerButton));

            for (int i = 0; i < bts.Length; i++)
            {
                if (Input.GetButtonDown(bts[i]))
                {
                    if (OnJoystickPrees != null) OnJoystickPrees((ControlerButton)i, controllerIndex);
                }
                if (Input.GetButtonUp(bts[i]))
                {
                    if (OnJoystickRelease != null) OnJoystickRelease((ControlerButton)i, controllerIndex);
                }
            }
        }
#endif

        #endregion
    }
}