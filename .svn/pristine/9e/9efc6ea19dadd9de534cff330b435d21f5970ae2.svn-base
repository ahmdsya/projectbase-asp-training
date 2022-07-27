using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace ProjectBase.Utilities
{
    public class UUtils
    {
        public static bool UserHasAuthorization(string username, string action, string controller, out Group_Menu menuAuth)
        {
            ProjectBaseEntities db = new ProjectBaseEntities();

            menuAuth = new Group_Menu();
            bool hasAuth = false;
            string query = @"select gm.*
                                from Group_Menu gm
		                        INNER JOIN Menu m ON m.Id = gm.Menu_Id
	                            INNER JOIN [Group] g ON g.Id = gm.Group_Id
	                            INNER JOIN [User_Group] ug ON ug.Group_Id = g.Id
	                            INNER JOIN [User] u ON u.Id = ug.User_Id
	                            WHERE u.Employee_Number = @emp
			                        AND m.Menu_Action = @action
			                        AND m.Menu_Controller = @controller ";
            List<SqlParameter> sqlParams = new List<SqlParameter>();
            sqlParams.Add(new SqlParameter("@emp", username));
            sqlParams.Add(new SqlParameter("@action", action));
            sqlParams.Add(new SqlParameter("@controller", controller));

            try
            {
                Group_Menu[] groupMenus = db.Database.SqlQuery<Group_Menu>(query, sqlParams.ToArray()).ToArray();

                if (groupMenus.Length > 0)
                {
                    hasAuth = true;

                    menuAuth.Menu_Id = groupMenus[0].Menu_Id;

                    int countView = groupMenus.Where(r => r.View == true).Count();
                    int countEdit = groupMenus.Where(r => r.Edit == true).Count();
                    int countDelete = groupMenus.Where(r => r.Delete == true).Count();
                    int countCreate = groupMenus.Where(r => r.Create == true).Count();
                    int countPrint = groupMenus.Where(r => r.Print == true).Count();

                    if (countView > 0) menuAuth.View = true;
                    else menuAuth.View = false;

                    if (countEdit > 0) menuAuth.Edit = true;
                    else menuAuth.Edit = false;

                    if (countDelete > 0) menuAuth.Delete = true;
                    else menuAuth.Delete = false;

                    if (countCreate > 0) menuAuth.Create = true;
                    else menuAuth.Create = false;

                    if (countPrint > 0) menuAuth.Print = true;
                    else menuAuth.Print = false;
                }
            }
            catch (Exception ex)
            {
                string a = ex.Message;
                hasAuth = false;
            }

            return hasAuth;
        }

        public static bool UserRegisteredInAD(string user)
        {
            UserPrincipal userData = null;
            try
            {
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Constants.AD_DOMAIN, Constants.AD_SA_USER, Constants.AD_SA_PASSWORD))
                {
                    userData = UserPrincipal.FindByIdentity(ctx, user);
                    if (userData == null)
                        return false;
                    else
                        return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static string GetAppVersion()
        {
            ProjectBaseEntities db = new ProjectBaseEntities();
            var version = db.Settings.Where(r => r.Code == Constants.SETTING_VERSION).FirstOrDefault();
            return version == null ? "0" : version.Value;
        }

    }
}