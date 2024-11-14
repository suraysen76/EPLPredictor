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
        public async Task<IActionResult> Index(string type)
        {
          
            var teams = await _teamService.GetTeams(type);
            
            return View(teams);
        }

        

        public async Task<IActionResult> Standings(string type)
        {
            var standings = await _teamService.GetStandings(type);
            return View(standings);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _teamService.GetTeamById(id);
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
            return RedirectToAction("Index", "Fixtures", new { type = retModel.Type }) ;
        }

        public async Task<IActionResult> Create()
        {           
            return View();
        }

        // POST: Fixtures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TeamModel teamModel)
        {
            var response = await _teamService.AddTeam(teamModel);

            ViewBag.Message = response.Message;
            if (response.Status)
            {
                return RedirectToAction("Index", new {type=teamModel.Type});
            }
            
            return View();

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
            return RedirectToAction("Index", "Fixtures", new {type=retModel.Type });
        }
        public async Task<IActionResult> GetStatsByTeam(int  id)
        {
            var retModel = await _teamService.GetStandingById(id);
            return View(retModel);
        }
        public async Task<IActionResult> EditTeamStats(int teamId)
        {
            var retModel = await _teamService.EditTeamStats(teamId);
            var tmodel= await _teamService.GetTeamById(teamId);
            ViewBag.Team = tmodel.Team;
            ViewBag.Type = tmodel.Type;
            return View(retModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditTeamStats(TeamStatModel model)
        {
            
            var tmodel = await _teamService.GetTeamById(model.TeamId);
            ViewBag.Team = tmodel.Team;
            ViewBag.Type = tmodel.Type;
            return View(retModel);
        }
    }
}
