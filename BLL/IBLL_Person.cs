using DAL;

namespace BLL
{
    public interface IBLL_Person
    {
        string Address { get; set; }
        DateTime BirthDate { get; set; }
        string Email { get; set; }
        string FullName { get; set; }
        int ID { get; set; }
        PersonDTO PDTO { get; }
        string Phone { get; set; }

        bool Save();
    }
}