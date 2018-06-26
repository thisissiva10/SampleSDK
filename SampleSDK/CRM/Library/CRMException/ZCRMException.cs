using System;
using Newtonsoft.Json.Linq;


namespace SampleSDK.CRM.Library.CRMException
{
    public class ZCRMException : Exception
    {

        private string code;
        private string errorMsg;

        //TODO: Declared type Exception because throwable is not available in c#;
        private Exception originalException = null;
        private JObject errorDetails;

        public string Code { get => code; private set => code = value; }
        public string ErrorMsg { 
            get {
                if (originalException != null)
                    return originalException.ToString();
                return errorMsg;
            }
            private set => errorMsg = value; 
        }
        public JObject ErrorDetails { get => errorDetails; private set => errorDetails = value; }

        public ZCRMException(string code, string message, JObject errorDetails) : base(message)
        {
            Code = code;
            ErrorMsg = message;
            ErrorDetails = errorDetails;
        }

        public ZCRMException(string code, string message) : this(code: code, message: message, errorDetails: null) { }

        public ZCRMException(string message) : this(code: null, message: message, errorDetails: null) { }

        //TODO: Inspect the necessity of this class and do the appropriate action;
        //public ZCRMException(string code, Exception ex) : base(ex) { }
        //public ZCRMException(Exception ex) : this(null, ex) { }




        public override string ToString()
        {
            string returnMsg = typeof(ZCRMException).FullName;
            if(Code != null)
            {
                returnMsg += $"{Code} - {ErrorMsg}";
            }
            else{
                returnMsg += ErrorMsg;
            }
            return returnMsg;
        }
    }
}
