using System;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.Setup.Users;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.CRMException;
using SampleSDK.CRM.Library.Api.Handler;

namespace SampleSDK.CRM.Library.CRUD
{

    //TODO:<IMPORTANT> Learn about ICloneable;
    public class ZCRMRecord : ZCRMEntity, ICloneable
    {

        private long? entityId = null;
        private string moduleAPIName = null;
        private Dictionary<string, object> fieldNameVsValue = new Dictionary<string, object>();
        private Dictionary<string, object> properties = new Dictionary<string, object>();
        private List<ZCRMInventoryLineItem> lineItems = new List<ZCRMInventoryLineItem>();
        private List<ZCRMEventParticipant> participants = new List<ZCRMEventParticipant>();
        private List<ZCRMPriceBookPricing> priceDetails = new List<ZCRMPriceBookPricing>();
        private string lookupLabel = null;
        private ZCRMUser owner;
        private ZCRMUser createdBy;
        private ZCRMUser modifiedBy;
        private string createdTime;
        private string modifiedTime;
        private ZCRMLayout layout;
        private List<ZCRMTax> taxList = new List<ZCRMTax>();


        private ZCRMRecord(string module, long? id)
        {
            ModuleAPIName = module;
            EntityId = id;
        }

        public ZCRMRecord(string module) : this(module, null) { }


        public static ZCRMRecord GetInstance(string moduleAPIName, long entityId)
        {
            return new ZCRMRecord(moduleAPIName, entityId);
        }

        public string ModuleAPIName { get => moduleAPIName; private set => moduleAPIName = value; }

        public long? EntityId { get => entityId; set => entityId = value; }

        public string LookupLabel { get => lookupLabel; set => lookupLabel = value; }

        public ZCRMLayout Layout { get => layout; set => layout = value; }

        public ZCRMUser Owner { get => owner; set => owner = value; }

        public ZCRMUser CreatedBy { get => createdBy; set => createdBy = value; }

        public string CreatedTime { get => createdTime; set => createdTime = value; }

        public ZCRMUser ModifiedBy { get => modifiedBy; set => modifiedBy = value; }

        public string ModifiedTime { get => modifiedTime; set => modifiedTime = value; }

        public Dictionary<string, object> Properties {  get => properties;  private set => properties = value; }

        //TODO: Note the property name is changed<Inspect the usage;
        public Dictionary<string, object> Data { get => fieldNameVsValue; set => fieldNameVsValue = value; }

        public List<ZCRMInventoryLineItem> LineItems { get => lineItems; private set => lineItems = value; }

        public List<ZCRMEventParticipant> Participants { get => participants; private set => participants = value; }

        public List<ZCRMPriceBookPricing> PriceDetails { get => priceDetails; private set => priceDetails = value; }

        public List<ZCRMTax> TaxList { get => taxList; private set => taxList = value; }

        public void SetProperty(string name, object value)
        {
            Properties.Add(name,value);
        }

        public object GetProperty(string propertyName)
        {
            return Properties[propertyName];
        }

        public void SetFieldValue(string fieldAPIName, object value)
        {
            Data.Add(fieldAPIName, value);
        }

        public object GetFieldValue(string fieldAPIName)
        {
            if(Data.ContainsKey(fieldAPIName))
            {
                if(Data[fieldAPIName] == null)
                {
                    return null;
                }

                return Data[fieldAPIName];
            }
            //TODO: Convert the Exception to ZCRMException;
            throw new Exception($"The given field is not present in this record - {fieldAPIName}");
        }


        public void AddLineItem(ZCRMInventoryLineItem newLineItem)
        {
            LineItems.Add(newLineItem);
        }

        public void AddParticipant(ZCRMEventParticipant participant)
        {
            Participants.Add(participant);
        }

        public void AddPriceDetail(ZCRMPriceBookPricing priceDetail)
        {
            PriceDetails.Add(priceDetail);
        }

        public void AddTax(ZCRMTax tax)
        {
            TaxList.Add(tax);
        }

        //TODO: Complete the remaining methods;


        public APIResponse Create()
        {
            if(EntityId != null)
            {
                throw new ZCRMException("Entity ID MUST be NUL for Create Operation");
            }
            return EntityAPIHandler.GetInstance(this).CreateRecord();
        }

        public APIResponse Update()
        {
            if(EntityId == null)
            {
                throw new ZCRMException("Entity ID MUST NOT be NULl for update operation");
            }
            return EntityAPIHandler.GetInstance(this).UpdateRecord();
        }

        public APIResponse Delete()
        {
            if(EntityId == null)
            {
                throw new ZCRMException("Entity ID MUST NOT be NULL for Delete Operation");
            }
            return EntityAPIHandler.GetInstance(this).DeleteRecord();
        }

        public Dictionary<string, long> Convert()
        {
            return Convert(null);
        }

        private Dictionary<string, long> Convert(ZCRMRecord potential)
        {
            return Convert(potential, null);
        }

        private Dictionary<string, long> Convert(ZCRMRecord potential, ZCRMUser assignToUser)
        {
            return EntityAPIHandler.GetInstance(this).ConvertRecord(potential, assignToUser);
        }

        //TODO<IMPORTANT>:Learn about the method completely before implementing the method;
        public object Clone()
        {
            
            throw new NotImplementedException();
        }
    }
}
