using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Serilog.LogBranch.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger) {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Say(string value) {
            _logger.LogInformation($"輸入的值：{value}");
            return Ok(new {Id=1, Value=value});
        }
    }
}
