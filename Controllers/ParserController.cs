using System;
using Microsoft.AspNetCore.Mvc;


namespace HTMLReaderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    public class ParserController : ControllerBase
    {
        private IParser _parser;
        public ParserController(IParser parser) {
            _parser = parser;
        }

        [HttpGet]
        public IActionResult Get() {
            return Ok("Holi");
        }

        [HttpPost]
        public IActionResult Post([FromBody] string original)
        {
            if(!String.IsNullOrWhiteSpace(original))
            {
                string result = _parser.Parse(original);
                return Ok(result);
            }
            return Ok("");
        }
    }
}
