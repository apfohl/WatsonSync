using MonadicBits;
using WatsonSync.Models;

namespace WatsonSync.Components.DataAccess;

public interface IDatabase
{
    UnitOfWork StartUnitOfWork();
    Task<Maybe<User>> FindUserByToken(string token);
}