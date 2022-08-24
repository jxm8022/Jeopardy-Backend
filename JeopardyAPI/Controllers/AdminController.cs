using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;

namespace JeopardyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly IBusiness _bl;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IBusiness bl, ILogger<AdminController> logger)
    {
        _bl = bl;
        _logger = logger;
    }

    [HttpGet("GetAdmin/{username}/{password}")]
    public async Task<ActionResult<Admin>> GetAdmin(string username, string password)
    {
        Admin admin = await _bl.GetAdminAsync(username, password);
        if (admin != null)
        {
            return Ok(admin);
        }
        return NoContent();
    }

    [HttpGet("GetAllAdmins")]
    public async Task<ActionResult<Admin>> GetAllAdmin()
    {
        List<Admin> admin = await _bl.GetAllAdminAsync();
        if (admin != null)
        {
            return Ok(admin);
        }
        return NoContent();
    }

    [HttpPut("UpdateAdmin")]
    public async Task<ActionResult> Put(Admin admin)
    {
        if (admin.admin_id > 0 && admin.admin_name.Length > 0 && admin.admin_password.Length > 0 && admin.admin_access > 0)
        {
            await _bl.UpdateAdminAsync(admin);
            return Ok();
        }
        return NoContent();
    }

    [HttpPost("CreateAdmin")]
    public async Task<ActionResult> Post(Admin admin)
    {
        if (admin.admin_name.Length > 0 && admin.admin_password.Length > 0 && admin.admin_access > 0)
        {
            await _bl.CreateAdminAsync(admin);
            return Ok();
        }
        return NoContent();
    }

    [HttpDelete("DeleteAdmin/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        if (id > 0)
        {
            await _bl.DeleteAdminAsync(id);
            return Ok();
        }
        return NoContent();
    }
}