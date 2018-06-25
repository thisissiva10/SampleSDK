using System;
using SampleSDK.CRM.Library.Common;

namespace SampleSDK.CRM.Library.Setup.Users
{
    public class ZCRMRole : ZCRMEntity
    {

        private long id;
        private string name;
        private string label;
        private Boolean isAdminUser;
        private ZCRMRole reportingTo;


        private ZCRMRole(long roleId, string roleName)
        {
            Id = roleId;
            Name = roleName;
        }

        public static ZCRMRole GetInstance(long roleId, string roleName)
        {
            return new ZCRMRole(roleId, roleName);
        }

        //Note: Id and Name cannot be set outside the class;

        public long Id { get => id; private set => id = value; }

        public string Name { get => name; private set => name = value; }

        public string Label { get => label; set => label = value; }

        //Note: Property name changed from IsAdminUser to AdminUser;
        public bool AdminUser { get => isAdminUser; set => isAdminUser = value; }

        public ZCRMRole ReportingTo { get => reportingTo; set => reportingTo = value; }
    }
}
