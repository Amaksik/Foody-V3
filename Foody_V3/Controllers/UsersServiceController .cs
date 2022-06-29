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
    public class UsersServiceController : Controller
    {
        private static IUserService _userService;
        public UsersServiceController(IUserService userService)
        {
            _userService = userService;
        }


        //Getting Statistics of user per days
        [HttpGet("users/{user_id}/statistics/{days}")]
        public async Task<IActionResult> GetStatistics(string user_id, int days)
        {
            if (user_id != null)
            {
                var findet = await _userService.GetUser(Convert.ToInt32(user_id));
                if (findet != null && findet.Favourite.Count > 0)
                {
                    List<DayIntake> result = new List<DayIntake>();

                    foreach (var item in findet.Statistics)
                    {
                        result.Add(item);
                    }
                    if (days > result.Count)
                    {
                        return Ok(result);
                    }
                    else
                    {
                        List<DayIntake> resulttosend = new List<DayIntake>();
                        for (int i = 0; i < days; i++)
                        {
                            resulttosend.Add(result[0]);
                        }
                        return Ok(JsonSerializer.Serialize(resulttosend));

                    }


                }
                else if (findet != null && findet.Favourite.Count == 0)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("user not found");
                }
            }
            else
            {
                return BadRequest("no id");
            }

        }


        //getting list of Favourite users products
        [HttpGet("users/{user_id}/favourite")]
        public async Task<IActionResult> Favourite(string user_id)
        {
            if (user_id != null)
            {
                var findet = await _userService.GetUser(Convert.ToInt32(user_id));
                if (findet != null && findet.Favourite.Count > 0)
                {
                    List<Product> result = new List<Product>();

                    foreach (var item in findet.Favourite)
                    {
                        result.Add(item);
                    }

                    return Ok(JsonSerializer.Serialize(result));
                }
                else if (findet != null && findet.Favourite.Count == 0)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("user not found");
                }
            }
            else
            {
                return BadRequest("no id provided");
            }
        }


        //adding product to list of Favourite users products
        [HttpPost("users/{user_id}/favourite")]
        public async Task<IActionResult> ProductAdd(string user_id, [FromBody] Product prdct)
        {
            if (user_id != null && !(prdct is null))
            {
                await _userService.AddProduct(Convert.ToInt32(user_id), prdct);
                return Ok("product has been added");
            }
            else
            {
                return BadRequest();
            }

        }


        //removing product from list of Favourite users products
        [HttpDelete("users/{user_id}/favourite")]
        public async Task<IActionResult> ProductRemove(string user_id, [FromBody] Product prdct)
        {
            if (user_id != null && !(prdct is null))
            {
                await _userService.RemoveProduct(Convert.ToInt32(user_id), prdct);
                return Ok("product has been added");
            }
            else
            {
                return BadRequest();
            }
        }



    }

}
