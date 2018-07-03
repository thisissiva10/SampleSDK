using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using SampleSDK.CRM.Library.Api.Response;
using SampleSDK.CRM.Library.CRUD;
using SampleSDK.CRM.Library.CRMException;
using SampleSDK.CRM.Library.Setup.MetaData;
using SampleSDK.CRM.Library.Setup.Users;
using SampleSDK.CRM.Library.Common;

namespace SampleSDK.CRM.Library.Api.Handler
{
    public class OrganizationAPIHandler : CommonAPIHandler, IAPIHandler
    {

        private OrganizationAPIHandler() { }

        public static OrganizationAPIHandler GetInstance()
        {
            return new OrganizationAPIHandler();
        }


        //TOOD: Handle exceptions;
        public APIResponse GetOrganizationDetails()
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "org";

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JObject responseJSON = response.ResponseJSON;
                JArray orgArray = (JArray)responseJSON.GetValue("org");
                response.Data = GetZCRMOrganization((JObject)orgArray[0]);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("Exception catught in GetOrganizationDetails");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        //TOOD: Handle exceptions;
        private ZCRMOrganization GetZCRMOrganization(JObject orgDetails)
        {
            ZCRMOrganization organization = ZCRMOrganization.GetInstance(Convert.ToString(orgDetails.GetValue("compan_name")), Convert.ToInt64(orgDetails.GetValue("id")));
            organization.Alias = Convert.ToString(orgDetails.GetValue("alias"));
            organization.PrimaryZuid = Convert.ToInt64(orgDetails.GetValue("primary_zuid"));
            organization.Zgid = Convert.ToInt64(orgDetails.GetValue("zgid"));
            organization.Phone = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Mobile = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Website = Convert.ToString(orgDetails.GetValue("alias"));
            organization.PrimaryEmail = Convert.ToString(orgDetails.GetValue("alias"));
            organization.EmployeeCount = Convert.ToInt32(orgDetails.GetValue("alias"));
            organization.Description = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Timezone = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Iso_code = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Currency_locale = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Currency_symbol = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Street = Convert.ToString(orgDetails.GetValue("alias"));
            organization.State = Convert.ToString(orgDetails.GetValue("alias"));
            organization.City = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Country = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Country_code = Convert.ToString(orgDetails.GetValue("alias"));
            organization.ZipCode = Convert.ToString(orgDetails.GetValue("alias"));
            organization.Mc_status = Convert.ToBoolean(orgDetails.GetValue("alias"));
            organization.Gapps_enabled = Convert.ToBoolean(orgDetails.GetValue("alias"));

            return organization;
        }

        //TOOD: Handle exceptions;
        public APIResponse GetUser(long? userId)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                if (userId != null)
                {
                    urlPath = "users/" + userId;
                }
                else
                {
                    urlPath = "users";
                    requestQueryParams.Add("type", "CurrentUser");
                }

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JArray usersArray = (JArray)response.ResponseJSON.GetValue("users");
                response.Data = GetZCRMUser((JObject)usersArray[0]);

                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the exceptions;

                Console.WriteLine("exception in GetUser");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        //TODO: Handle exceptions for all the below functions;
        private BulkAPIResponse<ZCRMUser> GetUsers(string type, string modifiedSince, int page, int perPage)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "users";
                requestQueryParams.Add("type", type);
                requestQueryParams.Add("page", page.ToString());
                requestQueryParams.Add("per_page", perPage.ToString());
                requestHeaders.Add("If-Modified-Since", modifiedSince);

                BulkAPIResponse<ZCRMUser> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMUser>();

