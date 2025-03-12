using Microsoft.Data.SqlClient;
using System.Data;

namespace DAL
{
    public record EmployeeDTO(int EmpID, int PersonID, int TitleID, decimal salary, int departmentID, DateTime StartDate, DateTime EndDate);

    public class DAL_Employee
    {
        public static EmployeeDTO? Find(int ID)
        {

            using var connection = new ConnectionToTheDB().GetConnection();
            using var cmd = new SqlCommand("sp_GetEmployee", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", ID);

            connection.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new EmployeeDTO
                    (
                        reader.GetInt32(reader.GetOrdinal("ID")),
                        reader.GetInt32(reader.GetOrdinal("PersonID")),
                        reader.GetInt32(reader.GetOrdinal("TitleID")),
                        reader.GetDecimal(reader.GetOrdinal("Salary")),
                        reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                        reader.GetDateTime(reader.GetOrdinal("StartDate")),
                        reader.GetDateTime(reader.GetOrdinal("EndDate"))

                    );
            }
            else
                return null;

        }

        public static List<EmployeeDTO> GetAll()
        {
            var Employees = new List<EmployeeDTO>();
            using (SqlConnection conn = new ConnectionToTheDB().GetConnection())
            {
                using SqlCommand cmd = new("sp_AllEmployeeData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Employees.Add
                    (
                        new EmployeeDTO
                        (
                            reader.GetInt32(reader.GetOrdinal("ID")),
                            reader.GetInt32(reader.GetOrdinal("PersonID")),
                            reader.GetInt32(reader.GetOrdinal("TitleID")),
                            reader.GetDecimal(reader.GetOrdinal("Salary")),
                            reader.GetInt32(reader.GetOrdinal("DepartmentID")),
                            reader.GetDateTime(reader.GetOrdinal("StartDate")),
                            reader.GetDateTime(reader.GetOrdinal("EndDate"))
                        )
                    );
                }
            }
            return Employees;
        }

        public static int AddNewEmployee(EmployeeDTO EDTO)
        {
            using var connection = new ConnectionToTheDB().GetConnection();
            using var cmd = new SqlCommand("sp_AddEmployee", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@PersonID", EDTO.PersonID);
            cmd.Parameters.AddWithValue("@TitleID", EDTO.TitleID);
            cmd.Parameters.AddWithValue("@Salary", EDTO.salary);
            cmd.Parameters.AddWithValue("@DepartmentID", EDTO.departmentID);
            cmd.Parameters.AddWithValue("@StartDate", EDTO.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EDTO.EndDate);

            var output = new SqlParameter("@EmployeeID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };

            cmd.Parameters.Add(output);

            connection.Open();
            cmd.ExecuteNonQuery();

            return (int)output.Value;
        }

        public static bool UpdateEmployee(EmployeeDTO EDTO)
        {
            using var connection = new ConnectionToTheDB().GetConnection();
            using var cmd = new SqlCommand("sp_UpdateEmployee", connection);
            cmd.CommandType = CommandType.StoredProcedure;
   
            cmd.Parameters.AddWithValue("@EmployeeID", EDTO.EmpID);
            cmd.Parameters.AddWithValue("@TitleID", EDTO.TitleID);
            cmd.Parameters.AddWithValue("@Salary", EDTO.salary);
            cmd.Parameters.AddWithValue("@DepartmentID", EDTO.departmentID);
            cmd.Parameters.AddWithValue("@StartDate", EDTO.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EDTO.EndDate);    
            try
            {
                connection.Open();
                return cmd.ExecuteNonQuery() > 0;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool DeleteEmployee(int ID)
        {
            using var connection = new ConnectionToTheDB().GetConnection();
            using var cmd = new SqlCommand("sp_DeleteEmployee", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeID", ID);
            try
            {
                connection.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

}
