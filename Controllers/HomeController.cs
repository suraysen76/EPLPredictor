﻿using Microsoft.AspNetCore.Mvc;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;
using System.Diagnostics;

namespace SS1892.EPLPredictor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IPredictionService? _predictionService;
        public HomeController(ILogger<HomeController> logger, IPredictionService predictionService)
        {
            _logger = logger;
            _predictionService = predictionService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _predictionService.GetMemberPredictionStandings(AuthModel.UserId);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

    }
}