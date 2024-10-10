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
    
    public class FixturesController : Controller
    {
        
        private IFixtureService? _fixtureService;
        public FixturesController( IFixtureService fixtureService)
        {
            
            _fixtureService = fixtureService;
        }

        // GET: Fixtures
        public async Task<IActionResult> Index(string type)
        {
            
            var fixtures = await _fixtureService?.GetFixtures(type);
            return View(fixtures);
            
        }

      
        // GET: Fixtures/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            FixtureModel model = await _fixtureService.GetFixturesById(id);
            return View(model);
        }

        // POST: Fixtures/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FixtureModel fixtureModel)
        {
            
            var retModel = await _fixtureService?.UpdateFixtures(fixtureModel);
            return RedirectToAction("Index", new {type=retModel.Type});
           
        }



        public async Task<IActionResult> GetFixtureByTeam(string type, int teamId)
        {

            var retModel = await _fixtureService?.GetFixtureByTeam(teamId);
            return View(retModel);

        }


        public async Task<IActionResult> Create()
        {
            var type ="UCL";
            ViewBag.AvailableTeams = _fixtureService.GetTeamsForDrpdn(type);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FixtureModel fModel)
        {
            var response = await _fixtureService.AddFixture(fModel);           

            ViewBag.Message = response.Message;
            if (response.Status)
            {
                return RedirectToAction("Index", new {type=fModel.Type});
            }

            return View();
        }
    }
}
