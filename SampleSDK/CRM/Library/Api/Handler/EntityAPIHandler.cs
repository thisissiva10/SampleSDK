using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.CRUD;
using SampleSDK.CRM.Library.Setup.Users;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.CRMException;


namespace SampleSDK.CRM.Library.Api.Handler
{
    public class EntityAPIHandler : CommonAPIHandler, IAPIHandler
    {
        //NOTE:Property not used;
        protected ZCRMRecord record = null;


        protected EntityAPIHandler(ZCRMRecord zcrmRecord)
        {
            record = zcrmRecord;
        }

        public static EntityAPIHandler GetInstance(ZCRMRecord zcrmRecord)
        {
            return new EntityAPIHandler(zcrmRecord);
        }

        //TODO: Handle Exceptions;
        public APIResponse GetRecord()
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = $"{record.ModuleAPIName}/{record.EntityId}";

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject recordDetails = (JObject)responseDataArray[0];
                SetRecordProperties(recordDetails);
                response.Data = record;
                Console.WriteLine("Response Data JSON Array:");
                Console.WriteLine(responseDataArray);
                return response;
            }catch(Exception e)
            {
                //TODO: Handle the response appropriately;
                throw new ZCRMException(e.ToString());
            }
        }

        //TODO: Needs to be logged and tested ad exceptions need to be handled;
        public APIResponse CreateRecord()
        {
            try{
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = record.ModuleAPIName;
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                dataArray.Add(GetZCRMRecordAsJSON());
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject responseData = (JObject)responseDataArray[0];
                JObject recordDetails = (JObject)responseData.GetValue("details");
                SetRecordProperties(recordDetails);
                response.Data = record;
                return response;
            }catch(Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("Exception at createRecord method!.. ");
                Console.WriteLine(e);
                throw new ZCRMException(e.ToString());
            }
        }

        public APIResponse UpdateRecord()
        {
            try{
                requestMethod = APIConstants.RequestMethod.PUT;
                urlPath = record.ModuleAPIName + "/" + record.EntityId;
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                dataArray.Add(GetZCRMRecordAsJSON());
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray responseDataArray = (JArray)response.ResponseJSON.GetValue("data");
                JObject responseData = (JObject)responseDataArray[0];
                JObject responseDetails = (JObject)responseData.GetValue("details");
                SetRecordProperties(responseDetails);
                response.Data = record;
                return response;

            }catch(Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("Exception at UpdateRecord method!.. ");
                Console.WriteLine(e);
                throw new ZCRMException(e.ToString());
            }
        }


        public APIResponse DeleteRecord()
        {
            try{
                requestMethod = APIConstants.RequestMethod.DELETE;
                urlPath = record.ModuleAPIName + "/" + record.EntityId;

                return APIRequest.GetInstance(this).GetAPIResponse();
            }catch(Exception e)
            {
                //TODO: Handle exceptions appropriately and log the info;
                Console.WriteLine("Exception caught at DeleteRecord");
                Console.WriteLine(e);
                throw new ZCRMException(e.ToString());
            }
        }


        //TODO: Handle exceptions appropriately;
        public Dictionary<string, long> ConvertRecord(ZCRMRecord potential, ZCRMUser assignToUser)
        {
            try{
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = record.ModuleAPIName + "/" + record.EntityId;
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                JObject dataObject = new JObject();
                if(assignToUser != null)
                {
                    dataObject.Add("assign_to", assignToUser.Id.ToString());
                }
                if(potential != null)
                {
                    dataObject.Add("Deals", GetInstance(potential).GetZCRMRecordAsJSON());
                }
                dataArray.Add(dataObject);
                requestBodyObject.Add("data", dataArray);
                requestBody = requestBodyObject;

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();


                JArray responseJson = (JArray)response.ResponseJSON.GetValue("data");
                JObject convertedIdsJSON = (JObject)responseJson[0];

                Dictionary<string, long> convertedIds = new Dictionary<string, long>();
                convertedIds.Add("Contacts", Convert.ToInt64(convertedIdsJSON.GetValue("Contacts")));
                if(convertedIdsJSON.GetValue("Accounts") != null)
                {
                    convertedIds.Add("Accounts", Convert.ToInt64(convertedIdsJSON.GetValue("Accounts")));
                }
                if (convertedIdsJSON.GetValue("Deals") != null)
                {
                    convertedIds.Add("Deals", Convert.ToInt64(convertedIdsJSON.GetValue("Deals")));
                }

                return convertedIds;
            }catch(Exception e){
                //TODO: Log the info;
                Console.WriteLine("Exception caught at ConvertRecord");
                throw new ZCRMException(e.ToString());
            }
        }

        //TODO: UploadPhoto(), DownloadPhoto() and DeletePhoto() methods to be written;

        public void SetRecordProperties(JObject recordDetails)
        {
            SetRecordProperties(recordDetails, record);
        }


        //TODO<IMPORTANT>: Lots of performance tune-ups are needed and needs plenty of testing!!;
        public void SetRecordProperties(JObject recordJSON, ZCRMRecord record)
        {
            //JObject recordDetails = new JObject(recordJSON);
            //TODO: If Performance is needed implement JSONTextReader or JSONReader and put thought into it later after completing the SDK;
            foreach (KeyValuePair<string, JToken> token in recordJSON)
            {
                string fieldAPIName = token.Key;
                if (fieldAPIName.Equals("id"))
                {
                    record.EntityId = Convert.ToInt64(token.Value);
                }
                else if (fieldAPIName.Equals("Product_Details"))
                {
                    SetInventoryLineItems((JArray)token.Value);
                }
                else if (fieldAPIName.Equals("Participants"))
                {
                    SetParticipants((JArray)token.Value);
                }
                else if (fieldAPIName.Equals("Pricing_Details"))
                {
                    SetPriceDetails((JArray)token.Value);
                }
                else if (fieldAPIName.Equals("Created_By"))
                {
                    JObject createdObject = (JObject)token.Value;
                    ZCRMUser createdUser = ZCRMUser.GetInstance(Convert.ToInt64(createdObject.GetValue("id")), Convert.ToString(createdObject.GetValue("name")));
                    record.CreatedBy = createdUser;
                }
                else if (fieldAPIName.Equals("Modified_By"))
                {
                    JObject modifiedObject = (JObject)token.Value;
                    ZCRMUser modifiedBy = ZCRMUser.GetInstance(Convert.ToInt64(modifiedObject.GetValue("id")), Convert.ToString(modifiedObject.GetValue("name")));
                }
                else if (fieldAPIName.Equals("Created_Time"))
                {
                    record.CreatedTime = Convert.ToString(token.Value);
                }
                else if(fieldAPIName.Equals("Modified_Time"))
                {
                    record.ModifiedTime = Convert.ToString(token.Value);
                }
                else if(fieldAPIName.Equals("Owner"))
                {
                    JObject ownerObject = (JObject)token.Value;
                    ZCRMUser ownerUser = ZCRMUser.GetInstance(Convert.ToInt64(ownerObject.GetValue("id")), Convert.ToString(ownerObject.GetValue("name")));
                    record.Owner = ownerUser;
                }
                else if(fieldAPIName.Equals("Layout") && token.Value.Type != JTokenType.Null)
                {
                    JObject layoutObject = (JObject)token.Value;
                    ZCRMLayout layout = ZCRMLayout.GetInstance(Convert.ToInt64(layoutObject.GetValue("id")));
                    layout.Name = Convert.ToString(layoutObject.GetValue("name"));
                 }
                else if(fieldAPIName.Equals("Handler") && token.Value.Type != JTokenType.Null)
                {
                    JObject handlerObject = (JObject)token.Value;
                    ZCRMUser handler = ZCRMUser.GetInstance(Convert.ToInt64(handlerObject.GetValue("id")), Convert.ToString(handlerObject.GetValue("name")));
                    record.SetFieldValue(fieldAPIName, handler);
                }
                else if(fieldAPIName.Equals("Remind_At") && token.Value.Type == JTokenType.Null)
                {
                    JObject remindObject = (JObject)token.Value;
                    record.SetFieldValue(fieldAPIName, remindObject.GetValue("ALARM"));
                }
                else if(fieldAPIName.Equals("Recurring_Activity") && token.Value.Type != JTokenType.Null)
                {
                    JObject recurringActivityObject = (JObject)token.Value;
                    record.SetFieldValue(fieldAPIName, recurringActivityObject.GetValue("RRULE"));
                }
                else if(fieldAPIName.Equals("$line_tax"))
                {
                    JArray taxDetails = (JArray)token.Value;
                    foreach(JObject taxDetail in taxDetails)
                    {
                        ZCRMTax tax = ZCRMTax.GetInstance(Convert.ToString(taxDetail.GetValue("name")));
                        tax.Percentage = Convert.ToDouble(taxDetail.GetValue("percentage"));
                        tax.Value = Convert.ToDouble(taxDetail.GetValue("value"));
                        record.AddTax(tax);
                    }
                }
                else if (fieldAPIName.Equals("Tax") && token.Value.Type != JTokenType.Null)
                {
                    JArray taxNames = (JArray)token.Value;
                    int arrayLen = taxNames.Count;
                    for (int i = 0; i < arrayLen; i++)
                    {
                        ZCRMTax tax = ZCRMTax.GetInstance(Convert.ToString(taxNames[i]));
                        record.AddTax(tax);
                    }
                }
                else if(fieldAPIName.StartsWith("$", StringComparison.CurrentCulture))
                {
                    fieldAPIName = fieldAPIName.TrimStart('\\','$');
                    if(APIConstants.PROPERTIES_AS_FILEDS.Contains(fieldAPIName))
                    {
                        record.SetFieldValue(fieldAPIName, token.Value);
                    }
                    else{
                        record.SetProperty(fieldAPIName, token.Value);
                    }
                }
                else if (token.Value is JObject)
                {
                    JObject lookupDetails = (JObject)token.Value;
                    ZCRMRecord lookupRecord = ZCRMRecord.GetInstance(fieldAPIName, Convert.ToInt64(lookupDetails.GetValue("id")));
                    lookupRecord.LookupLabel = Convert.ToString(lookupDetails.GetValue("name"));
                    record.SetFieldValue(fieldAPIName, lookupRecord);
                }
                else if(token.Value is JArray)
                {
                    JArray jsonArray = (JArray)token.Value;
                    List<object> values = new List<object>();
                    foreach(JObject jsonObject in jsonArray)
                    {
                        values.Add(jsonObject);
                    }
                    record.SetFieldValue(fieldAPIName, values);
                }
                else{
                    record.SetFieldValue(fieldAPIName, token.Value);
                }
            }
        }

        private void SetParticipants(JArray participants)
        {
            foreach(JObject participantDetails in participants)
            {
                record.AddParticipant(GetZCRMParticipant(participantDetails));
            }
        }


        private void SetInventoryLineItems(JArray lineItems)
        {
            foreach(JObject lineItem in lineItems)
            {
                record.AddLineItem(GetZCRMInventoryLineItem(lineItem));
            }
        }

        private void SetPriceDetails(JArray priceDetails)
        {
            foreach(JObject priceDetail in priceDetails)
            {
                record.AddPriceDetail(GetZCRMPriceDetail(priceDetail));
            }
        }


        //TODO: Plenty of optimization needed;
        public ZCRMInventoryLineItem GetZCRMInventoryLineItem(JObject lineItemJSON)
        {
            JObject productDetails = (JObject)lineItemJSON.GetValue("product");
            long lineItemId = Convert.ToInt64(lineItemJSON.GetValue("id"));
            ZCRMInventoryLineItem lineItem = ZCRMInventoryLineItem.GetInstance(lineItemId);

            ZCRMRecord product = ZCRMRecord.GetInstance("Products", Convert.ToInt64(productDetails.GetValue("id")));
            product.LookupLabel = (string)productDetails.GetValue("name");
            lineItem.Product = product;
            lineItem.Description = Convert.ToString(lineItemJSON.GetValue("product_description"));
            lineItem.Quantity = Convert.ToDouble(lineItemJSON.GetValue("quantity"));
            lineItem.ListPrice = Convert.ToDouble(lineItemJSON.GetValue("list_price"));
            lineItem.UnitPrice = Convert.ToDouble(lineItemJSON.GetValue("unit_price"));
            lineItem.Total = Convert.ToDouble(lineItemJSON.GetValue("total"));
            lineItem.Discount = Convert.ToDouble(lineItemJSON.GetValue("Discount"));
            lineItem.TotalAfterDiscount = Convert.ToDouble(lineItemJSON.GetValue("tota_after_discount"));
            lineItem.TaxAmount = Convert.ToDouble(lineItemJSON.GetValue("Tax"));
            JArray lineTaxes = (JArray)lineItemJSON.GetValue("line_tax");
            foreach(JObject lineTax in lineTaxes)
            {
                ZCRMTax tax = ZCRMTax.GetInstance(Convert.ToString(lineTax.GetValue("name")));
                tax.Percentage = Convert.ToDouble(lineTax.GetValue("percentage"));
                tax.Value = Convert.ToDouble(lineTax.GetValue("value"));
                lineItem.AddLineTax(tax);
            }
            lineItem.NetTotal = Convert.ToDouble(lineItemJSON.GetValue("net_total"));
            return lineItem;
        }

        private ZCRMEventParticipant GetZCRMParticipant(JObject participantDetails)
        {
            long participantId = Convert.ToInt64(participantDetails.GetValue("participant"));
            string type = Convert.ToString(participantDetails.GetValue("type"));

            ZCRMEventParticipant participant = ZCRMEventParticipant.GetInstance(type, participantId);
            participant.Name = Convert.ToString(participantDetails.GetValue("name"));
            participant.Email = Convert.ToString(participantDetails.GetValue("Email"));
            participant.IsInvited = Convert.ToBoolean(participantDetails.GetValue("invited"));
            participant.Status = Convert.ToString(participantDetails.GetValue("status"));

            return participant;
        }

        private ZCRMPriceBookPricing GetZCRMPriceDetail(JObject priceDetail)
        {
            long id = Convert.ToInt64(priceDetail.GetValue("id"));

            ZCRMPriceBookPricing pricing = ZCRMPriceBookPricing.GetInstance(id);
            pricing.Discount = Convert.ToDouble(priceDetail.GetValue("discount"));
            pricing.ToRange = Convert.ToDouble(priceDetail.GetValue("to_range"));
            pricing.FromRange = Convert.ToDouble(priceDetail.GetValue("from_range"));

            return pricing;
        }

        //TODO: Implement the remaining Methods;
       
        //TODO: After completing the sdk, try to serialize the objects directly instead of the implemented methods;
        public JObject GetZCRMRecordAsJSON()
        {
            JObject recordJSON = new JObject();
            Dictionary<string, object> recordData = record.Data;
            if(record.Owner != null)
            {
                recordJSON.Add("Owner", record.Owner.Id.ToString());
            }
            if(record.Layout != null)
            {
                recordJSON.Add("Layout", record.Layout.Id.ToString());
            }
            MapAsJSON(recordData, recordJSON);
            recordJSON.Add("Product_Details", GetLineItemsAsJSONArray());
            recordJSON.Add("Participants", GetParticipantsAsJSONArray());
            recordJSON.Add("Pricing_Details", GetPriceDetailsAsJSONArray());
            recordJSON.Add("Tax", GetTaxAsJSONArray());

            return recordJSON;
        }



        //TODO: Check this method;
        private void MapAsJSON(Dictionary<string, object> recordData, JObject recordJSON)
        {
            foreach(KeyValuePair<string, object> data in recordData)
            {
                object value = data.Value;
                Console.WriteLine(data.Key + " - " + value);
                if(value == null)
                {
                    value = null;
                }
                else if(value is ZCRMRecord)
                {
                    value = ((ZCRMRecord)value).EntityId.ToString();
                }
                else if(value is ZCRMUser)
                {
                    value = ((ZCRMUser)value).Id.ToString();
                }
                else if (value is List<object>)
                {
                    JArray jsonArray = new JArray();
                    foreach(object valueObject in (List<object>)value)
                    {
                        if(valueObject is ZCRMSubform)
                        {
                            jsonArray.Add(GetSubformAsJSON((ZCRMSubform)valueObject));
                        }
                        else{
                            jsonArray.Add(valueObject);
                        }
                    }
                    value = jsonArray;
                }

                recordJSON.Add(data.Key, value.ToString());
            }
        }

        private JObject GetSubformAsJSON(ZCRMSubform subform)
        {
            JObject subformJSON = new JObject();
            Dictionary<string, object> subformData = subform.Data;
            if(subform.EntityId != null)
            {
                subformJSON.Add("id", subform.EntityId.ToString());
            }
            if(subform.Owner != null)
            {
                subformJSON.Add("Owner", subform.Owner.Id.ToString());
            }
            if(subform.Layout != null)
            {
                subformJSON.Add("Layout", subform.Layout.Id.ToString());
            }
            MapAsJSON(subformData, subformJSON);

            return subformJSON;
        }

        private JArray GetLineItemsAsJSONArray()
        {
            if(record.LineItems.Count == 0){
                return null;
            }
            JArray lineItems = new JArray();
            List<ZCRMInventoryLineItem> lineItemsList = record.LineItems;
            foreach(ZCRMInventoryLineItem inventoryLineItem in lineItemsList)
            {
                lineItems.Add(GetZCRMInventoryLineItemAsJSON(inventoryLineItem));
            }
            return lineItems;
        }

        //TODO: Throws exception (Handle exceptions);
        //TODO: Inspect the usage of discount_percentage;
        private JObject GetZCRMInventoryLineItemAsJSON(ZCRMInventoryLineItem inventoryLineItem)
        {
            JObject lineItem = new JObject();
            if(inventoryLineItem.Id != null)
            {
                lineItem.Add("id", inventoryLineItem.Id.ToString());
            }
            if(inventoryLineItem.Product != null)
            {
                lineItem.Add("product", inventoryLineItem.Product.EntityId);
            }

            lineItem.Add("product_description", inventoryLineItem.Description);
            lineItem.Add("list_price", inventoryLineItem.ListPrice);
            lineItem.Add("quantity", inventoryLineItem.Quantity);

            if(inventoryLineItem.DiscountPercentage == null)
            {
                lineItem.Add("Discount", inventoryLineItem.Discount);
            }
            else
            {
                lineItem.Add("Discount", inventoryLineItem.DiscountPercentage + "%");
            }

            JArray lineTaxArray = new JArray();
            List<ZCRMTax> taxes = inventoryLineItem.LineTax;
            foreach(ZCRMTax tax in taxes)
            {
                JObject lineTax = new JObject();
                lineTax.Add("name", tax.TaxName);
                lineTax.Add("value", tax.Value);
                lineTax.Add("percentage", tax.Percentage);
                lineTaxArray.Add(lineTax);
            }
            lineItem.Add("line_tax", lineTaxArray);

            return lineItem;

        }

        //TODO: Handle exceptions appropriately;
        private JArray GetParticipantsAsJSONArray()
        {
            if (record.Participants.Count == 0)
            {
                return null;
            }
            JArray participants = new JArray();
            List<ZCRMEventParticipant> participantsList = record.Participants;
            foreach(ZCRMEventParticipant participant in participantsList)
            {
                participants.Add(GetZCRMParticipantsAsJSON(participant));
            }
            return participants;
        }

        //TODO: Handle exceptions;
        private JObject GetZCRMParticipantsAsJSON(ZCRMEventParticipant participant)
        {
            JObject participantJSON = new JObject();
            participantJSON.Add("participant", participant.Id.ToString());
            participantJSON.Add("type", participant.Type);
            participantJSON.Add("name", participant.Name);
            participantJSON.Add("Email", participant.Email);
            participantJSON.Add("invited", participant.IsInvited);
            participantJSON.Add("status", participant.Status);

            return participantJSON;
        }

        //TODO: Handle exceptions;
        private JArray GetPriceDetailsAsJSONArray()
        {
            if (record.PriceDetails.Count == 0) { return null; }

            JArray priceDetails = new JArray();
            List<ZCRMPriceBookPricing> priceDetailsList = record.PriceDetails;
            foreach(ZCRMPriceBookPricing priceDetail in priceDetailsList)
            {
                priceDetails.Add(GetZCRMPriceDetailAsJSON(priceDetail));
            }
            return priceDetails;
        }

        //TODO: Handle exceptions
        private JObject GetZCRMPriceDetailAsJSON(ZCRMPriceBookPricing priceDetail)
        {
            JObject priceDetailJSON = new JObject();
            if(priceDetail.Id != null)
            {
                priceDetailJSON.Add("id", priceDetail.Id.ToString());
            }
            priceDetailJSON.Add("discount", priceDetail.Discount);
            priceDetailJSON.Add("to_range", priceDetail.ToRange);
            priceDetailJSON.Add("from_range", priceDetail.FromRange);

            return priceDetailJSON;
        }

        //TODO: Handle exceptions appropriately;
        private JArray GetTaxAsJSONArray()
        {
            if(record.TaxList.Count == 0)
            {
                return null;
            }
            JArray taxes = new JArray();
            List<ZCRMTax> taxList = record.TaxList;
            if(record.ModuleAPIName.Equals("Products"))
            {
                foreach (ZCRMTax tax in taxList)
                {
                    taxes.Add(tax.TaxName);
                }
            }
            else{
                foreach(ZCRMTax tax in taxList)
                {
                    JObject taxObject = new JObject();
                    taxObject.Add("percentage", tax.Percentage);
                    taxObject.Add("name", tax.TaxName);
                    taxes.Add(taxObject);
                }
            }
            return taxes;
        }
    }
}
