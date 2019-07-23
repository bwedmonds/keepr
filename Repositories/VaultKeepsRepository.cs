using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class VaultKeepsRepository
  {
    private readonly IDbConnection _db;

    public VaultKeepsRepository(IDbConnection db)
    {
      _db = db;
    }

    public IEnumerable<Keep> GetKeepsByVaultId(int vaultId, string userId)
    {
      return _db.Query<Keep>(@"
      SELECT* FROM vaultkeeps vk
      INNER JOIN keeps k ON k.id = vk.keepId
      WHERE(vaultId = @vaultId AND vk.userId = @userId)", new { vaultId, userId });
    }

    public VaultKeep AddKeepToVault(VaultKeep vaultkeep)
    {
      int id = _db.ExecuteScalar<int>(@"
      INSERT INTO vaultkeeps (vaultId, keepId, userId)
      VALUES (@VaultId, @KeepId, @UserId);
      SELECT LAST_INSERT_ID();", vaultkeep);
      vaultkeep.Id = id;
      return vaultkeep;
    }

    public string DeleteVaultKeep(VaultKeep value)
    {
      string query = "DELETE FROM vaultkeeps WHERE (vaultId = @VaultId AND keepId = @KeepId AND userId = @UserId)";
      int changedRows = _db.Execute(query, value);
      if (changedRows < 1) throw new Exception("Invalid Id");
      return "Successfully deleted vaultkeep";
    }
  }
}