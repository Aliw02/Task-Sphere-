using DAL;
using System.Net.Http.Json;
using static BLL.BLL_Task;


namespace BLL
{
    public class Notification
    {
        public static bool Subscribe(BLL_Task task)
        {
            try
            {
                task.NotifyIfAdded += HandleTaskNotify;
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
                task.NotifyIfAdded -= HandleTaskNotify;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        private static async void HandleTaskNotify(object? sender, NotificationsEventArgs e)
        {
            var tsk = e?.Task;
            Console.WriteLine($"\n\nTask: {tsk?.TaskID}, {tsk?.TaskName}, Listed By User: {tsk?.EmployeeHasListed?.EmployeeDetails?.FullName}");

            try
            {

                if(tsk is null)
                {
                    Console.WriteLine("Task is null");
                    return;
                }
                string[] TaskStatus = ["", "TaskListting", "TaskTaken", "TaskCompleted", "TaskCancelled"];

                using HttpClient client = new();
                // Fix the URL to match your controller route
                var apiUrl = $"http://localhost:5062/api/TaskSphere/Notifications/{TaskStatus[tsk.TaskStatusID]}";
                var response = await client.PostAsJsonAsync(apiUrl, e?.Task?.TDTO);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"\n\n\nFailed to notify the API. Status code: {response.StatusCode}, Error: {errorContent}");
                }
                else
                {
                    Console.WriteLine($"Successfully notified the API about the {GetTaskStatus(tsk.TaskStatusID)}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in HandleTaskNotify: {ex.Message}");
            }
        }


        private static string GetTaskStatus(int TaskStatusID)
        {
            return TaskStatusID switch
            {
                1 => "Task Listting",
                2 => "Task Taken",
                3 => "Task Completed",
                4 => "Task Cancelled",
                _ => "Unknown"
            };
        }

    }
}
