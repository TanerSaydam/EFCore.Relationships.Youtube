using System.Text.Json.Serialization;

namespace EFCore.Relationships.Youtube.Models;

public sealed class User
{
    public User()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => string.Join(" ", FirstName, LastName);
    public object Information => new
    {
        InfoId = UserInformationId,
        IdentityNumber = UserInformation?.IdentityNumber,
        FullAddress = UserInformation?.FullAddress,
    };

    [JsonIgnore]
    public Guid UserInformationId { get; set; }
    [JsonIgnore]
    public UserInformation? UserInformation { get; set; } //navigation property
}
public sealed class UserInformation
{
    public UserInformation()
    {
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string IdentityNumber { get; set; } = default!;
    public string FullAddress { get; set; } = default!;
}