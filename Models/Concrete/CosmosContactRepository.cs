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
      ItemResponse<ContactTable> contactResponse = await 
      _container.CreateItemAsync<ContactTable>(contactTable);
      if(contactResponse.StatusCode == HttpStatusCode.Created){
        return contactTable;
      }
      return null;
    }

    public async Task<ContactTable> FindContactAsync(string id)
    {
      var sqlQuery = $"select * from c where c.id = '{id}'";
      QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
      FeedIterator<ContactTable> queryResulterator = _container.GetItemQueryIterator<ContactTable>(queryDefinition); 

      List<ContactTable> contactTableList = new List<ContactTable>();
      while(queryResulterator.HasMoreResults){
          FeedResponse<ContactTable> currentResult = await queryResulterator.ReadNextAsync();
          foreach(var item in currentResult){
            contactTableList.Add(item);

          }
          return contactTableList[0];
      }      
      return null;
    }
    public async Task<List<ContactTable>> FindContactCPAsync(string contactName, string phone)
    {
      var sqlQuery = $"select * from c where c.ContactName = '{contactName}' and c.Phone='{phone}'";
      QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
      FeedIterator<ContactTable> queryResulterator = _container.GetItemQueryIterator<ContactTable>(queryDefinition); 

      List<ContactTable> contactTableList = new List<ContactTable>();
      while(queryResulterator.HasMoreResults){
          FeedResponse<ContactTable> currentResult = await queryResulterator.ReadNextAsync();
          foreach(var item in currentResult){
            contactTableList.Add(item);

          }
          return contactTableList;
      }      
      return null;
    }

    public async Task<List<ContactTable>> FindContactByPhoneAsync(string phone)
    {
       var sqlQuery = $"select * from c where c.Phone = '{phone}'";
      QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
      FeedIterator<ContactTable> queryResulterator = _container.GetItemQueryIterator<ContactTable>(queryDefinition); 

      List<ContactTable> contactTableList = new List<ContactTable>();
      while(queryResulterator.HasMoreResults){
          FeedResponse<ContactTable> currentResult = await queryResulterator.ReadNextAsync();
          foreach(var item in currentResult){
            contactTableList.Add(item);

          }
          return contactTableList;
      }      
      return null;
    }

    public async Task<List<ContactTable>> FindContactsByContactNameAsync(string contactName)
    {
       var sqlQuery = $"select * from c where c.ContactName = '{contactName}'";
      QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
      FeedIterator<ContactTable> queryResulterator = _container.GetItemQueryIterator<ContactTable>(queryDefinition); 

      List<ContactTable> contactTableList = new List<ContactTable>();
      while(queryResulterator.HasMoreResults){
          FeedResponse<ContactTable> currentResult = await queryResulterator.ReadNextAsync();
          foreach(var item in currentResult){
            contactTableList.Add(item);

          }
          return contactTableList;
      }      
      return null;
    }

    public async Task<List<ContactTable>> GetAllContactsAsync()
    {
      var sqlQuery = $"select * from c ";
      QueryDefinition queryDefinition = new QueryDefinition(sqlQuery);
      FeedIterator<ContactTable> queryResulterator = _container.GetItemQueryIterator<ContactTable>(queryDefinition); 

      List<ContactTable> contactTableList = new List<ContactTable>();
      while(queryResulterator.HasMoreResults){
          FeedResponse<ContactTable> currentResult = await queryResulterator.ReadNextAsync();
          foreach(var item in currentResult){
            contactTableList.Add(item);

          }
          return contactTableList;
      }      
      return null;
    }

    public async Task<ContactTable> UpdateAsync(ContactTable contactTable)
    {

      ItemResponse<ContactTable> contactResponse = await 
      _container.ReadItemAsync<ContactTable>(contactTable.Id,new PartitionKey
      (contactTable.ContactName));
      var contactResult = contactResponse.Resource;
      contactResult.Id = contactTable.Id;
      contactResult.ContactName = contactTable.ContactName;
      contactResult.ContactType = contactTable.ContactType;
      contactResult.Email = contactTable.Email;

      contactResponse = await _container.ReplaceItemAsync<ContactTable>(contactResult,contactResult.Id);

      if(contactResponse.Resource != null){
        return contactResponse;
      }
      return null;
      
    }

    public async Task DeleteAsync(string id, string contactName)
    {      
      ItemResponse<ContactTable> contactTable = await _container.DeleteItemAsync<ContactTable>
      (id, new PartitionKey(contactName));

    }
  }
}