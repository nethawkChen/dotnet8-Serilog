using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Serilog.LogBranch.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase {
        private readonly ILogger<HelloController> _logger;

        public HelloController(ILogger<HelloController> logger) {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Say(string value) {
            _logger.LogInformation($"輸入的值：{value}");
            return Ok(new { Id = 1, Value = value });
        }
    }
}
