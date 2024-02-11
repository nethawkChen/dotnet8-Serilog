using Microsoft.AspNetCore.Mvc;

namespace SerilogAspNetDemo.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Say(string value) {
            _logger.LogInformation($"輸入的資料：{value}");
            return Ok(new { Id = 1, Value = $"You say {value}" });
        }

        [HttpPost]
        public IActionResult GetUser(string agentid) {
            _logger.LogInformation($"Agent：{agentid}");
            return Ok(new { AgentId = agentid, Name = "Tester" });
        }
    }
}
