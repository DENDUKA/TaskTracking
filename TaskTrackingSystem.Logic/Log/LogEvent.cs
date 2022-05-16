using NLog;

namespace TaskTrackingSystem.Logic.Log
{
    public class LogEvent
    {
        #region singleton
        public static LogEvent Instance { get; private set; }

        private LogEvent(){}    

        static LogEvent()
        {
            Instance = new LogEvent();            
        }
        #endregion


        public Logger Log = DAL.DAL.Log.LogEvents.Log;        
    }
}
