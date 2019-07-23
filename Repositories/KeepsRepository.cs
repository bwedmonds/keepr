using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using keepr.Models;

namespace keepr.Repositories
{
  public class KeepsRepository
  {
    private readonly IDbConnection _db;
    public KeepsRepository(IDbConnection db)
    {
      _db = db;
    }

    public IEnumerable<Keep> GetPublicKeeps()
    {
      return _db.Query<Keep>("SELECT * FROM keeps WHERE isPrivate = 0");
    }

    public IEnumerable<Keep> GetKeepsByUser(string userId)
    {
      return _db.Query<Keep>("SELECT * FROM keeps WHERE userId = @userId;", new { userId });
    }
    public Keep GetByKeepId(int id)
    {
      return _db.QueryFirstOrDefault<Keep>("SELECT * FROM keeps WHERE id = @id AND isPrivate = 0", new { id });
    }

    public Keep Create(Keep value)
    {
      string query = @"
            INSERT INTO keeps (name, description, userId, isPrivate, img)
                    VALUES (@Name, @Description, @UserId, @IsPrivate, @Img);
                    SELECT LAST_INSERT_ID();
                    ";
      int id = _db.ExecuteScalar<int>(query, value);
      value.Id = id;
      return value;
    }

    public Keep UpdateKeep(int id, Keep value)
    {
      string query = @"
      UPDATE keeps
      SET
      Id = @Id,
      name = @Name,
      description = @Description,
      img = @Img,
      isPrivate = @IsPrivate,
      views = @Views,
      shares = @Shares,
      keeps = @Keeps
      WHERE id = @Id;
      SELECT * FROM keeps WHERE id = @Id";
      return _db.QueryFirstOrDefault<Keep>(query, value);
    }

    public string Delete(int keepId, string userId)
    {
      string query = "DELETE FROM keeps WHERE id = @keepId AND userId = @userId";
      int changedRows = _db.Execute(query, new { keepId, userId });
      if (changedRows < 1) throw new Exception("Invalid Id");
      return "Successfully deleted keep";
    }

  }
}