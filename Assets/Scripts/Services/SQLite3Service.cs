using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InterativaSystem.Controllers;
using InterativaSystem.Models;
using Newtonsoft.Json;
using SQLite4Unity3d;
using UnityEngine;

namespace InterativaSystem.Services
{
    public class SQLite3Service : GenericService
    {
        private SQLiteConnection _connection;
        private IOController _ioController;

        public string DBName;

#if HAS_SQLITE3
        void Awake()
        {
            //Mandatory set for every service
            Type = ServicesTypes.SQLite3;
        }

        protected override void OnStart()
        {
            base.OnStart();

            _ioController = _bootstrap.GetController(ControllerTypes.IO) as IOController;

            ConnectToDB(DBName);
        }

        public void ConnectToDB(string DatabaseName)
        {
            DebugLog("Connecting");
            DatabaseName += ".db";

            var path = _ioController.DataFolder;
            /*
#if UNITY_EDITOR
            var dbPath = path + "/" + DatabaseName;//string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", _ioController.DataFolder, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID 
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
            /**/
            var dbPath = path + "/" + DatabaseName;
            _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
            Debug.Log("SqlService Log: Final path: " + dbPath);
        }

        public void CreateDB(string dbName)
        {
            ConnectToDB(dbName+".db");
        }
        public void CreateTable<T>()
        {
            //_connection.DropTable<Question>();
            _connection.CreateTable<T>();
        }

        #region Resources Table Methods
        #endregion

        #region Tracks Table Methods
        #endregion

        #region Register Table Methods
        #endregion

        #region Score Table Methods
        #endregion

        #region Scoreboard Table Methods
        #endregion

        #region GameSettings Table Methods
        #endregion

        #region Sound Table Methods
        #endregion

        #region Questions Table Methods
        public void CreateQuestionsTable()
        {
            //_connection.DropTable<Question>();
            _connection.CreateTable<Question>();
            _connection.CreateTable<Question.Alternatives>();
        }
        public void AddQuestionField(Question data)
        {
            _connection.InsertOrReplace(data);
            _connection.InsertOrReplace(data.alternatives);
        }
        public IEnumerable<Question> GetQuestionsEnumerable()
        {
            return _connection.Table<Question>();
        }
        public bool GetQuestions(out List<Question> data)
        {
            List<Question> list;
            List<Question.Alternatives> alts;

            try
            {
                list = GetQuestionsEnumerable().ToList();
                alts = _connection.Table<Question.Alternatives>().ToList();
            }
            catch (SQLiteException e)
            {
                DebugLog(e.ToString());
                data = null;
                return false;
            }
            
            for (int i = 0; i < list.Count; i++)
            {
                if(alts.Exists(x => x.referenceIndex == list[i].alternativesIndex))
                    list[i].alternatives = alts.FindAll(x => x.referenceIndex == list[i].alternativesIndex);
            }

            data = list;
            return true;
        }
        #endregion

        #region Group Table Methods
        public void AddGroupField(Group data)
        {
            _connection.InsertOrReplace(data);
        }
        public bool GetGroups(out List<Group> data)
        {
            List<Group> list;

            try
            {
                list = _connection.Table<Group>().ToList();
            }
            catch (SQLiteException e)
            {
                DebugLog(e.ToString());
                data = null;
                return false;
            }

            for (int i = 0; i < list.Count; i++)
            {
                var color = new Color(list[i].ColorR, list[i].ColorG, list[i].ColorB, list[i].ColorA);
                list[i].Color = color;
            }

            data = list;
            return true;
        }
        #endregion

        #region Turning Table Methods
        public void DropTurningGroupResponse()
        {
            _connection.DropTable<TurningGroupResponse>();
            _connection.CreateTable<TurningGroupResponse>();
        }
        public void DropTurningPadResponse()
        {
            _connection.DropTable<TurningPadResponse>();
            _connection.CreateTable<TurningPadResponse>();
        }

