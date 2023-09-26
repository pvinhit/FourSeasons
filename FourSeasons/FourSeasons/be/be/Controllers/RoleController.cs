using be.Helpers;
using be.Models;
using be.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.WebSockets;
using System.Xml.Linq;

namespace be.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("all")]
        public async Task<ActionResult> GetAllRole()
        {
            var data = _roleService.GetAllRoles();
            return Ok(new
            {
                status = 200,
                data
            });
        }
        [HttpGet]
        public async Task<ActionResult> GetRole(int id)
        {
            var role = _roleService.GetRole(id);
            if (role == null)
            {
                return Ok(new
                {
                    status = 404,
                    message = "Role is not found"
                });
            }
            return Ok(new
            {
                status = 200,
                message = "Get role success!",
                data = role

            });
        }
        [HttpPost("add")]
        public async Task<ActionResult> AddRole([FromBody]Role role)
        {
            try
            {
                _roleService.AddRole(role);
                return Ok(new
                {
                    status = 200,
                    message = "Add role success"
                });
            }
            catch
            {
                return BadRequest();
            }
            
        }
        [HttpDelete("delete")]
        public async Task<ActionResult> DeleteRole(int id)
        {
            try
            {
                _roleService.DeleteRole(id);
                return Ok(new
                {
                    status = 200,
                    message = "Delete role success"
                });
            }
            catch
            {
                return BadRequest();
            }

        }
    }
}
