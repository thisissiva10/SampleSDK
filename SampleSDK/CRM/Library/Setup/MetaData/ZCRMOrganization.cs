using System;
using System.Collections.Generic;
using SampleSDK.CRM.Library.Api.Handler;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.Common;
using SampleSDK.CRM.Library.CRMException;
using SampleSDK.CRM.Library.CRUD;
using SampleSDK.CRM.Library.Setup.Users;

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





        public BulkAPIResponse<ZCRMUser> GetAllUsers()
        {
            return GetAllUsers(null);
        }


        public BulkAPIResponse<ZCRMUser> GetAllUsers(string modifiedSince)
        {
            return OrganizationAPIHandler.GetInstance().GetAllUsers(modifiedSince, 1, 200);
        }

        public BulkAPIResponse<ZCRMUser> GetAllUsers(string modifiedSince, int page, int perPage)
        {
            return OrganizationAPIHandler.GetInstance().GetAllUsers(modifiedSince, page, perPage);
        }


        //TODO: AllConfirmedUsers, ALLUnConfirmedUsers, AllDeletedUsers get methods to be writter;

        public BulkAPIResponse<ZCRMUser> GetAllActiveConfirmedUsers()
        {
            return OrganizationAPIHandler.GetInstance().GetAllActiveConfirmedUsers(1, 200);
        }

        public BulkAPIResponse<ZCRMUser> GetAllActiveConfirmedUsers(int page, int perPage)
        {
            return OrganizationAPIHandler.GetInstance().GetAllActiveConfirmedUsers(page, perPage);
        }




       
        public BulkAPIResponse<ZCRMUser> GetAllAdminUsers()
        {
            return OrganizationAPIHandler.GetInstance().GetAllAdminUsers(1, 200);
        }

        public BulkAPIResponse<ZCRMUser> GetAllAdminUsers(int page, int perPage)
        {
            return OrganizationAPIHandler.GetInstance().GetAllAdminUsers(page, perPage);
        }



        public BulkAPIResponse<ZCRMUser> GetAllActiveUsers()
        {
            return OrganizationAPIHandler.GetInstance().GetAllActiveUsers(1, 200);
        }

        public BulkAPIResponse<ZCRMUser> GetAllActiveUsers(int page, int perPage)
        {
            return OrganizationAPIHandler.GetInstance().GetAllActiveUsers(page, perPage);
        }



        public BulkAPIResponse<ZCRMUser> GetAllInActiveUsers()
        {
            return OrganizationAPIHandler.GetInstance().GetAllDeactivatedUsers(1, 200);
        }

        public BulkAPIResponse<ZCRMUser> GetAllInActiveUsers(int page, int perPage)
        {
            return OrganizationAPIHandler.GetInstance().GetAllDeactivatedUsers(page, perPage);
        }


        public APIResponse GetUser(long userId)
        {
            return OrganizationAPIHandler.GetInstance().GetUser(userId);
        }


        public BulkAPIResponse<ZCRMRole> GetAllRoles()
        {
            return OrganizationAPIHandler.GetInstance().GetAllRoles();
        }



        public APIResponse GetRole(long roleId)
        {
            return OrganizationAPIHandler.GetInstance().GetRole(roleId);
        }

        public BulkAPIResponse<ZCRMProfile> GetAllProfiles()
        {
            return OrganizationAPIHandler.GetInstance().GetAllProfiles();
        }



        public APIResponse GetProfile(long profileId)
        {
            return OrganizationAPIHandler.GetInstance().GetProfile(profileId);
        }


        public BulkAPIResponse<ZCRMOrgTax> GetAllTaxes(long taxId)
        {
            return OrganizationAPIHandler.GetInstance().GetAllTaxes();
        }



        public APIResponse GetTax(long taxId)
        {
            return OrganizationAPIHandler.GetInstance().GetTax(taxId);
        }


        public BulkAPIResponse<ZCRMOrgTax> CreateTaxes(List<ZCRMOrgTax> taxes)
        {
            foreach (ZCRMOrgTax tax in taxes)
            {
                if (tax.Id != null)
                {
                    throw new ZCRMException("Tax ID MUST be null for create operation.");
                }
            }
            return OrganizationAPIHandler.GetInstance().CreateTaxes(taxes);
        }


        public BulkAPIResponse<ZCRMOrgTax> UpdateTaxes(List<ZCRMOrgTax> taxes)
        {
            foreach(ZCRMOrgTax tax in taxes)
            {
                if(tax.Id == null)
                {
                    throw new ZCRMException("Tax ID MUST NOT be null for update operation.");
                }
            }
            return OrganizationAPIHandler.GetInstance().UpdateTaxes(taxes);
        }
       
        public BulkAPIResponse<ZCRMOrgTax> DeleteTaxes(List<long> taxIds)
        {
            if(taxIds == null || taxIds.Count == 0)
            {
                throw new ZCRMException("Tax ID list MUST NOT be null/empty for delete operation");
            }
            return OrganizationAPIHandler.GetInstance().DeleteTaxes(taxIds);
        }

    }
}
