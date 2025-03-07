using Microsoft.Data.SqlClient;
using System.Data;


namespace DAL;


public class PersonDTO(int id, string fullName, DateOnly birthDate, string email, string phone, string address)
{
    public int ID { get; set; } = id;
    public string FullName { get; set; } = fullName;
    public DateOnly BirthDate { get; set; } = birthDate;
    public string Email { get; set; } = email;
    public string Phone { get; set; } = phone;
    public string Address { get; set; } = address;
}
public class DAL_Person
{


    public static List<(int Id, string Title)> GetAllEmployeeTitles()
    {
        var EmployeesTitles = new List<(int Id, string Title)>();

        using(SqlConnection conn = new ConnectionToTheDB().GetConnection())
        {
            using SqlCommand cmd = new("sp_GetAllEmployeesTitles", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.CommandText = "SELECT * FROM EmployeeTitles";

            conn.Open();

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                EmployeesTitles.Add
                (
                    (
                        reader.GetInt32(reader.GetOrdinal("ID")),   // Correctly getting ID
                        reader.GetString(reader.GetOrdinal("Title")) // Correctly getting Title
                    )
                );
            }
        }


        return EmployeesTitles;
    }
    

}
