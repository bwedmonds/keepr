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
  public class VaultsController : ControllerBase
  {
    private readonly VaultsRepository _repo;

    public VaultsController(VaultsRepository repo)
    {
      _repo = repo;
    }

    //GetUserVaults
    [Authorize]
    [HttpGet]
    public ActionResult<IEnumerable<Vault>> Get()
    {
      string userId = HttpContext.User.FindFirstValue("Id");
      IEnumerable<Vault> userVaults = _repo.GetUserVaults(userId);
      try
      {
        return Ok(userVaults);
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    [Authorize]
    [HttpGet("{id}")]
    public ActionResult<Vault> GetVaultById(int id)
    {
      string userId = HttpContext.User.FindFirstValue("Id");
      try
      {
        return Ok(_repo.GetByVaultId(id, userId));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    [Authorize]
    [HttpPost]
    public ActionResult<Vault> Post([FromBody] Vault vault)
    {
      vault.UserId = HttpContext.User.FindFirstValue("Id");
      try
      {
        return Ok(_repo.Create(vault));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }


    [HttpDelete("{id}")]
    public ActionResult<string> Delete(int id)
    {
      string userId = HttpContext.User.FindFirstValue("Id");
      try
      {
        return Ok(_repo.Delete(id, userId));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

  }
}