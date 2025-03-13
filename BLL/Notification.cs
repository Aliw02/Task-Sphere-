using DAL;
using System.Net.Http.Json;
using static BLL.BLL_Task;
//using System.

namespace BLL
{
    public class Notification
    {
        //public delegate bool OnNotify(DAL_Employee employee);
        //public static event OnNotify? Notify;


        public static bool Subscribe(BLL_Task task)
        {
            try
            {
                task.NotifyIfAdded += HandleTaskAdded;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }        
        
        public static bool Unsubscribe(BLL_Task task)
        {
            try
            {
                task.NotifyIfAdded -= HandleTaskAdded;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private static async void HandleTaskAdded(object? sender, NotificationsEventArgs e)
        {
            var tsk = e?.Task;
            Console.WriteLine($"\n\nTask: {tsk?.TaskID}, {tsk?.TaskName}, Listed By User: {tsk?.EmployeeHasListed?.EmployeeDetails?.FullName}");

            try
            {
                using HttpClient client = new();
                // Fix the URL to match your controller route
                var apiUrl = "http://localhost:5062/api/TaskSphere/Notifications/TaskListting";
                var response = await client.PostAsJsonAsync(apiUrl, e?.Task?.TDTO);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"\n\n\nFailed to notify the API. Status code: {response.StatusCode}, Error: {errorContent}");
                }
                else
                {
                    Console.WriteLine("Successfully notified the API about the new task.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in HandleTaskAdded: {ex.Message}");
            }
        }

    }
}
