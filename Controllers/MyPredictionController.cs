using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;
using SS1892.EPLPredictor.Utility;

namespace SS1892.EPLPredictor.Controllers
{
    public class MyPredictionController : Controller
    {
        private readonly DBCtx _context;
        private IPredictionService? _predictionService;

        public MyPredictionController(DBCtx context, IPredictionService predictionService)
        {
            _context = context;
            _predictionService = predictionService;
        }

        // GET: MyPrediction/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            
            var model = await _predictionService.GetMyPrediction(id, AuthModel.UserId);
            
            return View(model);// RedirectToAction("GetPredictionsByFixture","Predictions",new { id = id });
            
        }

        // POST: MyPrediction/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PredictionModel predictionModel)
        {
            var x = await _predictionService.UpdateMyPrediction(predictionModel);
            //return View(predictionModel);
            return RedirectToAction("GetPredictionsByFixture", "Predictions", new {id= predictionModel .FixtureId});
        }
 
    }
}
