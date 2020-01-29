using System.Collections.Generic;
using System.Threading.Tasks;
using ContactsCoreMVC.Models.Entities;

namespace ContactsCoreMVC.Models.Abstract
{
  public interface IContactRepository
  {
    Task<List<ContactTable>> GetAllContactsAsync();
    Task<List<ContactTable>> FindContactByPhoneAsync(string phone);
    Task<List<ContactTable>> FindContactsByContactNameAsync(string contactName);
    Task<ContactTable> FindContactAsync(string id);
    Task<List<ContactTable>> FindContactCPAsync(string contactName, string phone);
    Task<ContactTable> CreateAsync(ContactTable contactTable);
    Task<ContactTable> UpdateAsync(ContactTable contactTable);
    Task DeleteAsync(string id, string contactName);
  }
}