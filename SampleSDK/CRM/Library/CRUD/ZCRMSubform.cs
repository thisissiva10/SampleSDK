using System;


namespace SampleSDK.CRM.Library.CRUD
{
    public class ZCRMSubform : ZCRMRecord
    {

        private long? id = null;
        private string name = null;

        private ZCRMSubform(string name, long? id) : base(name)
        {
            Id = id;
            Name = name;
        }

        public ZCRMSubform(string name) : this(name, null) { }


        private long? Id { get => id; set => id = value; }
        private string Name { get => name; set => name = value; }


    }
}
