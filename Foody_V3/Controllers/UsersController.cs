using BAL.Interfaces;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Foody_V3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private static IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("users")]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            var result = await _userService.GetUsers();
            return result;
        }


        //getting user info by id
        [HttpGet("users/{user_id}")]
        public async Task<IActionResult> GetUser(string user_id)
        {
            if (user_id != null)
            {
                var id = Convert.ToInt32(user_id);
                var findet = await _userService.GetUser(id);
                if (findet != null)
                {
                    return Ok(JsonSerializer.Serialize(findet));
                }
                else
                {
                    return NotFound("user not found");
                }
            }
            else
            {
                return BadRequest("id not provided");
            }

        }

        ////adding users
        [HttpPost("users")]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            if (user.Id != null && user.Callories >= 300)
            {

                var id = Convert.ToInt32(user.Id);
                if (id == 0)
                {

                    var result = await _userService.GetUser(id);
                    return Ok("user has been added");
                }
                else
                {
                    return BadRequest("user already exist");
                }

            }
            else
            {
                return BadRequest("not enough info");
            }
        }

        //removing users
        [HttpDelete("users/{user_id}")]
        public async Task<IActionResult> RemoveUser(string user_id)
        {
            if (user_id != null)
            {
                var findet = await _userService.GetUser(Convert.ToInt32(user_id));
                if (findet != null)
                {
                    await _userService.RemoveUser(Convert.ToInt32(user_id));
                    return Ok("user has been removed");
                }
                else
                {
                    return NotFound("user not found");
                }
            }
            else
            {
                return BadRequest("id not provided");
            }

        }



        //adding info about eaten product
        [HttpPost("users/{user_id}/consume")]
        public async Task<IActionResult> Consume(string user_id, [FromBody] int _new)
        {
            var id = Convert.ToInt32(user_id);
            await _userService.Consume(id, _new);
            return Ok("consumed");

        }



    }

}
