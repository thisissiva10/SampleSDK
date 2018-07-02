using System;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Common;

namespace SampleSDK.CRM.Library.Setup.MetaData
{
    public class ZCRMOrganization : ZCRMEntity
    {
        private string company_name;
        private string alias;
        private long? orgId;
        private long primaryZuid;
        private long zgid;
        private string primaryEmail;
        private string website;
        private string mobile;
        private string phone;
        private string fax;
        private int employeeCount;
        private string description;
        private string timezone;
        private string iso_code;
        private string currency_locale;
        private string currency_symbol;
        private string street;
        private string state;
        private string city;
        private string country;
        private string zipCode;
        private string country_code;
        private Boolean mc_status;
        private Boolean gapps_enabled;


        private ZCRMOrganization(string orgName, long? orgId)
        {
            Company_name = orgName;
            OrgId = orgId;
        }

        public static ZCRMOrganization GetInstance()
        {
            return new ZCRMOrganization(null, null);
        }

        public static ZCRMOrganization GetInstance(string orgName, long orgId)
        {
            return new ZCRMOrganization(orgName, orgId);
        }


        public string Company_name { get => company_name; private set => company_name = value; }
        public string Alias { get => alias; set => alias = value; }
        public long? OrgId { get => orgId; set => orgId = value; }
        public long PrimaryZuid { get => primaryZuid; set => primaryZuid = value; }
        public long Zgid { get => zgid; set => zgid = value; }
        public string PrimaryEmail { get => primaryEmail; set => primaryEmail = value; }
        public string Website { get => website; set => website = value; }
        public string Mobile { get => mobile; set => mobile = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Fax { get => fax; set => fax = value; }
        public int EmployeeCount { get => employeeCount; set => employeeCount = value; }
        public string Description { get => description; set => description = value; }
        public string Timezone { get => timezone; set => timezone = value; }
        public string Iso_code { get => iso_code; set => iso_code = value; }
        public string Street { get => street; set => street = value; }
        public string State { get => state; set => state = value; }
        public string City { get => city; set => city = value; }
        public string Country { get => country; set => country = value; }
        public string ZipCode { get => zipCode; set => zipCode = value; }
        public string Country_code { get => country_code; set => country_code = value; }
        public bool Mc_status { get => mc_status; set => mc_status = value; }
        public bool Gapps_enabled { get => gapps_enabled; set => gapps_enabled = value; }
        public string Currency_locale { get => currency_locale; set => currency_locale = value; }
        public string Currency_symbol { get => currency_symbol; set => currency_symbol = value; }


        //TODO: Handle exceptions;
        /* public BulkAPIResponse GetAllUsers()
         {

         }*/
    }
}
