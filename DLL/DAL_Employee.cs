namespace DAL
{
    public class EmployeeDTO
    {
        public int employeeID { get; set; }
        public int personID { get; set; }
        public int titleID { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }

        public EmployeeDTO(int EmpID, int PersonID, int TitleID , DateTime StartDate, DateTime EndDate)
        {
            employeeID = EmpID;
            personID = PersonID;
            titleID = TitleID;
            startDate = StartDate;
            endDate = EndDate;
        }
    }

    class DAL_Employee
    {
    }

}
