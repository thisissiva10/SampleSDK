using Newtonsoft.Json.Linq;
namespace SampleSDK.CRM.Library.CRUD
{
    public class ZCRMPickListValue
    {
        private string displayName;
        private string actualName;
        private int sequenceNumber;

        //TODO: Learn about JSON Array and declare a field of name maps and implement properties;

        private JArray maps;

        private ZCRMPickListValue() { }

        public static ZCRMPickListValue GetInstance()
        {
            return new ZCRMPickListValue();
        }

        public string DisplayName { get => displayName; set => displayName = value; }

        public string ActualName { get => actualName; set => actualName = value; }

        public int SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }

        public JArray Maps { get => maps; set => maps = value; }
    }
}
