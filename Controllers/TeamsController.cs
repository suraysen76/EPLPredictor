using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;
using SS1892.EPLPredictor.Services;
using SS1892.EPLPredictor.Utility;

namespace SS1892.EPLPredictor.Controllers
{

    
    public class TeamsController : Controller
    {
        private ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }
        public async Task<IActionResult> Index()
        {
            
            var teams = await _teamService.GetTeams();
            return View(teams);
        }

        public async Task<IActionResult> Standings()
        {
            var standings = await _teamService.GetStandings();
            return View(standings);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _teamService.GetStandingById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( StandingsModel model)
        {
            var teamStatModel = model.TeamsStats;
            var retModel= await _teamService.UpdateTeamStats(teamStatModel);
            return RedirectToAction("Standings");
        }

        public async Task<IActionResult> EditResult(int id)
        {
            ResultModel model = await _teamService.GetResultById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResult(ResultModel model)
        {
            var retModel= await _teamService.UpdateResult(model);
            return RedirectToAction("Index","Fixtures");
        }
        public async Task<IActionResult> ResetResult(int id)
        {
            ResultModel model = await _teamService.GetResultById(id);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetResult(ResultModel model)
        {
            var retModel = await _teamService.ResetResult(model);
            return RedirectToAction("Index", "Fixtures");
        }
        public async Task<IActionResult> GetStatsByTeam(int  id)
        {
            var retModel = await _teamService.GetStandingById(id);
            return View(retModel);
        }

    }
}
