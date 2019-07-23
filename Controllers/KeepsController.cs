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
  public class KeepsController : ControllerBase
  {
    private readonly KeepsRepository _repo;
    public KeepsController(KeepsRepository repo)
    {
      _repo = repo;
    }
    // GET api/values
    [HttpGet]
    public ActionResult<IEnumerable<Keep>> Get()
    {
      try
      {
        return Ok(_repo.GetPublicKeeps());
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    //GetKeepsByUser
    [Authorize]
    [HttpGet("user")]
    public ActionResult<IEnumerable<Keep>> GetByUser()
    {
      string userId = HttpContext.User.FindFirstValue("Id");
      IEnumerable<Keep> userKeeps = _repo.GetKeepsByUser(userId);
      try
      {
        return Ok(userKeeps);
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }
    [HttpGet("{id}")]
    public ActionResult<Keep> GetKeepById(int id)
    {
      try
      {
        return Ok(_repo.GetByKeepId(id));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    [Authorize]
    [HttpPost]
    public ActionResult<Keep> Post([FromBody] Keep value)
    {
      try
      {
        string userId = HttpContext.User.FindFirstValue("Id");
        value.UserId = userId;
        return Ok(_repo.Create(value));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    [Authorize]
    [HttpPut("{id}")]
    public ActionResult<Keep> Put(int id, [FromBody] Keep value)
    {
      try
      {
        value.Id = id;
        return Ok(_repo.UpdateKeep(id, value));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }

    [Authorize]
    [HttpDelete("{keepId}")]
    public ActionResult<String> Delete(int keepId)
    {
      string userId = HttpContext.User.FindFirstValue("Id");
      try
      {
        return Ok(_repo.Delete(keepId, userId));
      }
      catch (Exception e)
      {
        return BadRequest(e);
      }
    }
  }
}