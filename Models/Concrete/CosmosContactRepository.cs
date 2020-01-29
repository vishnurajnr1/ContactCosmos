using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using ContactsCoreMVC.Models.Abstract;
using ContactsCoreMVC.Models.Entities;
using System.Collections.Generic;
using System.Net;

namespace ContactsCoreMVC.Models.Concrete
{
  public class CosmosContactRepository : IContactRepository
  {
    private readonly string _cosmosEndpoint;
    private readonly string _cosmosKey;
    private readonly string _databaseId;
    private readonly string _containerId;
    private Database _database;
    private Container _container;
    private CosmosClient _cosmosClient;

    public CosmosContactRepository(IOptions<CosmosUtility> cosmosUtility)
    {
      _cosmosEndpoint = cosmosUtility.Value.CosmosEndpoint;
      _cosmosKey = cosmosUtility.Value.CosmosKey;
      _databaseId = "multiDb";
      _containerId = "contact";

      _cosmosClient = new CosmosClient(_cosmosEndpoint, _cosmosKey);
      _cosmosClient.CreateDatabaseIfNotExistsAsync(_databaseId).GetAwaiter().GetResult();
      _database = _cosmosClient.GetDatabase(_databaseId);
      _database.CreateContainerIfNotExistsAsync(_containerId, "/ContactName").GetAwaiter().GetResult();
      _container = _database.GetContainer(_containerId);
    }

    public async Task<ContactTable> CreateAsync(ContactTable contactTable)
    {
      return null;
    }

    public async Task<ContactTable> FindContactAsync(string id)
    {
      return null;
    }
    public async Task<List<ContactTable>> FindContactCPAsync(string contactName, string phone)
    {
      return null;
    }

    public async Task<List<ContactTable>> FindContactByPhoneAsync(string phone)
    {
      return null;
    }

    public async Task<List<ContactTable>> FindContactsByContactNameAsync(string contactName)
    {
      return null;
    }

    public async Task<List<ContactTable>> GetAllContactsAsync()
    {
      return null;
    }

    public async Task<ContactTable> UpdateAsync(ContactTable contactTable)
    {
      return null;
    }

    public async Task DeleteAsync(string id, string contactName)
    {
    }
  }
}