using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectBase.Utilities
{
    public class UMenu
    {
        public static Menu[] GetAuthorizedMenu(string username, bool isAdmin)
        {
            ProjectBaseEntities db = new ProjectBaseEntities();
            Menu[] menu = new Menu[0];
            string query = @";with name_tree as 
                            (
                                SELECT m.*
	                            FROM Menu m
	                            {0}
                                union all
                                select C.*
                                from menu c 

                                inner join name_tree p on c.id = p.parent
	                            --and c.id <> c.parent
                            ) 

                            SELECT distinct * FROM name_tree";
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EMP", username));

            if (!isAdmin) query = string.Format(query, @"INNER JOIN Group_Menu gm ON gm.Menu_Id = m.Id
	                            INNER JOIN [Group] g ON g.Id = gm.Group_Id
	                            INNER JOIN [User_Group] ug ON ug.Group_Id = g.Id
	                            INNER JOIN [User] u ON u.Id = ug.User_Id
	                            WHERE u.Employee_Number = @EMP AND m.Visible = 1");
            else query = string.Format(query, " WHERE m.MENU_ACTION IS NOT NULL AND m.Visible = 1 ");


            Menu[] listOfData_ = db.Database.SqlQuery<Menu>(query, parameters.ToArray()).ToArray();

            return listOfData_;
        }

        private static IList<Menu> GetChildrenMenu(IList<Menu> menuList, int parentId = 0)
        {
            return menuList.Where(x => x.Parent == parentId).OrderBy(x => x.Ordinal).ToList();
        }
        private static Menu GetMenuItem(IList<Menu> menuList, int id)
        {
            return menuList.FirstOrDefault(x => x.Id == id);
        }

        public static string GetMenuString(Menu[] menuItems, RequestContext context)
        {
            //var menuItems = GetAuthorizedMenu(username);

            var builder = new StringBuilder();
            builder.Append(GetMenuLiString(menuItems, 0, context));
            return builder.ToString();

        }

        private static string GetMenuLiString(IList<Menu> menuList, int parentId, RequestContext context)
        {
            var children = GetChildrenMenu(menuList, parentId);

            if (children.Count <= 0)
            {
                return "";
            }

            var builder = new StringBuilder();

            foreach (var item in children)
            {
                var childStr = GetMenuLiString(menuList, (int)item.Id, context);
                if (!string.IsNullOrWhiteSpace(childStr))
                {
                    builder.Append("<li class=\"treeview\">");
                    builder.Append("<a href=\"#\">");
                    builder.AppendFormat("<i class=\"{0}\"></i> <span>{1}</span>", item.Menu_Icon, item.Menu_Name);
                    builder.Append("<span class=\"pull-right-container\">");
                    builder.Append("<i class=\"fa fa-angle-left pull-right\"></i>");
                    builder.Append("</span>");
                    builder.Append("</a>");
                    builder.Append("<ul class=\"treeview-menu\">");
                    builder.Append(childStr);
                    builder.Append("</ul>");
                    builder.Append("</li>");
                }
                else
                {
                    builder.Append("<li>");

                    string[] splitted = item.Menu_Action.Split('?');
                    string param = "";
                    string action = item.Menu_Action;

                    if (splitted.Length > 1)
                    {
                        param = splitted[1];
                        action = splitted[0];
                    }

                    builder.AppendFormat("<a href=\"{0}\">", new UrlHelper(context).Action(action, item.Menu_Controller) + (string.IsNullOrEmpty(param) ? "" : "?" + param));
                    builder.AppendFormat("<i class=\"{0}\"></i> <span>{1}</span>", item.Menu_Icon, item.Menu_Name);
                    builder.Append("</a>");
                    builder.Append("</li>");
                }
            }

            return builder.ToString();
        }

    }
}