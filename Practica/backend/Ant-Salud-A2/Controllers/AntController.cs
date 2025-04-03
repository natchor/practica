//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;

//namespace Ant_Salud_A2.Controllers
//{
//    [EnableCors]
//    [ApiController]
//    [Route("[controller]")]
//    public class AntController : ControllerBase
//    {
//        private readonly ApplicationDbContext _context;

//        public AntController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        [AllowAnonymous]
//        [HttpGet("fichaMedica")]
//        public async Task<IActionResult> GetFichaMedica()
//        {
//            var fichas = await _context.FichaMedica.ToListAsync();
//            return Ok(fichas);
//        }
//        [HttpGet("ping")]
//        public IActionResult Ping()
//        {
//            return Ok("pong");
//        }
//    }
//}