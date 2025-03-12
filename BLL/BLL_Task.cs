
using DAL;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace BLL
{

    public class BLL_Task(TaskDTO TDTO, BLL.BLL_Task.enMode mode = BLL_Task.enMode.AddNew)
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = mode;

        public enum enTaskStatus { NotStarted = 0, InProgress = 1, Completed = 2, Cancelled = 3 };
        public int TaskID { get; set; } = TDTO.ID;
        public string TaskName { get; set; } = TDTO.TaskName;
        public int TaskStatusID { get; set; } = TDTO.TaskStatusID;
        public enTaskStatus TaskStatus { get; set; } = (enTaskStatus)TDTO.TaskStatusID;
        public int ListedBy { get; set; } = TDTO.ListedBy;
        public int? CompletedBy { get; set; } = TDTO.ListedBy;
        public DateTime ListedDate { get; set; } = TDTO.ListedDate;
        public DateTime? CompletedDate { get; set; } = TDTO.CompletedDate;

        public BLL_Employee? EmployeeHasListed { get; private set; } = BLL_Employee.Find(TDTO.ListedBy);

        public BLL_Employee? EmployeeHasCompleted
        {
            get
            {
                return TDTO.CompletedBy.HasValue ? BLL_Employee.Find(TDTO.CompletedBy.Value) : null;
            }
            private set { }
        }


        [JsonIgnore]
        public TaskDTO TDTO { get { return new TaskDTO(this.TaskID, this.TaskName, this.TaskStatusID, this.ListedBy, this.CompletedBy, this.ListedDate, this.CompletedDate); } }

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
            return new BLL_Task(TDTO);
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
            return this.TaskID > 0;
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
    }

}
