using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebGameController.Models;
using GameMaster;

namespace WebGameController.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<GameController> _logger;

        private MainGame game;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
            game = new MainGame();
        }

        public IActionResult Index()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
