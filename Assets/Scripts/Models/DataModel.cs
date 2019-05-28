using System.Collections.Generic;
using System.Net.NetworkInformation;
using InterativaSystem.Controllers;
using UnityEngine;

namespace InterativaSystem.Models
{
    /// <summary>
    /// Model usado para carregar e armazenar dados
    /// </summary>
    public class DataModel : GenericModel
    {
        /// <summary>
        /// referencia para o model de Ids necessario em quase todos os models de data
        /// </summary>
        protected IdsData _idsData;

        /// <summary>
        /// Node do arquivo que será salvo
        /// </summary>
        [Header("Settings (.json extension)")]
        public string DataFile;

        /// <summary>
        /// Evento disparado sempre que um novo dado é adicionado ao modelo
        /// </summary>
        public event GenericController.SimpleEvent OnNewValue;
        
        /// <summary>
        /// Definie se o arquivo será lido no inicio ou sera salvo usando o array inserido no inspector
        /// </summary>
        [Header("Warning: this will override the file", order = 2)]
        public bool OverrideDB;

        /// <summary>
        /// grava arquivo apenas quando o jogo acaba, deixando de gavar todas as vezes que é atualizado
        /// </summary>
        [Header("For Dynamic Models")]
        public bool SaveOnGameEnd;
        public bool SaveOnReset = true;

#if HAS_SERVER
        /// <summary>
        /// booleana usada para definir se o model vai buscar os dados do servidor ao conectar-se ou osar os dados locais
        /// </summary>
        [Header("For Server uses")]
        public bool GetDataFromServerOnConnect;
#endif

        /// <summary>
        /// Reseta os dados e carrega ou salva arquivo caso nao existam dados.
        /// Dispara o EndInitialization
        /// </summary>
        protected override void OnStart()
        {
            base.OnStart();

            if (Type != ModelTypes.Ids)
            {
                _idsData = _bootstrap.GetModel(ModelTypes.Ids) as IdsData;

                if(_idsData == null)
                    Debug.LogError("Sem Model de Id, favor adiciona-lo e reiniciar a aplicação");
            }
            
            ResetData();

#if HAS_SERVER
            if (!GetDataFromServerOnConnect && !OverrideDB && !_IOController.TryLoad(this))
#else
            if (!OverrideDB && !_IOController.TryLoad(this))
#endif
            {
                _IOController.Save(this);
            }
            else if (OverrideDB)
            {
                _IOController.Save(this);
            }

            if (SaveOnGameEnd)
                _bootstrap.GameControllerEnded += Save;

            OnNewValue += Save;

            GetReferences();
            EndInitialization();
        }

        public override void CallReset()
        {
            base.CallReset();

            if (SaveOnReset)
                Save();
        }

        #region GetSetters
        /// <summary>
        /// Reseta dados e contador
        /// </summary>
        protected virtual void ResetData() { }
        /// <summary>
        /// Acrescenta um novo dado ao Model
        /// </summary>
        public virtual void AddNewValue() { if (OnNewValue != null) OnNewValue(); }
        /// <summary>
        /// Acrescenta um novo dado ao Model
        /// </summary>
        ///<param name="parameters">parametros passado em um objeto, para genericamente adicionar dados</param>
        public virtual void AddNewValue(object[] parameters) { if (OnNewValue != null) OnNewValue(); }

        /// <summary>
        /// Chama o evento de OnNewValue
        /// </summary>
        protected void OnNewValueEvent() { if (OnNewValue != null) OnNewValue(); }

        #endregion

        #region IO
        /// <summary>
        /// Grava os dados
        /// </summary>
        public virtual void Save()
        {
            if (string.IsNullOrEmpty(DataFile))
                return;

            _IOController.Save(this);
        }
        /// <summary>
        /// Carrega os dados
        /// </summary>
        public virtual void Load()
        {
            if (string.IsNullOrEmpty(DataFile))
                return;

            if (_IOController.TryLoad(this))
            {
                OnLoadSuccess();
            }
        }

        protected virtual void OnLoadSuccess() { }
        #endregion

        #region Serialization
        /// <summary>
        /// Usado para serializar os dados do Model
        /// </summary>
        /// <returns> retorna o valor em texto para ser salvo</returns>
        public virtual string SerializeDataBase() { return ""; }
        /// <summary>
        /// Usado para deserializar os dados de um arquivo, quardando o resultado dentro do proprio Model
        /// </summary>
        /// <param name="json"> valor em formato de texto para ser deserializado</param>
        public virtual void DeserializeDataBase(string json) { }

        /// <summary>
        /// Usado para mesclar os dados do arquivo com os dados do Model
        /// </summary>
        /// <param name="json"> valor em formato de texto para ser deserializado </param>
        public virtual void UpdateDataBase(string json) { }


        public virtual string SerializeDataBaseToCSV() { return ""; }
        #endregion

    }
}