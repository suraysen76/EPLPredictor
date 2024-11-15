﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
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
    public class PredictionsController : Controller
    {
        private IPredictionService? _predictionService;

        public PredictionsController(IPredictionService predictionService)
        {
            _predictionService = predictionService;
        }

        // GET: Predictions
        public async Task<IActionResult> Index()
        {
            
            var fixturess = await _predictionService?.GetPredictions();
            return View(fixturess);
        }
        public async Task<IActionResult> GetPredictionsByFixture(int fixId)
        {
            var model = await _predictionService.GetPredictionsByFixture(fixId);
            return View(model);
        }

        // POST: Predictions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PredictionModel predictionModel)
        {
            var retModel = await _predictionService.UpdatePredictions(predictionModel);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PredictionsWinners(int id)
        {
            ViewBag.Fixture = await _predictionService.GetPredictionFixture(id);
            var model = await _predictionService.GetPredictionWinners(id);
            return View(model);
        }
        
        public async Task<IActionResult> PredictionStandings()
        {            
            var model = await _predictionService.GetPredictionStandings();
            return View(model);
        }

        
        public async Task<IActionResult> GetMyPrediction(int id)
        {
            ViewBag.Fixture = await _predictionService.GetPredictionFixture(id);
            var model = await _predictionService.GetMyPrediction(id, AuthModel.UserId);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateMyPrediction(PredictionModel model)
        {

            var viewmodel = await _predictionService.UpdatePredictions(model);
            return RedirectToAction("GetPredictionsByFixture", new { fixId = model.Id });
        }

        public async Task<IActionResult> CreatePrediction(int id)
        {

            var model = await _predictionService.GetPrediction(id);

            return View(model);// RedirectToAction("GetPredictionsByFixture","Predictions",new { id = id });

        }
        [HttpPost]
        public async Task<IActionResult> CreatePrediction(PredictionModel pmodel)
        {
            //  return View();
            var model = await _predictionService.UpdatePredictions(pmodel);
            return RedirectToAction("GetPredictionsByFixture", "Predictions", new { fixId = pmodel.FixtureId });
            
        }
        
        public async Task<IActionResult> MembersPrediction(int fixId)
        {          
            List<UserPredictionModel> model = await _predictionService.GetMembersPrediction(fixId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> MembersPrediction(List<UserPredictionModel> upmodel)
        {
            if (ModelState.IsValid)
            {
               var success= _predictionService.SetMembersPrediction(upmodel);
                return RedirectToAction("Index");

            }


            // ModelState is not valid, return to the form with errors
            return View(upmodel);
        }

        //private bool PredictionModelExists(int id)
        //{
        //  return (_context.Predictions?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }


}
