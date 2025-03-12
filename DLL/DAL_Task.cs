using Microsoft.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace DAL;

public record TaskDTO(int ID, string TaskName, int TaskStatusID, int ListedBy, int? CompletedBy, DateTime ListedDate, DateTime? CompletedDate);

public class DAL_Task
{
    /// <summary>
    /// Finds a task by its ID.
    /// Assumes stored procedure 'sp_FindTask' returns:
    /// ID, TaskTitle, TaskStatusID, ListedBy, CompletedBy, ListedDate, CompletedDate.
    /// </summary>
    public static TaskDTO? Find(int taskID)
    {
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_GetTask", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@TaskID", taskID);

        connection.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            int id = reader.GetInt32(reader.GetOrdinal("ID"));
            string taskName = reader.GetString(reader.GetOrdinal("TaskTitle"));
            int taskStatusID = reader.GetInt32(reader.GetOrdinal("StatusID"));
            int listedBy = reader.GetInt32(reader.GetOrdinal("ListedBy"));
            int? completedBy = reader.IsDBNull(reader.GetOrdinal("CompletedBy"))
                ? (int?)null
                : reader.GetInt32(reader.GetOrdinal("CompletedBy"));
            DateTime listedDate = reader.GetDateTime(reader.GetOrdinal("ListedDate"));
            DateTime? completedDate = reader.IsDBNull(reader.GetOrdinal("CompletedDate"))
                ? (DateTime?)null
                : reader.GetDateTime(reader.GetOrdinal("CompletedDate"));

