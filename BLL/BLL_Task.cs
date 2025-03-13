
using DAL;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using static BLL.Notification;

namespace BLL
{

    public class BLL_Task
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode;

        public enum enTaskStatus { NotStarted = 0, InProgress = 1, Completed = 2, Cancelled = 3 };
        public int TaskID { get; set; } = -1;
        public string TaskName { get; set; } = string.Empty;
        public int TaskStatusID { get; set; } = -1;
        public enTaskStatus TaskStatus { get; set; } = enTaskStatus.NotStarted;
        public int ListedBy { get; set; } = -1;
        public int? CompletedBy { get; set; } = -1;
        public DateTime ListedDate { get; set; } = DateTime.MinValue;
        public DateTime? CompletedDate { get; set; } = null;

        public BLL_Employee? EmployeeHasListed { get; private set; }

        public BLL_Employee? EmployeeHasCompleted{ get; private set; }

        //public readonly Notification Notify = new();

        [JsonIgnore]
        public TaskDTO TDTO { get { return new TaskDTO(this.TaskID, this.TaskName, this.TaskStatusID, this.ListedBy, this.CompletedBy, this.ListedDate, this.CompletedDate); } }


        public class NotificationsEventArgs(BLL_Task? task)
        {
            public BLL_Task? Task { get; set; } = task;
        }

        public event EventHandler<NotificationsEventArgs>? NotifyIfAdded;

        //public bool IsSubscribed { get; private set => value = Notification.Subscribe(this); }

        public BLL_Task(TaskDTO TDTO, enMode mode = enMode.AddNew)
        {
            // Initialize properties
            this.Mode = mode;
            this.TaskID = TDTO.ID;
            this.TaskName = TDTO.TaskName;
            this.TaskStatusID = TDTO.TaskStatusID;
            this.TaskStatus = (enTaskStatus)TDTO.TaskStatusID;
            this.ListedBy = TDTO.ListedBy;
            this.CompletedBy = TDTO.CompletedBy;
            this.ListedDate = TDTO.ListedDate;
            this.CompletedDate = TDTO.CompletedDate;

            this.EmployeeHasListed = BLL_Employee.Find(TDTO.ListedBy);
            this.EmployeeHasCompleted = TDTO.CompletedBy.HasValue ? BLL_Employee.Find(TDTO.CompletedBy.Value) : null;

            // ✅ Auto-subscribe to notification
            Notification.Subscribe(this);
        }

        public bool Complete()
        {
            if (this.TaskStatus == enTaskStatus.Completed || this.TaskStatus == enTaskStatus.Cancelled)
                return false;

            return DAL_Task.CompleteTask(this.TDTO);
        }

        public static BLL_Task? Find(int ID)
        {
            TaskDTO? TDTO = DAL_Task.Find(ID);
            if (TDTO is null)
                return null;
            else
            {
                //OnNewTaskAdded(new BLL_Task(TDTO));
                return new BLL_Task(TDTO);
            }
        }

        public static IEnumerable<TaskDTO> GetAll()
        {
            var Tasks = DAL_Task.GetAllTasks();

            return Tasks;
        }

        public static IEnumerable<TaskDTO> GetAllTasksBySpecificStatus(int StatusID)
        {
            var Tasks = DAL_Task.GetAllTasksBySpecificStatus(StatusID);
            return Tasks;
        }

        bool _AddNew()
        {
            this.TaskID = DAL_Task.AddTask(this.TDTO);  
            
            if(this.TaskID > 0)
            {
                OnNewTaskAdded(this);
                return true;
            }

            return false;
        }

        bool _Update()
        {
            return DAL_Task.UpdateTask(this.TDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNew())
                    {
                        Mode = enMode.Update;
                        return true;
                    }

                    else
                        return false;
                case enMode.Update:
                    return _Update();
            }
            return false;
        }

        public static IEnumerable<TaskDTO> GetAllTasksByEmployee(int EmployeeID)
        {
            var Tasks = DAL_Task.GetListedTasksByUser(EmployeeID);
            return Tasks;
        }

        public static IEnumerable<TaskDTO> GetAllCompletedTasksByEmployee(int EmployeeID)
        {
            var Tasks = DAL_Task.GetCompletedTasksByUser(EmployeeID);
            return Tasks;
        }

        public static IEnumerable<TaskDTO> GetAllListedTasksByEmployee(int EmployeeID)
        {
            var Tasks = DAL_Task.GetListedTasksByUser(EmployeeID);
            return Tasks;
        }

        protected virtual void OnNewTaskAdded(BLL_Task? task)
        {
            if (task is null)
                return;

            NotifyIfAdded?.Invoke(this, new NotificationsEventArgs(task));
        }

    }
}
