using Newtonsoft.Json;

namespace ContactsCoreMVC.Models.Entities
{
  public class ContactTable
  {
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    public string ContactName { get; set; }
    public string Phone { get; set; }
    public string ContactType { get; set; }
    public string Email { get; set; }
  }
}