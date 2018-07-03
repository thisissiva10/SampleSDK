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
                JArray orgArray = (JArray)responseJSON["org"];
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
            ZCRMOrganization organization = ZCRMOrganization.GetInstance((string)orgDetails["compan_name"], (long)orgDetails["id"]);
            organization.Alias = (string)orgDetails["alias"];
            organization.PrimaryZuid = (long)orgDetails["primary_zuid"];
            organization.Zgid = (long)orgDetails["zgid"];
            organization.Phone = (string)orgDetails["alias"];
            organization.Mobile = (string)orgDetails["alias"];
            organization.Website = (string)orgDetails["alias"];
            organization.PrimaryEmail = (string)orgDetails["alias"];
            organization.EmployeeCount = (int)orgDetails["alias"];
            organization.Description = (string)orgDetails["alias"];
            organization.Timezone = (string)orgDetails["alias"];
            organization.Iso_code = (string)orgDetails["alias"];
            organization.Currency_locale = (string)orgDetails["alias"];
            organization.Currency_symbol = (string)orgDetails["alias"];
            organization.Street = (string)orgDetails["alias"];
            organization.State = (string)orgDetails["alias"];
            organization.City = (string)orgDetails["alias"];
            organization.Country = (string)orgDetails["alias"];
            organization.Country_code = (string)orgDetails["alias"];
            organization.ZipCode = (string)orgDetails["alias"];
            organization.Mc_status = (bool)orgDetails["alias"];
            organization.Gapps_enabled = (bool)orgDetails["alias"];

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

                JArray usersArray = (JArray)response.ResponseJSON["users"];
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
                JArray usersArray = (JArray)responseJSON["users"];
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
            return GetUsers("DeactiveUsers", null, page, perPage);
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
            ZCRMUser user = ZCRMUser.GetInstance((long)userDetails["id"], (string)userDetails["full_name"]);
            user.EmailId = (string)userDetails["email"];
            user.FirstName = (string)userDetails["first_name"];
            user.LastName = (string)userDetails["last_name"];
            user.Language = (string)userDetails["language"];
            user.Mobile = (string)userDetails["mobile"];
            user.Status = (string)userDetails["status"];
            user.ZuId = (long?)userDetails["zuid"];
            if (userDetails.ContainsKey("profile"))
            {
                JObject profileObject = (JObject)userDetails["profile"];
                ZCRMProfile profile = ZCRMProfile.GetInstance((long)profileObject["id"], (string)profileObject["name"]);
                user.Profile = profile;
            }
            if (userDetails.ContainsKey("role"))
            {
                JObject roleObject = (JObject)userDetails["role"];
                ZCRMRole role = ZCRMRole.GetInstance((long)roleObject["id"], (string)roleObject["name"]);
                user.Role = role;
            }

            user.Alias = (string)userDetails["alias"];
            user.City = (string)userDetails["city"];
            user.Confirm = (bool)userDetails["confirm"];
            user.CountryLocale = (string)userDetails["country_locale"];
            user.DateFormat = (string)userDetails["date_format"];
            user.TimeFormat = (string)userDetails["time_format"];
            user.DateOfBirth = (string)userDetails["dob"];
            user.Country = (string)userDetails["country"];
            user.Fax = (string)userDetails["fax"];
            user.Locale = (string)userDetails["locale"];
            user.NameFormat = (string)userDetails["name_format"];
            user.Website = (string)userDetails["website"];
            user.TimeZone = (string)userDetails["time_zone"];
            user.Street = (string)userDetails["street"];
            user.State = (string)userDetails["state"];
            if (userDetails.ContainsKey("Created_By"))
            {
                JObject createdByObject = (JObject)userDetails["Created_By"];
                ZCRMUser createdUser = ZCRMUser.GetInstance((long)createdByObject["id"], (string)createdByObject["name"]);
                user.CreatedBy = createdUser;
                user.CreatedTime = (string)userDetails["Created_Time"];
            }
            if (userDetails.ContainsKey("Modified_By"))
            {
                JObject modifiedByObject = (JObject)userDetails["Modified_By"];
                ZCRMUser modifiedByUser = ZCRMUser.GetInstance((long)modifiedByObject["id"], (string)modifiedByObject["name"]);
                user.ModifiedBy = modifiedByUser;
                user.ModifiedTime = (string)userDetails["Modified_Time"];
            }
            if (userDetails.ContainsKey("Reporting_To"))
            {
                JObject reportingToObject = (JObject)userDetails["Reporting_To"];
                ZCRMUser reportingTo = ZCRMUser.GetInstance((long)reportingToObject["id"], (string)reportingToObject["name"]);
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
                JArray rolesArray = (JArray)responseJSON["roles"];
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
                JArray rolesArray = (JArray)responseJSON["roles"];
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
            ZCRMRole role = ZCRMRole.GetInstance((long)roleDetails["id"], (string)roleDetails["name"]);
            role.Label = (string)roleDetails["display_label"];
            role.AdminUser = (bool)roleDetails["admin_user"];
            ZCRMRole reportingTo = null;
            if (roleDetails["reporting_to"].Type != JTokenType.Null)
            {
                JObject reportingToObject = (JObject)roleDetails["reporting_to"];
                reportingTo = ZCRMRole.GetInstance((long)reportingToObject["id"], (string)reportingToObject["name"]);
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
                JArray rolesArray = (JArray)responseJSON["profiles"];
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
                JArray profilesArray = (JArray)responseJSON["profiles"];
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
            ZCRMProfile profile = ZCRMProfile.GetInstance((long)profileDetails["id"], (string)profileDetails["name"]);
            profile.Category = (bool)profileDetails["category"];
            profile.Description = (string)profileDetails["description"];
            if (profileDetails["created_by"] != null)
            {
                JObject createdByObject = (JObject)profileDetails["created_by"];
                ZCRMUser createdBy = ZCRMUser.GetInstance((long)createdByObject["id"], (string)createdByObject["name"]);
                profile.CreatedBy = createdBy;
                profile.CreatedTime = (string)profileDetails["created_time"];
            }
            if (profileDetails["modified_by"] != null)
            {
                JObject modifiedByObject = (JObject)profileDetails["modified_by"];
                ZCRMUser modifiedBy = ZCRMUser.GetInstance((long)modifiedByObject["id"], (string)modifiedByObject["name"]);
                profile.ModifiedBy = modifiedBy;
                profile.ModifiedTime = (string)profileDetails["modified_time"];
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
                JArray taxArray = (JArray)responseJSON["taxes"];
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
            JArray taxArray = (JArray)responseJSON["taxes"];
            foreach (JObject taxDetails in taxArray)
            {
                allOrgTaxes.Add(GetZCRMOrgTax(taxDetails));
            }
            return allOrgTaxes;
        }

        private ZCRMOrgTax GetZCRMOrgTax(JObject taxDetails)
        {
            ZCRMOrgTax tax = ZCRMOrgTax.GetInstance((long)taxDetails["id"]);
            tax.Name = (string)taxDetails["name"];
            tax.DisplayName = (string)taxDetails["display_label"];
            tax.Value = (double)taxDetails["value"];
            tax.Sequence = (int)taxDetails["sequence_number"];
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
                        JObject responseDetails = (JObject)responseData["details"];
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