                List<ZCRMUser> allUsers = new List<ZCRMUser>();
                JObject responseJSON = response.ResponseJSON;
                JArray usersArray = (JArray)responseJSON.GetValue("users");
                foreach (JObject userJSON in usersArray)
                {
                    allUsers.Add(GetZCRMUser(userJSON));
                }
                response.BulkData = allUsers;
                return response;
            }
            catch (Exception e)
            {
                //TODO: Handle exceptions;
                Console.WriteLine("Exception at GetUsers");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        public APIResponse GetCurrentUser()
        {
            return GetUser(null);
        }


        public BulkAPIResponse<ZCRMUser> GetAllUsers(string modifiedSince, int page, int perPage)
        {
            return GetUsers(null, modifiedSince, page, perPage);
        }

        public BulkAPIResponse<ZCRMUser> GetAllActiveUsers(int page, int perPage)
        {
            return GetUsers("ActiveUsers", null, page, perPage);
        }

        public BulkAPIResponse<ZCRMUser> GetAllDeactivatedUsers(int page, int perPage)
        {
            return GetUsers("DeactivatedUsers", null, page, perPage);
        }

        public BulkAPIResponse<ZCRMUser> GetAllConfirmedUsers(int page, int perPage)
        {
            return GetUsers("ConfirmedUsers", null, page, perPage);
        }


        public BulkAPIResponse<ZCRMUser> GetAllUnConfirmedUsers(int page, int perPage)
        {
            return GetUsers("NotConfirmedUsers", null, page, perPage);
        }

        public BulkAPIResponse<ZCRMUser> GetAllDeletedUsers(int page, int perPage)
        {
            return GetUsers("DeletedUsers", null, page, perPage);
        }

        public BulkAPIResponse<ZCRMUser> GetAllActiveConfirmedUsers(int page, int perPage)
        {
            return GetUsers("ActiveConfirmedUsers", null, page, perPage);
        }


        public BulkAPIResponse<ZCRMUser> GetAllAdminUsers(int page, int perPage)
        {
            return GetUsers("AdminUsers", null, page, perPage);
        }

        public BulkAPIResponse<ZCRMUser> GetAllActiveConfirmedAdmins(int page, int perPage)
        {
            return GetUsers("ActiveConfirmedAdmins", null, page, perPage);
        }

        private ZCRMUser GetZCRMUser(JObject userDetails)
        {
            ZCRMUser user = ZCRMUser.GetInstance(Convert.ToInt64(userDetails.GetValue("id")), Convert.ToString(userDetails.GetValue("full_name")));
            user.EmailId = Convert.ToString(userDetails.GetValue("email"));
            user.FirstName = Convert.ToString(userDetails.GetValue("first_name"));
            user.LastName = Convert.ToString(userDetails.GetValue("last_name"));
            user.Language = Convert.ToString(userDetails.GetValue("language"));
            user.Mobile = Convert.ToString(userDetails.GetValue("mobile"));
            user.Status = Convert.ToString(userDetails.GetValue("status"));
            user.ZuId = Convert.ToInt64(userDetails.GetValue("zuid"));

            if (userDetails.ContainsKey("profile"))
            {
                JObject profileObject = (JObject)userDetails.GetValue("profile");
                ZCRMProfile profile = ZCRMProfile.GetInstance(Convert.ToInt64(profileObject.GetValue("id")), Convert.ToString(profileObject.GetValue("name")));
                user.Profile = profile;
            }
            if (userDetails.ContainsKey("role"))
            {
                JObject roleObject = (JObject)userDetails.GetValue("role");
                ZCRMRole role = ZCRMRole.GetInstance(Convert.ToInt64(roleObject.GetValue("id")), Convert.ToString(roleObject.GetValue("name")));
                user.Role = role;
            }

            user.Alias = Convert.ToString(userDetails.GetValue("alias"));
            user.City = Convert.ToString(userDetails.GetValue("city"));
            user.Confirm = Convert.ToBoolean(userDetails.GetValue("confirm"));
            user.CountryLocale = Convert.ToString(userDetails.GetValue("country_locale"));
            user.DateFormat = Convert.ToString(userDetails.GetValue("date_format"));
            user.TimeFormat = Convert.ToString(userDetails.GetValue("time_format"));
            user.DateOfBirth = Convert.ToString(userDetails.GetValue("dob"));
            user.Country = Convert.ToString(userDetails.GetValue("country"));
            user.Fax = Convert.ToString(userDetails.GetValue("fax"));
            user.Locale = Convert.ToString(userDetails.GetValue("locale"));
            user.NameFormat = Convert.ToString(userDetails.GetValue("name_format"));
            user.Website = Convert.ToString(userDetails.GetValue("website"));
            user.TimeZone = Convert.ToString(userDetails.GetValue("time_zone"));
            user.Street = Convert.ToString(userDetails.GetValue("street"));
            user.State = Convert.ToString(userDetails.GetValue("state"));
            if (userDetails.ContainsKey("Created_By"))
            {
                JObject createdByObject = (JObject)userDetails.GetValue("Created_By");
                ZCRMUser createdUser = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject.GetValue("id")), Convert.ToString(createdByObject.GetValue("name")));
                user.CreatedBy = createdUser;
                user.CreatedTime = Convert.ToString(userDetails.GetValue("Created_Time"));
            }
            if (userDetails.ContainsKey("Modified_By"))
            {
                JObject modifiedByObject = (JObject)userDetails.GetValue("Modified_By");
                ZCRMUser modifiedByUser = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
                user.ModifiedBy = modifiedByUser;
                user.ModifiedTime = Convert.ToString(userDetails.GetValue("Modified_Time"));
            }
            if (userDetails.ContainsKey("Reporting_To"))
            {
                JObject reportingToObject = (JObject)userDetails.GetValue("Reporting_To");
                ZCRMUser reportingTo = ZCRMUser.GetInstance(Convert.ToInt64(reportingToObject.GetValue("id")), Convert.ToString(reportingToObject.GetValue("name")));
                user.ReportingTo = reportingTo;
            }

