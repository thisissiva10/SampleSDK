using System.Diagnostics;


namespace SampleSDK.CRM.Library.CRMException
{
    public class ZCRMLogger
    {
        private static TraceSwitch logSwitch;

        public static void Init()
        {
            logSwitch = new TraceSwitch("ZCRMLogger", "SampleSDK Logger")
            {
                Level = TraceLevel.Verbose
            };

        }

        public static void LogInfo(string message)
        {
            Trace.WriteLineIf(logSwitch.TraceInfo, message, "INFO");
        }

        public static void LogWarning(string message)
        {
            Trace.WriteLineIf(logSwitch.TraceWarning, message, "WARNING");
        }


        public static void LogError(string message)
        {
            Trace.WriteLineIf(logSwitch.TraceError, message, "ERROR");
        }


    }
}
