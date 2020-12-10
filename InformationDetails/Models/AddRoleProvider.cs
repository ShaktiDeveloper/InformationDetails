using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace InformationDetails.Models
{
    public class AddRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (var informationDetailsEntities = new InformationDetailsEntities())
            {
                var Role = (from User in informationDetailsEntities.UserLogins
                         join Roles in informationDetailsEntities.UserRoles on User.Id equals Roles.Id
                         where User.Fullname == username
                         select Roles.RoleName).ToArray();
                //orderby od.OrderID
                //select new
                //{
                //    od.OrderID,
                //    pd.ProductID,
                //    pd.Name,
                //    pd.UnitPrice,
                //    od.Quantity,
                //    od.Price,
                //}).ToList();
                return Role;
            }
                
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}