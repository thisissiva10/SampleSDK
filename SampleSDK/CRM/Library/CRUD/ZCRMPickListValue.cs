using System;
namespace SampleSDK.CRM.Library.CRUD
{
    public class ZCRMPickListValue
    {
        private string displayName;
        private string actualName;
        private int sequenceNumber;

        //TODO: Learn about JSON Array and declare a field of name maps and implement properties;

        private ZCRMPickListValue() { }

        public string DisplayName { get => displayName; set => displayName = value; }

        public string ActualName { get => actualName; set => actualName = value; }

        public int SequenceNumber { get => sequenceNumber; set => sequenceNumber = value; }
    }
}
