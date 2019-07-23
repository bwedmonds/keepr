using System;
using System.Collections.Generic;
using System.Security.Claims;
using keepr.Models;
using keepr.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace keepr.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class VaultKeepsController : ControllerBase
  {
    private readonly VaultKeepsRepository _repo;
    public VaultKeepsController(VaultKeepsRepository repo)
    {
      _repo = repo;
    }


    //GET keeps by vaultId
    [Authorize]
    [HttpGet("{vaultId}")]
    public ActionResult<IEnumerable<Keep>> GetVaultKeeps(int vaultId)
    {
      string UserId = HttpContext.User.FindFirstValue("Id");
      IEnumerable<Keep> VaultKeeps = _repo.GetKeepsByVaultId(vaultId, UserId);
      try
      {
        // if (VaultKeeps != null)
        {
          return Ok(VaultKeeps);
        }
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    [HttpPost]
    public ActionResult<VaultKeep> Post([FromBody] VaultKeep value)
    {
      value.UserId = HttpContext.User.FindFirstValue("Id");
      VaultKeep addedVaultKeep = _repo.AddKeepToVault(value);
      try
      {
        {
          return Ok(_repo.AddKeepToVault(value));
        }
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    // [Authorize]
    // [HttpPut("{vaultId}/{keepId}")]
    // public ActionResult<String> Put([FromBody]VaultKeep value)
    // {
    //   try
    //   {
    //     string userId = HttpContext.User.FindFirstValue("Id");
    //     int vaultId = value.VaultId;
    //     int keepId = value.KeepId;
    //     return Ok(_repo.DeleteVaultKeep(vaultId, keepId, userId));
    //   }
    //   catch (Exception e)
    //   {
    //     return BadRequest(e);
    //   }
    // }

    [Authorize]
    [HttpPut]
    public ActionResult<String> Put([FromBody]VaultKeep value)
    {
      try
      {
        string userId = HttpContext.User.FindFirstValue("Id");
        value.UserId = userId;
        return Ok(_repo.DeleteVaultKeep(value));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }
  }
}