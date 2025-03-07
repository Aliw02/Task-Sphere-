using DAL;


namespace BLL
{
    public class BLL_Employee(EmployeeDTO employeeDTO, BLL_Employee.enMode mode = BLL_Employee.enMode.AddNew)
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = mode;

        public int EmployeeID { get; set; } = employeeDTO.employeeID;
        public int PersonID { get; set; } = employeeDTO.personID;
        public int TitleID { get; set; } = employeeDTO.titleID;
        public BLL_Person? EmployeeDetails { get; }
        public DateTime StartDate { get; set; } = employeeDTO.startDate;
        public DateTime EndDate { get; set; } = employeeDTO.endDate;
        public EmployeeDTO EDTO { get { return new EmployeeDTO(this.EmployeeID, this.PersonID, this.TitleID, this.StartDate, this.EndDate); } }






    }
}
