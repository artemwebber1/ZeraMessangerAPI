using Microsoft.AspNetCore.Mvc;
using SoftworkMessanger.Models;

namespace SoftworkMessanger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatsController : ControllerBase
    {
        [HttpGet("{chatId:int}")]
        public Chat? GetChat(int chatId)
        {
            return null;
        }
    }
}
