using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using InterativaSystem.Models;
using InterativaSystem.Services;
using Newtonsoft.Json;
using UnityEngine;

namespace InterativaSystem.Controllers
{
    [AddComponentMenu("ModularSystem/Controllers/IO Controller")]
    public class IOController : GenericController
    {
        public bool UseMultipleAppIO;
        public bool LoadFromPlayrPrefs;

        [Header("Settings (No End Slash)")]
        public string DataFolder;

        [Space]
        public bool UseCustomPath;
        public string ImageLoadPath;

        public delegate void ListTexture2DEvent(List<Texture2D> values);

        public event SimpleEvent Saved;
        public event StringEvent Loaded;
        public event ListTexture2DEvent LoadedAll;

        private RegistrationData _registrationData;

        private int _progressionCounter;

#if HAS_SERVER
        public bool SentPicturesToServer;
        public bool SaveOnMobile;
#endif

#if HAS_SQLITE3
        [HideInInspector]
        public SQLite3Service SqLite3Service;
#endif

        void Awake()
        {
            //Mandatory set for every controller
            Type = ControllerTypes.IO;

            _progressionCounter = 0;
            if (PlayerPrefs.HasKey("io_progression"))
                _progressionCounter = PlayerPrefs.GetInt("io_progression");

            PlayerPrefs.SetInt("io_progression", _progressionCounter);

            if (LoadFromPlayrPrefs && PlayerPrefs.GetString("IOFolder") != null)
                DataFolder = PlayerPrefs.GetString("IOFolder");
            else
                PlayerPrefs.SetString("IOFolder", DataFolder);

            if (PlayerPrefs.GetString("IOimageFolder") != null)
                ImageLoadPath = string.IsNullOrEmpty(PlayerPrefs.GetString("IOimageFolder")) ? ImageLoadPath : PlayerPrefs.GetString("IOimageFolder");
            else
                PlayerPrefs.SetString("IOimageFolder", ImageLoadPath);

            if (!UseCustomPath)
                ImageLoadPath = null;
        }

        protected override void OnStart()
        {
            SetUpFolder();

            _registrationData = _bootstrap.GetModel(ModelTypes.Register) as RegistrationData;

#if HAS_SQLITE3
            SqLite3Service = _bootstrap.GetService(ServicesTypes.SQLite3) as SQLite3Service;;
#endif
        }

        public void SetUpFolder()
        {
            if(LoadFromPlayrPrefs)
                PlayerPrefs.SetString("IOFolder", DataFolder);

            if (string.IsNullOrEmpty(DataFolder)) return;

            //Check if is IOS or Android and change directory path
            if (_bootstrap.IsMobile)
                DataFolder = Application.persistentDataPath + "/" + DataFolder;
            //else if(useAbsolutePath)

            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
                File.SetAttributes(DataFolder, FileAttributes.Normal);
                DebugLog("Directory created on: " + DataFolder);
            }
            if (!Directory.Exists(DataFolder + "/pictures"))
            {
                Directory.CreateDirectory(DataFolder + "/pictures");
                File.SetAttributes(DataFolder + "/pictures", FileAttributes.Normal);
                DebugLog("Directory created on: " + DataFolder + "/pictures");
            }
        }

