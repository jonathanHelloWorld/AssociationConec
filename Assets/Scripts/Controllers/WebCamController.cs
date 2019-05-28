using System;
using UnityEngine;
using System.Collections;
using InterativaSystem.Controllers.Sound;
using InterativaSystem.Models;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/web Cam Controller")]
    public class WebCamController : GenericController
    {
        public enum Resolutions
        {
            FullHD,
            HD,
            Vga
        }

        public SimpleEvent OnWebCanStarted, OnStartTakePicture;

        private SFXController _sfx;
        private IOController _io;

        public bool TakeFromWebCam;
        public bool TakeFromScreen;

        [Space]
        public Resolutions CamResolution;

        [HideInInspector]
        public WebCamTexture WebCam;

        public int CounterTime;
        public string RegisterName;

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.WebCam;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _sfx = _bootstrap.GetController(ControllerTypes.SoundSFX) as SFXController;
            _io = _bootstrap.GetController(ControllerTypes.IO) as IOController;

            switch (CamResolution)
            {
                case Resolutions.FullHD:
                    WebCam = new WebCamTexture(1920, 1080);
                    break;
                case Resolutions.HD:
                    WebCam = new WebCamTexture(1280, 720);
                    break;
                case Resolutions.Vga:
                    WebCam = new WebCamTexture(640, 480);
                    break;
            }

            DebugDevices();

            WebCam.deviceName = WebCamTexture.devices[0].name;
        }

        public void DebugDevices()
        {
            Debug.Log("Cameras conectadas " + WebCamTexture.devices.Length);
            for (int i = 0, n = WebCamTexture.devices.Length; i < n; i++)
            {
                Debug.Log("Camera " + i.ToString("d2") + ": " + WebCamTexture.devices[i].name);
            }
        }

        public void StopWebCam()
        {
            WebCam.Stop();
        }
        public void StartWebCam()
        {
            if (WebCamTexture.devices.Length > 0)
            {
                WebCam.Play();
                if (OnWebCanStarted != null) OnWebCanStarted();
            }
        }

        public void CountStopWebCam()
        {
            _bootstrap.StartGame(this);
            StartCoroutine(CountStopRoutine());
        }

        private IEnumerator CountStopRoutine()
        {
            yield return new WaitForSeconds(CounterTime);
            StopWebCam();
        }

        public void TakePicture(bool count)
        {
            if (count)
            {
                _bootstrap.StartGame(this);
                if (OnStartTakePicture != null) OnStartTakePicture();
            }
            StartCoroutine(TakePictureRoutine(count));
        }

        IEnumerator TakePictureRoutine(bool count)
        {
            if(count)
                yield return new WaitForSeconds(CounterTime);

            yield return 0;

            if (_sfx != null) 
            _sfx.PlaySound("Snapshot");

            Texture2D picture = new Texture2D(WebCam.width, WebCam.height);
            if (TakeFromWebCam)
            {
                picture.SetPixels(WebCam.GetPixels(0, 0, (int) WebCam.width, (int) WebCam.height));
                picture.Apply();
            }

            StopWebCam();

            if (string.IsNullOrEmpty(RegisterName))
            {
                if (TakeFromWebCam)
                    _io.Save(picture);
                
                if (TakeFromScreen)
                    _io.SaveFromScreen();
            }
            else
            {
                var reg = _bootstrap.GetModel(ModelTypes.Register) as RegistrationData;

                if (TakeFromWebCam)
                    _io.Save(picture, reg.GetActualRegistry(RegisterName));
                if (TakeFromScreen)
                    _io.SaveFromScreen(reg.GetActualRegistry(RegisterName));
            }

            StartWebCam();
        }
    }
}