using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class VaultsRepository
  {
    private readonly IDbConnection _db;
    public VaultsRepository(IDbConnection db)
    {
      _db = db;
    }

    public IEnumerable<Vault> GetUserVaults(string userId)
    {
      return _db.Query<Vault>("SELECT * FROM vaults WHERE userId = @userId;", new { userId });
    }

    public Vault GetByVaultId(int id, string userId)
    {

      return _db.QueryFirstOrDefault<Vault>("SELECT * FROM vaults WHERE id = @id AND userId = @userId", new { id, userId });
    }

    public Vault Create(Vault vault)
    {
      int id = _db.ExecuteScalar<int>(@"
            INSERT INTO vaults (name, description, userId)
            VALUES (@Name, @Description, @UserId);
            SELECT LAST_INSERT_ID();
            ", vault);
      vault.Id = id;
      return vault;
    }

    public object Delete(int id, string userId)
    {
      string query = "DELETE FROM vaults WHERE id = @Id AND userId = @userId";
      int changedRows = _db.Execute(query, new { id, userId });
      if (changedRows < 1) throw new Exception("Invalid Id");
      return "Successfully Deleted Vault";
    }
  }
}