            return user;
        }

        //TODO: Handle exceptions;
        public APIResponse GetRole(long roleId)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/roles/" + roleId;

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JObject responseJSON = response.ResponseJSON;
                JArray rolesArray = (JArray)responseJSON.GetValue("roles");
                response.Data = GetZCRMRole((JObject)rolesArray[0]);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Log the exception;
                Console.WriteLine("exception at GetRole");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }



        public BulkAPIResponse<ZCRMRole> GetAllRoles()
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/roles";

                BulkAPIResponse<ZCRMRole> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMRole>();

                List<ZCRMRole> allRoles = new List<ZCRMRole>();
                JObject responseJSON = response.ResponseJSON;
                JArray rolesArray = (JArray)responseJSON.GetValue("roles");
                foreach (JObject roleDetails in rolesArray)
                {
                    allRoles.Add(GetZCRMRole(roleDetails));
                }
                response.BulkData = allRoles;
                return response;
            }
            catch (Exception e)
            {
                //TODO: Handle exceptions and log the exceptions;
                Console.WriteLine("Exception in GetAllRoles");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }





        //TODO: Handle exceptions;
        private ZCRMRole GetZCRMRole(JObject roleDetails)
        {
            ZCRMRole role = ZCRMRole.GetInstance(Convert.ToInt64(roleDetails.GetValue("id")), Convert.ToString(roleDetails.GetValue("name")));
            role.Label = Convert.ToString(roleDetails.GetValue("display_label"));
            role.AdminUser = Convert.ToBoolean(roleDetails.GetValue("admin_user"));
            ZCRMRole reportingTo = null;
            if (roleDetails.GetValue("reporting_to") != null)
            {
                JObject reportingToObject = (JObject)roleDetails.GetValue("reporting_to");
                reportingTo = ZCRMRole.GetInstance(Convert.ToInt64(reportingToObject.GetValue("id")), Convert.ToString(reportingToObject.GetValue("name")));
            }
            role.ReportingTo = reportingTo;
            return role;
        }

        public APIResponse GetProfile(long profileId)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/profiles/" + profileId;

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JObject responseJSON = response.ResponseJSON;
                JArray rolesArray = (JArray)responseJSON.GetValue("profiles");
                response.Data = GetZCRMProfile((JObject)rolesArray[0]);
                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("exception at GetProfile");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        public BulkAPIResponse<ZCRMProfile> GetAllProfiles()
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "settings/profiles";

                BulkAPIResponse<ZCRMProfile> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMProfile>();

                List<ZCRMProfile> allProfiles = new List<ZCRMProfile>();
                JObject responseJSON = response.ResponseJSON;
                JArray profilesArray = (JArray)responseJSON.GetValue("profiles");
                foreach (JObject profileDetails in profilesArray)
                {
                    allProfiles.Add(GetZCRMProfile(profileDetails));
                }
                response.BulkData = allProfiles;
                return response;
            }
            catch (Exception e)
            {
                //TODO: Handle exceptions and log the exceptions;
                Console.WriteLine("Exception in GetAllProfiles");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        private ZCRMProfile GetZCRMProfile(JObject profileDetails)
        {
            ZCRMProfile profile = ZCRMProfile.GetInstance(Convert.ToInt64(profileDetails.GetValue("id")), Convert.ToString(profileDetails.GetValue("name")));
            profile.Category = Convert.ToBoolean(profileDetails.GetValue("category"));
            profile.Description = Convert.ToString(profileDetails.GetValue("description"));
            if (profileDetails.GetValue("created_by") != null)
            {
                JObject createdByObject = (JObject)profileDetails.GetValue("created_by");
                ZCRMUser createdBy = ZCRMUser.GetInstance(Convert.ToInt64(createdByObject.GetValue("id")), Convert.ToString(createdByObject.GetValue("name")));
                profile.CreatedBy = createdBy;
                profile.CreatedTime = Convert.ToString(profileDetails.GetValue("created_time"));
            }
            if (profileDetails.GetValue("modified_by") != null)
            {
                JObject modifiedByObject = (JObject)profileDetails.GetValue("modified_by");
                ZCRMUser modifiedBy = ZCRMUser.GetInstance(Convert.ToInt64(modifiedByObject.GetValue("id")), Convert.ToString(modifiedByObject.GetValue("name")));
                profile.ModifiedBy = modifiedBy;
                profile.ModifiedTime = Convert.ToString(profileDetails.GetValue("modified_time"));
            }

            return profile;
        }

        public APIResponse GetTax(long taxId)
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "org/taxes/" + taxId;

                APIResponse response = APIRequest.GetInstance(this).GetAPIResponse();

                JObject responseJSON = response.ResponseJSON;
                JArray taxArray = (JArray)responseJSON.GetValue("taxes");
                response.Data = GetZCRMOrgTax((JObject)taxArray[0]);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Handle exceptions and log the exceptions;
                Console.WriteLine("exception in GetTax");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNA_ERROR");
            }
        }

        public BulkAPIResponse<ZCRMOrgTax> GetAllTaxes()
        {
            try
            {
                requestMethod = APIConstants.RequestMethod.GET;
                urlPath = "org/taxes";


                BulkAPIResponse<ZCRMOrgTax> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMOrgTax>();

                JObject responseJSON = response.ResponseJSON;
                response.BulkData = GetAllZCRMOrgTax(responseJSON);
                return response;
            }
            catch (Exception e)
            {
                //TODO: Handle exceptions and log the excepitons;
                Console.WriteLine("Exception in GetAllTaxes");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        private List<ZCRMOrgTax> GetAllZCRMOrgTax(JObject responseJSON)
        {
            List<ZCRMOrgTax> allOrgTaxes = new List<ZCRMOrgTax>();
            JArray taxArray = (JArray)responseJSON.GetValue("taxes");
            foreach (JObject taxDetails in taxArray)
            {
                allOrgTaxes.Add(GetZCRMOrgTax(taxDetails));
            }
            return allOrgTaxes;
        }

        private ZCRMOrgTax GetZCRMOrgTax(JObject taxDetails)
        {
            ZCRMOrgTax tax = ZCRMOrgTax.GetInstance(Convert.ToInt64(taxDetails.GetValue("id")));
            tax.Name = Convert.ToString(taxDetails.GetValue("name"));
            tax.DisplayName = Convert.ToString(taxDetails.GetValue("display_label"));
            tax.Value = Convert.ToDouble(taxDetails.GetValue("value"));
            tax.Sequence = Convert.ToInt32(taxDetails.GetValue("sequence_number"));
            return tax;
        }

        private JObject GetZCRMOrgTaxAsJSON(ZCRMOrgTax tax)
        {
            JObject taxJSON = new JObject();
            taxJSON.Add("name", tax.Name);
            taxJSON.Add("id", tax.Id);
            taxJSON.Add("display_label", tax.DisplayName);
            taxJSON.Add("value", tax.Value);
            taxJSON.Add("sequence_number", tax.Sequence);
            return taxJSON;

        }

        public BulkAPIResponse<ZCRMOrgTax> CreateTaxes(List<ZCRMOrgTax> taxes)
        {
            if (taxes.Count > 100)
            {
                throw new ZCRMException("Cannot process more than 100 records at a time");
            }
            try
            {
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = "org/taxes";
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                foreach (ZCRMOrgTax tax in taxes)
                {
                    dataArray.Add(GetZCRMOrgTaxAsJSON(tax));
                }
                requestBodyObject.Add("taxes", dataArray);
                requestBody = requestBodyObject;

                BulkAPIResponse<ZCRMOrgTax> response = APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMOrgTax>();

                List<ZCRMOrgTax> createdTaxes = new List<ZCRMOrgTax>();
                List<EntityResponse> responses = response.BulkEntitiesResponse;
                foreach (EntityResponse entityResponse in responses)
                {
                    if (entityResponse.Status.Equals("success"))
                    {
                        JObject responseData = entityResponse.ResponseJSON;
                        JObject responseDetails = (JObject)responseData.GetValue("details");
                        ZCRMOrgTax tax = GetZCRMOrgTax(responseDetails);
                        createdTaxes.Add(tax);
                        entityResponse.Data = tax;
                    }
                    else
                    {
                        entityResponse.Data = null;
                    }
                }
                response.BulkData = createdTaxes;
                return response;
            }
            catch (Exception e)
            {
                //TODO: log the exceptions;
                Console.WriteLine("Exception in Create Taxes");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        public BulkAPIResponse<ZCRMOrgTax> UpdateTaxes(List<ZCRMOrgTax> taxes)
        {
            if (taxes.Count > 100)
            {
                throw new ZCRMException("Cannot process more than 100 records at a time");
            }
            try
            {
                requestMethod = APIConstants.RequestMethod.POST;
                urlPath = "org/taxes";
                JObject requestBodyObject = new JObject();
                JArray dataArray = new JArray();
                foreach (ZCRMOrgTax tax in taxes)
                {
                    dataArray.Add(GetZCRMOrgTaxAsJSON(tax));
                }
                requestBodyObject.Add("taxes", dataArray);
                requestBody = requestBodyObject;

                return APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMOrgTax>();
            }
            catch (Exception e)
            {
                //TODO: Handle exceptions and log the exceptions;
                Console.WriteLine("Exception in  UpdateTaxes");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }

        public BulkAPIResponse<ZCRMOrgTax> DeleteTaxes(List<long> taxIds)
        {
            if(taxIds.Count > 100)
            {
                throw new ZCRMException("Cannot process more than 100 records at a time");
            }

            try{
                requestMethod = APIConstants.RequestMethod.DELETE;
                urlPath = "org/taxes";
                requestQueryParams.Add("ids", CommonUtil.CollectionToCommaDelimitedString(taxIds));

                return APIRequest.GetInstance(this).GetBulkAPIResponse<ZCRMOrgTax>();
            }catch(Exception e){
                //TODO: Log the exceptions;
                Console.WriteLine("Exception in DeleteTaxes");
                Console.WriteLine(e);
                throw new ZCRMException("ZCRM_INTERNAL_ERROR");
            }
        }
    }
}
