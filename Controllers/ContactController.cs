using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ContactsCoreMVC.Models.Abstract;
using ContactsCoreMVC.Models.Entities;
using ContactsCoreMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ContactsCoreMVC.Controllers
{
  public class ContactController : Controller
  {
    private readonly IContactRepository _contactRepository;
    private readonly ILogger<ContactController> _logger;

    public ContactController(IContactRepository contactRepository, ILogger<ContactController> logger)
    {
      _contactRepository = contactRepository;
      _logger = logger;
    }

    // public async Task<IActionResult> Index()
    // {
    //   var contactTableList = await _contactRepository.GetAllContactsAsync();
    //   List<Contact> contactList = new List<Contact>();
    //   foreach (var item in contactTableList)
    //   {
    //     contactList.Add(new Contact
    //     {
    //       ContactName = item.PartitionKey,
    //       Phone = item.RowKey,
    //       ContactType = item.ContactType,
    //       Email = item.Email
    //     });
    //   }
    //   return View(contactList);
    // }

    public async Task<IActionResult> Index(string contactName = null, string phone = null)
    {
      List<ContactTable> contactTableList = new List<ContactTable>();
      if (string.IsNullOrEmpty(contactName) && string.IsNullOrEmpty(phone))
      {
        contactTableList = await _contactRepository.GetAllContactsAsync();
      }
      else if (!string.IsNullOrEmpty(contactName) && !string.IsNullOrEmpty(phone))
      {
        var contact = await _contactRepository.FindContactCPAsync(contactName, phone);
        contactTableList.AddRange(contact);
      }

      else if (!string.IsNullOrEmpty(contactName) && string.IsNullOrEmpty(phone))
      {
        contactTableList = await _contactRepository.FindContactsByContactNameAsync(contactName);
      }
      else if (string.IsNullOrEmpty(contactName) && !string.IsNullOrEmpty(phone))
      {
        contactTableList = await _contactRepository.FindContactByPhoneAsync(phone);
        // var contactTable = await _contactRepository.FindContactByRowKeyAsync(phone);
        // contactTableList.Add(contactTable);
      }
      List<Contact> contactList = new List<Contact>();
      foreach (var item in contactTableList)
      {
        contactList.Add(new Contact
        {
          Id = item.Id,
          ContactName = item.ContactName,
          Phone = item.Phone,
          ContactType = item.ContactType,
          Email = item.Email
        });
      }
      return View(contactList);
    }


    public IActionResult Create()
    {
      return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Contact contact)
    {
      contact.Id = Guid.NewGuid().ToString();
      if (ModelState.IsValid)
      {
        ContactTable contactTable = new ContactTable { Id = contact.Id, ContactName = contact.ContactName, Phone = contact.Phone, Email = contact.Email, ContactType = contact.ContactType };
        var contactResult = await _contactRepository.CreateAsync(contactTable);
        if (contactResult != null)
        {
          return RedirectToAction("Index");
        }
        else
        {
          ModelState.AddModelError("", "New Contact Could not be created");
        }
      }
      return View();
    }

    public async Task<IActionResult> Edit(string id, string contactName)
    {
      var contactTable = await _contactRepository.FindContactAsync(id);

      var contact = new Contact { Id = contactTable.Id, ContactName = contactTable.ContactName, Phone = contactTable.Phone, ContactType = contactTable.ContactType, Email = contactTable.Email };
      return View(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Contact contact)
    {
      if (ModelState.IsValid)
      {
        var contactTable = new ContactTable { Id = contact.Id, ContactName = contact.ContactName, Phone = contact.Phone, ContactType = contact.ContactType, Email = contact.Email };
        var updateContact = await _contactRepository.UpdateAsync(contactTable);
        if (updateContact != null)
        {
          return RedirectToAction("Index");
        }
      }
      return View(contact);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id, string contactName)
    {
      await _contactRepository.DeleteAsync(id, contactName);
      return RedirectToAction("Index");
    }
  }
}