using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        private readonly UserManager<UserModel> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(IConfiguration configuration, UserManager<UserModel> userManager, RoleManager<IdentityRole> roleManager)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UsersDto>> GetUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not Found"
                });

            }

            var result = new UsersDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount
            };

            return Ok(result);

        }
        // api/user/
        [HttpGet]
        public async Task<ActionResult<List<UsersDto>>> GetUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UsersDto>();
            foreach (var user in users)
            {
                var userDto = new UsersDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Roles = [.. await _userManager.GetRolesAsync(user)],
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                    AccessFailedCount = user.AccessFailedCount
                };
                result.Add(userDto);
            }
            return Ok(result);
        }

    }
}