using DAL;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;


namespace BLL
{
    public class BLL_Employee(EmployeeDTO employeeDTO, BLL_Employee.enMode mode = BLL_Employee.enMode.AddNew) : IEqualityComparer<BLL_Employee>
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = mode;
        public List<TaskDTO> TaskListedByThisUser { get; set; } = [];
        public int EmployeeID { get; set; } = employeeDTO.EmpID;
        public int PersonID { get; set ; } = employeeDTO.PersonID;
        public int TitleID { get; set; } = employeeDTO.TitleID;
#pragma warning disable CS8618 
        public BLL_Person? EmployeeDetails { get; private set; } = BLL_Person.Find(employeeDTO.PersonID);
#pragma warning restore CS8618 
        public DateTime StartDate { get; set; } = employeeDTO.StartDate;
        public DateTime EndDate { get; set; } = employeeDTO.EndDate;
        public decimal Salary { get; set; } = employeeDTO.salary;
        public int DepartmentID { get; set; } = employeeDTO.departmentID;

        [JsonIgnore]
        [JsonPropertyName("eDTO")]
        public EmployeeDTO EDTO { get { return new EmployeeDTO(this.EmployeeID, this.PersonID, this.TitleID, this.Salary, this.DepartmentID, this.StartDate, this.EndDate); } }

        public override bool Equals(object? obj)
        {
            if (obj is not BLL_Employee employee)
            {
                return false;
            }

            return  this.TitleID == employee.TitleID &&
                    this.Salary == employee.Salary &&
                    this.DepartmentID == employee.DepartmentID;
        }


        public override int GetHashCode()
        {
            return HashCode.Combine(this.DepartmentID * 7 , (this.DepartmentID * 7 + this.DepartmentID));            
        }

        public override string? ToString()
        {
            if(EmployeeDetails is not null)
            {
                return $"{this.EmployeeID}- {EmployeeDetails.FullName}, {EmployeeDetails.BirthDate.ToShortDateString()}, {Salary:N2}";
            }

            return $"{this.EmployeeID}- {Salary :N2}";
        }

        public bool Equals(BLL_Employee? x, BLL_Employee? y)
        {
            if(x is null && y is null)
            {
                return true;
            }
            else if (x is null || y is null)
            {
                return false;
            }

            return x.Equals(y);
        }

        public int GetHashCode([DisallowNull] BLL_Employee obj)
        {
            return this.GetHashCode();
        }


        public static BLL_Employee? Find(int id)
        {
            EmployeeDTO? EDTO = DAL_Employee.Find(id);

            if (EDTO is not null)
            {
                return new BLL_Employee(EDTO, enMode.Update);
            }
            return null;
        }

        public static List<EmployeeDTO> GetAll()
        {
            return DAL_Employee.GetAll();
        }

        private bool _AddNewEmployee()
        {
            this.EmployeeID = DAL_Employee.AddNewEmployee(this.EDTO);
            return this.EmployeeID > 0;
        }

        private bool _UpdateEmployee()
        {
            return DAL_Employee.UpdateEmployee(this.EDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewEmployee())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateEmployee();
            }
            return false;
        }

        public static bool Delete(int ID)
        {
            return DAL_Employee.DeleteEmployee(ID);
        }

    }
}