            return new TaskDTO(id, taskName, taskStatusID, listedBy, completedBy, listedDate, completedDate);
        }
        return null;
    }

    public static IEnumerable<TaskDTO> GetAllTasks()
    {
        var tasks = new List<TaskDTO>();
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_GetAllTasks", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        connection.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(reader.GetOrdinal("ID"));
            // Assuming the stored procedure returns TaskTitle which corresponds to TaskName in the DTO
            string taskName = reader.GetString(reader.GetOrdinal("TaskTitle"));
            int taskStatusID = reader.GetInt32(reader.GetOrdinal("StatusID"));
            int listedBy = reader.GetInt32(reader.GetOrdinal("ListedBy"));
            DateTime listedDate = reader.GetDateTime(reader.GetOrdinal("ListedDate"));

            // For listed tasks, these might not be present (set to null)
            int? completedBy = null;
            DateTime? completedDate = null;

            tasks.Add(new TaskDTO(id, taskName, taskStatusID, listedBy, completedBy, listedDate, completedDate));
        }
        return tasks;
    }

    public static IEnumerable<TaskDTO> GetAllTasksBySpecificStatus(int StatusID)
    {
        var tasks = new List<TaskDTO>();
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_GetAllTasksBySpecificStatus", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        cmd.Parameters.AddWithValue("@StatusID", StatusID);
        connection.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(reader.GetOrdinal("ID"));
            // Assuming the stored procedure returns TaskTitle which corresponds to TaskName in the DTO
            string taskName = reader.GetString(reader.GetOrdinal("TaskTitle"));
            int taskStatusID = reader.GetInt32(reader.GetOrdinal("StatusID"));
            int listedBy = reader.GetInt32(reader.GetOrdinal("ListedBy"));
            DateTime listedDate = reader.GetDateTime(reader.GetOrdinal("ListedDate"));

            // For listed tasks, these might not be present (set to null)
            int? completedBy = null;
            DateTime? completedDate = null;

            tasks.Add(new TaskDTO(id, taskName, taskStatusID, listedBy, completedBy, listedDate, completedDate));
        }
        return tasks;
    }


    /// <summary>
    /// Adds a new task.
    /// Assumes stored procedure 'sp_AddTask' requires:
    /// @TaskTitle, @StatusID, @ListedBy, @ListedDate.
    /// </summary>
    public static int AddTask(TaskDTO TDTO)
    {
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_AddTask", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@TaskTitle",  TDTO.TaskName);
        cmd.Parameters.AddWithValue("@StatusID",   TDTO.TaskStatusID);
        cmd.Parameters.AddWithValue("@ListedBy",   TDTO.ListedBy);


        // Define the output parameter for the new task ID
        var outputParam = new SqlParameter("@NewTaskID", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(outputParam);

        connection.Open();
        cmd.ExecuteNonQuery();
        return (int)outputParam.Value;
    }


    public static bool UpdateTask(TaskDTO TDTO)
    {
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_UpdateTask", connection)
        {
            CommandType = CommandType.StoredProcedure
        };

        // Add input parameters
        cmd.Parameters.AddWithValue("@TaskID", TDTO.ID);
        cmd.Parameters.AddWithValue("@StatusID", TDTO.TaskStatusID);
        cmd.Parameters.AddWithValue("@TaskTitle", TDTO.TaskName);

        // Handle optional CompletedBy and CompletedDate parameters
        if (TDTO.CompletedBy.HasValue)
        {
            cmd.Parameters.AddWithValue("@CompletedBy", TDTO.CompletedBy.Value);
            cmd.Parameters.AddWithValue("@CompletedDate", (object)TDTO.CompletedBy ?? DBNull.Value);
        }
        else
        {
            cmd.Parameters.AddWithValue("@CompletedBy", DBNull.Value);
            cmd.Parameters.AddWithValue("@CompletedDate", DBNull.Value);
        }

        // Define the output parameter
        var outputParam = new SqlParameter("@Updated", SqlDbType.Bit)
        {
            Direction = ParameterDirection.Output
        };
        cmd.Parameters.Add(outputParam);

        connection.Open();
        cmd.ExecuteNonQuery();

        // Check if the task was updated
        return (bool)outputParam.Value;
    }

    /// <summary>
    /// Completes an existing task.
    /// Assumes stored procedure 'sp_CompleteTask' requires:
    /// @TaskID, @NewStatusID, @CompletedBy, @CompletedDate.
    /// </summary>
    public static bool CompleteTask(TaskDTO TDTO)
    {
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_CompleteTask", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@TaskID", TDTO.ID);
        cmd.Parameters.AddWithValue("@CompletedBy", TDTO.CompletedBy);

        connection.Open();
        int affected = cmd.ExecuteNonQuery();
        return affected > 0;
    }

    /// <summary>
    /// Retrieves all tasks listed by a specific active employee.
    /// Assumes stored procedure 'sp_GetListedTasksByUser' returns:
    /// ID, TaskTitle, TaskStatusID, ListedBy, ListedDate.
    /// </summary>
    public static IEnumerable<TaskDTO> GetListedTasksByUser(int userID)
    {
        var tasks = new List<TaskDTO>();
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_GetListedTasksByUser", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@UserID", userID);

        connection.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(reader.GetOrdinal("ID"));
            // Assuming the stored procedure returns TaskTitle which corresponds to TaskName in the DTO
            string taskName = reader.GetString(reader.GetOrdinal("TaskTitle"));
            int taskStatusID = reader.GetInt32(reader.GetOrdinal("StatusID"));
            int listedBy = reader.GetInt32(reader.GetOrdinal("ListedBy"));
            DateTime listedDate = reader.GetDateTime(reader.GetOrdinal("ListedDate"));

            // For listed tasks, these might not be present (set to null)
            int? completedBy = null;
            DateTime? completedDate = null;

            tasks.Add(new TaskDTO(id, taskName, taskStatusID, listedBy, completedBy, listedDate, completedDate));
        }
        return tasks;
    }

    /// <summary>
    /// Retrieves all tasks completed by a specific active employee.
    /// Assumes stored procedure 'sp_GetCompletedTasksByUser' returns:
    /// ID, TaskTitle, TaskStatusID, CompletedBy, CompletedDate.
    /// </summary>
    public static IEnumerable<TaskDTO> GetCompletedTasksByUser(int userID)
    {
        var tasks = new List<TaskDTO>();
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_GetCompletedTasksByUser", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        cmd.Parameters.AddWithValue("@UserID", userID);

        connection.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int id = reader.GetInt32(reader.GetOrdinal("ID"));
            string taskName = reader.GetString(reader.GetOrdinal("TaskTitle"));
            int taskStatusID = reader.GetInt32(reader.GetOrdinal("StatusID"));
            // For completed tasks, the CompletedBy column must be provided by the SP.
            int completedBy = reader.GetInt32(reader.GetOrdinal("CompletedBy"));
            DateTime completedDate = reader.GetDateTime(reader.GetOrdinal("CompletedDate"));

            // In a completed task, ListedBy and ListedDate may or may not be returned.
            // You can choose to fill them with defaults or adjust the DTO/SP as needed.
            int listedBy = 0;
            DateTime listedDate = DateTime.MinValue;

            tasks.Add(new TaskDTO(id, taskName, taskStatusID, listedBy, completedBy, listedDate, completedDate));
        }
        return tasks;
    }
}

