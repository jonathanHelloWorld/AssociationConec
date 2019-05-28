using InterativaSystem.Services;
using Newtonsoft.Json;

namespace InterativaSystem.Views.DebugObject
{
    public class DebugSQLite : GenericView
    {
#if HAS_SQLITE3
        private SQLite3Service sqlite;
        private UDPReceive receiver;

        protected override void OnStart()
        {
            base.OnStart();

            sqlite = _bootstrap.GetService(ServicesTypes.SQLite3) as SQLite3Service;

            //sqlite.CreateDB("DebugDB");
            sqlite.CreateTurningTable();
            sqlite.CreateSunVoteTable();

            receiver = _bootstrap.GetService(ServicesTypes.UDPRead) as UDPReceive;
            receiver.OnPacketReceive += PrintDebug;
        }

        private void PrintDebug(string value)
        {
            sqlite.AddTurningField(JsonConvert.DeserializeObject<TurningPad>(value));
        }
#endif
    }
}