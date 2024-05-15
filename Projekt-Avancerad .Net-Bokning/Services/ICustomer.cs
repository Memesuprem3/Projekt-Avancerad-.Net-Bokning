using Projekt_Models;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public interface ICustomer
    {
        Task<IEnumerable<Customer>> GetAll(); //onödig?
        Task<IEnumerable<Appointment>> Add(int id);
        Task<IEnumerable<Appointment>> Update(int id);
        Task<IEnumerable<Appointment>> Delete(int id);



    }
}