        public void AddTurningField(TurningPad data)
        {
            _connection.InsertOrReplace(data);
        }
        public void AddPadsGroupsField(TurningGroup data)
        {
            _connection.InsertOrReplace(data);
        }
        public void AddTurningGroupResponseField(TurningGroupResponse data)
        {
            _connection.InsertOrReplace(data);
        }
        public void AddTurningPadResponseField(TurningPadResponse data)
        {
            _connection.InsertOrReplace(data);
        }
        public void CreateTurningTable()
        {
            _connection.DropTable<TurningPad>();
            _connection.CreateTable<TurningPad>();

            _connection.InsertAll(new[]
            {
                new TurningPad
                {
                    PadId = "test",
                    Vote = "9",
                    Time = 0.55
                },
                new TurningPad
                {
                    PadId = "testB",
                    Vote = "4",
                    Time = 0.22
                }
            });
        }
        public bool GetPadsGroups(out List<TurningGroup> data)
        {
            List<TurningGroup> list;

            try
            {
                list = _connection.Table<TurningGroup>().ToList();
            }
            catch (SQLiteException e)
            {
                DebugLog(e.ToString());
                data = null;
                return false;
            }

            data = list;
            return true;
        }
        public bool GetTurningGroupResponses(out List<TurningGroupResponse> data)
        {
            List<TurningGroupResponse> list;

            try
            {
                list = _connection.Table<TurningGroupResponse>().ToList();
            }
            catch (SQLiteException e)
            {
                DebugLog(e.ToString());
                data = null;
                return false;
            }

            data = list;
            return true;
        }
        public bool GetTurningPadResponses(out List<TurningPadResponse> data)
        {
            List<TurningPadResponse> list;

            try
            {
                list = _connection.Table<TurningPadResponse>().ToList();
            }
            catch (SQLiteException e)
            {
                DebugLog(e.ToString());
                data = null;
                return false;
            }

            data = list;
            return true;
        }
        #endregion

        #region SunVote Table Methods
        public void CreateSunVoteTable()
        {
            _connection.DropTable<SunvotePad>();
            _connection.CreateTable<SunvotePad>();

            _connection.InsertAll(new[]
            {
                new SunvotePad
                {
                    PadId = "test",
                    Vote = "9",
                    Time = 0.55
                },
                new SunvotePad
                {
                    PadId = "testB",
                    Vote = "4",
                    Time = 0.22
                }
            });
        }
        #endregion


        public IEnumerable<Person> GetPersons()
        {
            return _connection.Table<Person>();
        }

        public IEnumerable<Person> GetPersonsNamedRoberto()
        {
            return _connection.Table<Person>().Where(x => x.Name == "Roberto");
        }

        public Person GetJohnny()
        {
            return _connection.Table<Person>().Where(x => x.Name == "Johnny").FirstOrDefault();
        }

        public Person CreatePerson()
        {
            var p = new Person
            {
                Name = "Johnny",
                Surname = "Mnemonic",
                Age = 21
            };
            _connection.Insert(p);
            return p;
        }
#endif
    }

#if HAS_SQLITE3
    public class GenericTable
    {

    }
    public class TurningPad : GenericTable
    {
        [PrimaryKey]
        public string PadId { get; set; }
        public string Vote { get; set; }
        public double Time { get; set; }
    }
    public class SunvotePad : GenericTable
    {
        [PrimaryKey]
        public string PadId { get; set; }
        public string Vote { get; set; }
        public double Time { get; set; }
    }
#else
    [System.Serializable]
    public class GenericTable
    {

    }
    [System.Serializable]
    public class TurningPad : GenericTable
    {
        public string PadId;
        public string Vote;
        public double Time;
    }
    [System.Serializable]
    public class SunvotePad : GenericTable
    {
        public string PadId;
        public string Vote;
        public double Time;
    }
#endif
}