        public void Save(DataModel model)
        {
#if HAS_SERVER
            if (_bootstrap.IsMobile && SaveOnMobile) return;
#endif

            if (UseMultipleAppIO)
            {
                try
                {
                    if (!model.OverrideDB)
                    {
                        if (File.Exists(DataFolder + "/" + model.DataFile))
                        {
                            var strfdl = File.ReadAllText(DataFolder + "/" + model.DataFile);
                            model.UpdateDataBase(strfdl);
                        }
                    }

                    var str = model.SerializeDataBase();

                    var dir = Path.GetDirectoryName(DataFolder + "/" + model.DataFile);
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    File.WriteAllText(DataFolder + "/" + model.DataFile, str);

                    DebugLog("Saved on: " + DataFolder + "/" + model.DataFile);
                    if (Saved != null) Saved();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                var str = model.SerializeDataBase();

                var dir = Path.GetDirectoryName(DataFolder + "/" + model.DataFile);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(DataFolder + "/" + model.DataFile, str);

                DebugLog("Saved on: " + DataFolder + "/" + model.DataFile);
                if (Saved != null) Saved();
            }
#if HAS_SQLITE3
            switch (model.Type)
            {
                case ModelTypes.Resources:
                    break;
                case ModelTypes.Tracks:
                    break;
                case ModelTypes.Register:
                    break;
                case ModelTypes.Score:
                    break;
                case ModelTypes.Scoreboard:
                    break;
                case ModelTypes.GameSettings:
                    break;
                case ModelTypes.Sound:
                    break;
                case ModelTypes.Questions:
                    SqLite3Service.CreateQuestionsTable();
                    var db = JsonConvert.DeserializeObject<List<Question>>(str);
                    for (int i = 0; i < db.Count; i++)
                    {
                        SqLite3Service.AddQuestionField(db[i]);
                    }
                    break;
                case ModelTypes.TurningVote:
                    break;
                case ModelTypes.Group:
                    SqLite3Service.CreateTable<Group>();
                    var groupdb = JsonConvert.DeserializeObject<List<Group>>(str);
                    for (int i = 0; i < groupdb.Count; i++)
                    {
                        SqLite3Service.AddGroupField(groupdb[i]);
                    }
                    break;
            }
#endif
        }
        public void Save(string json, string flie)
        {
#if HAS_SERVER
            if (_bootstrap.IsMobile && SaveOnMobile) return;
#endif
            var dir = Path.GetDirectoryName(DataFolder + "/" + flie);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            DebugLog("Saving on: " + DataFolder + "/" + flie);
            File.WriteAllText(DataFolder + "/" + flie, json);
            DebugLog("Saved on: " + DataFolder + "/" + flie);
            if (Saved != null) Saved();
        }
        public void Save(Texture2D image)
        {
#if HAS_SERVER
            if (_bootstrap.IsMobile && SaveOnMobile) return;
#endif

            byte[] file = image.EncodeToJPG();
            DebugLog("Saving on: " + DataFolder + "/pictures/" + _registrationData.GetActualRegistry("UniqueId") + ".jpg");
            File.WriteAllBytes(DataFolder + "/pictures/" + _registrationData.GetActualRegistry("UniqueId") + ".jpg", file);
            if (Saved != null) Saved();
            _progressionCounter++;
            PlayerPrefs.SetInt("io_progression", _progressionCounter);

#if HAS_SERVER
            if(SentPicturesToServer)
            {
                SendImageToServer(file);
            }
#endif
        }
        public void Save(Texture2D image, string filename)
        {
#if HAS_SERVER
            if (_bootstrap.IsMobile && SaveOnMobile) return;
#endif
            byte[] file = image.EncodeToJPG();
            DebugLog("Saving on: " + DataFolder + "/pictures/" + filename + "_" + _registrationData.GetActualRegistry("UniqueId") + ".jpg");
            File.WriteAllBytes(DataFolder + "/pictures/" + filename + "_" + _registrationData.GetActualRegistry("UniqueId") + ".jpg", file);
            if (Saved != null) Saved();

#if HAS_SERVER
            if(SentPicturesToServer)
            {
                SendImageToServer(file);
            }
#endif
        }

        public RenderTexture[] RenderTextures;
        public void SaveFromScreen()
        {
#if HAS_SERVER
            if (_bootstrap.IsMobile && SaveOnMobile) return;
#endif
            for (int i = 0; i < RenderTextures.Length; i++)
            {
                RenderTexture.active = RenderTextures[i];
                Texture2D image = new Texture2D(RenderTextures[i].width, RenderTextures[i].height, TextureFormat.RGB24, false);
                image.ReadPixels(new Rect(0, 0, RenderTextures[i].width, RenderTextures[i].height), 0, 0);
                RenderTexture.active = null; //can help avoid errors

                if (!Directory.Exists(DataFolder + "/pictures/" + "RT_" + i.ToString("00") + "/"))
                    Directory.CreateDirectory(DataFolder + "/pictures/" + "RT_" + i.ToString("00") + "/");

                byte[] file = image.EncodeToJPG();
                DebugLog("Saving on: " + DataFolder + "/pictures/" + _registrationData.GetActualRegistry("UniqueId") + "_" + i.ToString("000") + ".jpg");
                File.WriteAllBytes(DataFolder + "/pictures/" + "RT_" + i.ToString("00") + "/" + _registrationData.GetActualRegistry("UniqueId") + "_" + i.ToString("000") + ".jpg", file);
                if (Saved != null) Saved();
                _progressionCounter++;
                PlayerPrefs.SetInt("io_progression", _progressionCounter);

#if HAS_SERVER
                if(SentPicturesToServer)
                {
                    SendImageToServer(file);
                }
#endif
            }
        }
        public void SaveFromScreen(string filename)
        {
#if HAS_SERVER
            if (_bootstrap.IsMobile && SaveOnMobile) return;
#endif
            for (int i = 0; i < RenderTextures.Length; i++)
            {
                RenderTexture.active = RenderTextures[i];
                Texture2D image = new Texture2D(RenderTextures[i].width, RenderTextures[i].height, TextureFormat.RGB24, false);
                image.ReadPixels(new Rect(0, 0, RenderTextures[i].width, RenderTextures[i].height), 0, 0);
                RenderTexture.active = null; //can help avoid errors

                if (!Directory.Exists(DataFolder + "/pictures/" + "RT_" + i.ToString("00") + "/"))
                    Directory.CreateDirectory(DataFolder + "/pictures/" + "RT_" + i.ToString("00") + "/");

                byte[] file = image.EncodeToJPG();
                DebugLog("Saving on: " + DataFolder + "/pictures/" + filename + "_" + _registrationData.GetActualRegistry("UniqueId") + "_" + i.ToString("000") + ".jpg");
                File.WriteAllBytes(DataFolder + "/pictures/" + "RT_" + i.ToString("00") + "/" + filename + "_" + _registrationData.GetActualRegistry("UniqueId") + "_" + i.ToString("000") + ".jpg", file);
                if (Saved != null) Saved();
                _progressionCounter++;
                PlayerPrefs.SetInt("io_progression", _progressionCounter);

#if HAS_SERVER
                if(SentPicturesToServer)
                {
                    SendImageToServer(file);
                }
#endif
            }
        }

        public bool TryLoad(DataModel model)
        {
#if HAS_SQLITE3
            DebugLog("Trying to Load from a SQLite3 DB");
            switch (model.Type)
            {
                case ModelTypes.Resources:
                    break;
                case ModelTypes.Tracks:
                    break;
                case ModelTypes.Register:
                    break;
                case ModelTypes.Score:
                    break;
                case ModelTypes.Scoreboard:
                    break;
                case ModelTypes.GameSettings:
                    break;
                case ModelTypes.Sound:
                    break;
                case ModelTypes.Questions:
                    List<Question> data;
                    if (SqLite3Service.GetQuestions(out data))
                    {
                        var str = JsonConvert.SerializeObject(data);
                        model.DeserializeDataBase(str);
                        return true;
                    }
                    return false;
                case ModelTypes.TurningVote:
                    break;
                case ModelTypes.Group:
                    List<Group> dataGroup;
                    if (SqLite3Service.GetGroups(out dataGroup))
                    {
                        var str = JsonConvert.SerializeObject(dataGroup);
                        model.DeserializeDataBase(str);
                        return true;
                    }
                    break;
            }
#endif
            if (UseMultipleAppIO)
            {
                try
                {
                    DebugLog("Trying to Load");

                    if (File.Exists(DataFolder + "/" + model.DataFile))
                    {
                        var str = File.ReadAllText(DataFolder + "/" + model.DataFile);

                        model.DeserializeDataBase(str);

                        if (Loaded != null) Loaded(str);
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    
                    throw;
                }
            }
            else
            {
                DebugLog("Trying to Load");

                if (File.Exists(DataFolder + "/" + model.DataFile))
                {
                    var str = File.ReadAllText(DataFolder + "/" + model.DataFile);
                    
                    model.DeserializeDataBase(str);

                    if (Loaded != null) Loaded(str);
                    return true;
                }
                return false;
            }
        }
        public bool TryLoad(string pictureName, out Texture2D picture)
        {
            var pic = new Texture2D(0,0);
            DebugLog("Trying to Load an image");
            
            var path = string.IsNullOrEmpty(ImageLoadPath) ? DataFolder + "/pictures/" : ImageLoadPath;

            if (string.IsNullOrEmpty(ImageLoadPath))
                PlayerPrefs.SetString("IOimageFolder", ImageLoadPath);

            if (File.Exists(path + pictureName))
            {
                var data = File.ReadAllBytes(path + pictureName);

                pic.LoadImage(data);
                picture = pic;

                if (Loaded != null) Loaded(pictureName);
                return true;
            }

            picture = pic;
            return false;
        }

        private List<Texture2D> lastPictures;
        private List<string> picturesChosen;

        IEnumerator _TryLoad(bool onlyNew, ListTexture2DEvent callback)
        {
            if (picturesChosen == null)
                picturesChosen = new List<string>();

            var customPath = !string.IsNullOrEmpty(ImageLoadPath);
            var path = string.IsNullOrEmpty(ImageLoadPath) ? DataFolder + "/pictures/" : ImageLoadPath;
            
            if(string.IsNullOrEmpty(ImageLoadPath))
                PlayerPrefs.SetString("IOimageFolder", ImageLoadPath);

            if (!onlyNew && lastPictures != null)
            {
                for (int i = 0; i < lastPictures.Count; i++)
                    Destroy(lastPictures[i]);
            }
            else if (lastPictures == null)
            {
                lastPictures = new List<Texture2D>();
            }

            var pictures = new List<Texture2D>();

            var pic = new Texture2D(0, 0);

            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);

                for (int i = 0; i < files.Length; i++)
                {
                    if (onlyNew && !picturesChosen.Contains(files[i]) || !onlyNew)
                    {
                        if (onlyNew)
                            picturesChosen.Add(files[i]);

                        var data = File.ReadAllBytes(files[i]);
                        yield return null;
                        pic = new Texture2D(0, 0);
                        pic.LoadImage(data, true);
                        pictures.Add(pic);
                    }

                    yield return null;
                }

                if (onlyNew)
                    lastPictures.AddRange(pictures);
                else
                    lastPictures = pictures;

                if(pictures.Count > 0)
                    CallAction(5);

                callback(pictures);
            }
        } 
        public void TryLoad(bool onlyNew, ListTexture2DEvent callback)
        {
            StartCoroutine(_TryLoad(onlyNew, callback));
        }
        
#if HAS_SERVER
        public void SendImageToServer(byte[] image)
        {
            if (_isServer) return;

            //Send all in one json
            _clientController.SendMessageToServer("NetworkSave", Convert.ToBase64String(image));
        }
        public void NetworkSave(string base64)
        {
            if (!_isServer) return;

            var file = Convert.FromBase64String(base64);

            DebugLog("Saving on: " + DataFolder + "/pictures/" + "image_" + _progressionCounter.ToString("000") + ".png");

            File.WriteAllBytes(DataFolder + "/pictures/" + "image_" + _progressionCounter.ToString("000") + ".png", file);
            if (Saved != null) Saved();

            _progressionCounter++;
            PlayerPrefs.SetInt("io_progression", _progressionCounter);
        }
#endif
    }
}