using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Person
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string FullName { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public PersonDTO PDTO => new(this.ID, this.FullName, this.BirthDate, this.Email, this.Phone, this.Address);

        public BLL_Person(PersonDTO PDTO, enMode mode = enMode.AddNew)
        {
            this.ID = PDTO.ID;
            this.FullName = PDTO.FullName;
            this.BirthDate = PDTO.BirthDate;
            this.Email = PDTO.Email;
            this.Phone = PDTO.Phone;
            this.Address = PDTO.Address;
            this.Mode = mode;
        }
        public BLL_Person(enMode mode, int iD, string fullName, DateOnly birthDate, string email, string phone, string address)
        {
            Mode = mode;
            ID = iD;
            FullName = fullName;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
            Address = address;
        }
        public BLL_Person(PersonDTO PDTO)
        {
            this.ID = PDTO.ID;
            this.FullName = PDTO.FullName;
            this.BirthDate = PDTO.BirthDate;
            this.Email = PDTO.Email;
            this.Phone = PDTO.Phone;
            this.Address = PDTO.Address;
        }

        public BLL_Person FindPerson(int ID)
        {
            return new BLL_Person(PDTO);
        }


    }
}
