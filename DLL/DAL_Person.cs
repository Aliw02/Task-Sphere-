using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json.Serialization;


namespace DAL;

public class PersonDTO(int id, string fullName, DateTime birthDate, string email, string phone, string address)
{
    
    public int ID { get; set; } = id;
    
    public string FullName { get; set; } = fullName;
    
    public DateTime BirthDate { get; set; } = birthDate;
    
    public string Email { get; set; } = email;
    
    public string Phone { get; set; } = phone;
    
    public string Address { get; set; } = address;
}

public class DAL_Person
{

    public static PersonDTO? Find(int ID)
    {
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_GetPerson", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PersonID", ID);

        connection.Open();
        using var reader = cmd.ExecuteReader();
        if (reader.Read())
        {
            return new PersonDTO
                (
                    reader.GetInt32(reader.GetOrdinal("ID")),
                    reader.GetString(reader.GetOrdinal("FullName")),
                    reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                    reader.GetString(reader.GetOrdinal("Email")),
                    reader.GetString(reader.GetOrdinal("Phone")),
                    reader.GetString(reader.GetOrdinal("Address"))

                );
        }
        else
            return null;
    }

    public static List<(int Id, string Title)> GetAllEmployeeTitles()
    {
        var EmployeesTitles = new List<(int Id, string Title)>();

        using (SqlConnection conn = new ConnectionToTheDB().GetConnection())
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

    public static List<PersonDTO> GetAll()
    {
        List<PersonDTO> People = [];
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_GetAllPeople", connection);

        cmd.CommandType = CommandType.StoredProcedure;

        connection.Open();
        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            People.Add
                (
                    new PersonDTO(
                        reader.GetInt32(reader.GetOrdinal("ID")),
                        reader.GetString(reader.GetOrdinal("FullName")),
                        reader.GetDateTime(reader.GetOrdinal("BirthDate")),
                        reader.GetString(reader.GetOrdinal("Email")),
                        reader.GetString(reader.GetOrdinal("Phone")),
                        reader.GetString(reader.GetOrdinal("Address"))
                    )
                );
        }

        return People;
    }

    public static int AddNewPerson(PersonDTO PDTO)
    {
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_AddPerson", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@FullName", PDTO.FullName);
        cmd.Parameters.AddWithValue("@BirthDate", PDTO.BirthDate);
        cmd.Parameters.AddWithValue("@Address", PDTO.Address);
        cmd.Parameters.AddWithValue("@Email", PDTO.Email);
        cmd.Parameters.AddWithValue("@Phone", PDTO.Phone);
        //cmd.Parameters.AddWithValue("@Phone", PDTO.Phone);

        var output = new SqlParameter("@PersonID", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };

        cmd.Parameters.Add(output);

        connection.Open();
        cmd.ExecuteNonQuery();

        return (int)output.Value;

    }

    public static bool UpdatePerson(PersonDTO PDTO)
    {
        using var connection = new ConnectionToTheDB().GetConnection();
        using var cmd = new SqlCommand("sp_UpdatePerson", connection);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PersonID", PDTO.ID);
        cmd.Parameters.AddWithValue("@FullName", PDTO.FullName);
        cmd.Parameters.AddWithValue("@BirthDate", PDTO.BirthDate);
        cmd.Parameters.AddWithValue("@Address", PDTO.Address);
        cmd.Parameters.AddWithValue("@Email", PDTO.Email);
        cmd.Parameters.AddWithValue("@Phone", PDTO.Phone);

        connection.Open();
        var res = cmd.ExecuteNonQuery();
        return res > 0;
    }

    public static bool IsExist(int ID)
    {

        using SqlConnection conn = new ConnectionToTheDB().GetConnection();

        using SqlCommand cmd = new("sp_PersonIsExist", conn);

        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PersonID", ID);

        // Declare a return parameter
        SqlParameter returnValue = new()
        {
            Direction = ParameterDirection.ReturnValue
        };
        cmd.Parameters.Add(returnValue);

        conn.Open();
        cmd.ExecuteNonQuery();
        int result = (int)returnValue.Value; // Get the return value

        return result == 1;
    }

    public static bool DeletePerson(int ID)
    {

        using SqlConnection conn = new ConnectionToTheDB().GetConnection();
        using SqlCommand cmd = new("sp_DeletePerson", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@PersonID", ID);

        try
        {
            conn.Open();
            cmd.ExecuteNonQuery();
            return true;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"\n\nError: {ex.Message}");
            return false;
        }
    }
}
