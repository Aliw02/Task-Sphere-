
using DAL;

namespace BLL
{

    
    public class BLL_Task
    {

        public static List<(int ID, string Title)> GetEmployeeTitles()
        {
            return DAL_Person.GetAllEmployeeTitles();
        }
        
    }
}
