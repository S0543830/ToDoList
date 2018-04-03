using System.Web;
using System.Web.Mvc;

namespace ToDoList_Hassan_El_Bardan
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
