using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BLL
{
    public class BLL_Person : IBLL_Person
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int ID { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }

        [JsonPropertyName("pDTO")]
        [JsonIgnore]
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

        public BLL_Person(enMode mode, int iD, string fullName, DateTime birthDate, string email, string phone, string address)
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

        public static BLL_Person? Find(int ID)
        {
            PersonDTO? PDTO = DAL_Person.Find(ID);

            if (PDTO is not null)
                return new BLL_Person(PDTO, enMode.Update);

            return null;
        }

        private bool _AddNewPerson()
        {
            this.ID = DAL_Person.AddNewPerson(PDTO);

            return this.ID != -1;
        }
        private bool _UpdatePerson() => DAL_Person.UpdatePerson(PDTO);

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }

                    else
                        return false;
                case enMode.Update:
                    return _UpdatePerson();
            }
            return false;
        }
        
        public static bool DeletePerson(int ID)
        {

            return DAL_Person.DeletePerson(ID);
            //if (DAL_Person.IsExist(ID))
            //    return DAL_Person.DeletePerson(ID);

            //else
            //    return false;
        }

        public static List<PersonDTO> GetAll()
        {
            return DAL_Person.GetAll();
        }
    }
}
