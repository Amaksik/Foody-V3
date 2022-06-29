using BAL.Clients;
using BAL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foody_V3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MealsController : Controller
    {
        private static IRecognitionService _recognitionService;
        public MealsController(IRecognitionService dataService)
        {
            _recognitionService = dataService;
        }


        //meal recognition without proper user info
        [HttpPost("/recognize")]
        public IActionResult Upload()
        {
            PhotoHandling ph = new PhotoHandling();
            try
            {
                var file = Request.Form.Files[0];

                var message = ph.FileUpload(file).Result;
                if (message != "notOk")
                {
                    return Ok(message);
                }
                else
                {
                    return BadRequest();
                }



            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }


        }


        //meal info by name
        [HttpGet("/100g/{name}")]
        public async Task<IActionResult> Consume100Info([FromQuery] string name)
        {
            if (name != null)
            {

                try
                {
                    var result = await _recognitionService.Get100gInfo(name);
                    return Ok(result);
                }
                catch (Exception)
                {

                    return BadRequest("couldn't recognize it");
                }
            }
            else
            {
                return BadRequest("info not provided");
            }

        }


        //consuming 
        [HttpPost("/natural/{query}")]
        public async Task<IActionResult> NaturalInfo(string query)
        {
            if (query != null)
            {
                try
                {
                    var result = await _recognitionService.NaturalInfo(query);
                    return Ok(result);
                }
                catch
                {

                    return BadRequest("couldn't recognize it");
                }



            }
            else
            {
                return BadRequest("no info provided");
            }

        }



        //info by barcode
        [HttpGet("/barcode")]
        public async Task<IActionResult> BarcodeInfo([FromQuery] string barcode)
        {
            if (barcode != null)
            {
                var result = await _recognitionService.BarcodeInfo(barcode);
                return Ok(result);

            }
            else
            {
                return BadRequest("barcode not provided");
            }

        }

    }

}
