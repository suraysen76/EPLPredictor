using Microsoft.EntityFrameworkCore;
using SS1892.EPLPredictor.Interfaces;
using SS1892.EPLPredictor.Models;
using System.Diagnostics.Contracts;

namespace SS1892.EPLPredictor.Services
{
    public class PredictionService : IPredictionService
    {

        private readonly DBCtx _context;

        public PredictionService(DBCtx context)
        {
            _context = context;
        }
        public async Task<List<FixtureModel>> GetPredictions()
        {
            var predictTeamIdEPL = 6;// "Liverpool EPL";
            
            //var fixtures = await _context.Fixtures.Where(f => f.HomeTeamId == predictTeamId || f.AwayTeamId == predictTeamId).ToListAsync();
            var fmodelEPL =
            from f in _context.Fixtures.Where(f => f.HomeTeamId == predictTeamIdEPL || f.AwayTeamId == predictTeamIdEPL)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            where f.Type == "EPL"
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Location = f.Location,
                HomeTeamId = f.HomeTeamId,
                AwayTeamId = f.AwayTeamId,
                HomeTeam = t1.Team,
                AwayTeam = t2.Team,
                Result = f.Result,
                Type= f.Type,
                IsLocked = f.IsLocked
            };

            var predictTeamIdUCL = 24;// "Liverpool UCL";
            var fmodelUCL =
            from f in _context.Fixtures.Where(f => f.HomeTeamId == predictTeamIdUCL || f.AwayTeamId == predictTeamIdUCL )
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            where f.Type == "UCL"
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Location = f.Location,
                HomeTeamId = f.HomeTeamId,
                AwayTeamId = f.AwayTeamId,
                HomeTeam = t1.Team,
                AwayTeam = t2.Team,
                Result = f.Result,
                Type= f.Type,
                IsLocked = f.IsLocked
            };

            var predictTeamIdCRB = 60;// "Liverpool CRB";
            var fmodelCRB =
            from f in _context.Fixtures.Where(f => f.HomeTeamId == predictTeamIdCRB || f.AwayTeamId == predictTeamIdCRB)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            where f.Type == "CRB"
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Location = f.Location,
                HomeTeamId = f.HomeTeamId,
                AwayTeamId = f.AwayTeamId,
                HomeTeam = t1.Team,
                AwayTeam = t2.Team,
                Result = f.Result,
                Type = f.Type,
                IsLocked = f.IsLocked
            };

            var combinedList = fmodelEPL.Union(fmodelUCL).ToList().Union(fmodelCRB).ToList();

            return combinedList.OrderBy(f=>f.Date).ToList();
        }
        public async Task<FixturePredictionModel> GetPredictionsByFixture(int fixId)
        {

            var fpmodel = new FixturePredictionModel(); 
            var predictions = _context.Predictions.Where(f => f.Id == fixId).ToList();
           
            var fmodel =
            from f in _context.Fixtures.Where(f => f.Id == fixId)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Location = f.Location,
                HomeTeamId = f.HomeTeamId,
                AwayTeamId = f.AwayTeamId,
                HomeTeam = t1.Team,
                AwayTeam = t2.Team,
                Result = f.Result,
                IsLocked = f.IsLocked
            };
            var pmodel = await _context.Predictions.Where(p => p.FixtureId == fixId).OrderBy(p => p.UserId).ToListAsync();
            foreach (var item in pmodel)
            {
                var umodel = await _context.Users.Where(u => u.Id == item.UserId).FirstOrDefaultAsync();
                item.UserName = umodel.Name;
            }
            //var hteam = await _context.Teams.Where(t => t.Id == fmodel.HomeTeamId).FirstOrDefaultAsync();
            //fmodel.HomeTeam = hteam.Team;
            //var ateam = await _context.Teams.Where(t => t.Id == fmodel.AwayTeamId).FirstOrDefaultAsync();
            //fmodel.AwayTeam = ateam.Team;
            

            fpmodel.Fixture = fmodel.FirstOrDefault();
            fpmodel.Predictions = pmodel;

            return fpmodel;
        }



        public async Task<PredictionModel> UpdatePredictions(PredictionModel model)
        {
            var viewModel = _context.Predictions.Where(f => f.Id == model.Id).FirstOrDefault();
            if (viewModel == null)
            {
                var pmodel = new PredictionModel() { FixtureId = model.FixtureId, UserName = model.UserName, HomeTeamScore = model.HomeTeamScore, AwayTeamScore = model.AwayTeamScore, UpdatedDate = DateTime.Now };
                _context.Predictions.Add(pmodel);
            }
            else
            {
                viewModel.HomeTeamScore = model.HomeTeamScore;
                viewModel.AwayTeamScore = model.AwayTeamScore;
                viewModel.UpdatedDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();

            return viewModel;
        }

        public async Task<List<PredictionWinnersModel>> GetPredictionWinners(int fixtureId)
        {
           
            var X = _context.PredictionWinners.Where(pw => pw.FixtureId == fixtureId);
            var pwmodel =
            from pw in _context.PredictionWinners.Where(pw => pw.FixtureId == fixtureId)
            join u in _context.Users
            on pw.UserId equals u.Id  
            select new PredictionWinnersModel
            {
                Id = pw.Id,
                FixtureId= fixtureId,
                UserName = u.UserName,
                UserId=u.Id,
                Name= u.Name,
                Point=pw.Point
            };
            //var viewModel = await _context.PredictionWinners
            //    .Where(w => w.FixtureId == fixtureId).ToListAsync();
            return pwmodel.ToList();
        }

        public async Task<string> GetPredictionFixture(int fixtureId)
        {
            var fmodel =
            from f in _context.Fixtures.Where(f => f.Id == fixtureId)
            join t1 in _context.Teams
            on f.HomeTeamId equals t1.Id
            join t2 in _context.Teams
            on f.AwayTeamId equals t2.Id
            select new FixtureModel
            {
                Id = f.Id,
                MatchWeek = f.MatchWeek,
                Date = f.Date,
                Location= f.Location,
                HomeTeamId= f.HomeTeamId,
                AwayTeamId=f.AwayTeamId,
                HomeTeam=t1.Team,
                AwayTeam=t2.Team,
                Result=f.Result,
                IsLocked=f.IsLocked
            };

           
            return fmodel.FirstOrDefault().HomeTeam + " vs " + fmodel.FirstOrDefault().AwayTeam;
        }

        public async Task<List<PredictionStandingsModel>> GetPredictionStandings()
        {

            var pwmodel = from pw in _context.PredictionWinners
                      join u in _context.Users on pw.UserId equals u.Id
                      //where pw.Status != 2
                      group pw by new { pw.UserId, u.UserName,pw.Name } into grp
                      select (new PredictionStandingsModel
                      {
                          Name=grp.Key.Name,
                          UserId = grp.Key.UserId,
                          Username = grp.Key.UserName,
                          TotalPoints = grp.Sum(g => g.Point)
                      });
          




            //var pwmodel = await _context.PredictionWinners
            //    .GroupBy(g => g.UserId)                 
            //    .Select(cl => new PredictionStandingsModel
            //    {
            //        Name = cl.First().Name,
            //        UserId = cl.First().UserId,
            //        TotalPoints = cl.Sum(c => c.Point),
            //        Username=cl.First().UserName,
            //    })
            //    .ToListAsync();

        
            //foreach(var item in pwmodel)
            //{
            //    var user = await _context.Users.Where(u => u.Id == item.UserId).FirstAsync();
            //    item.Username = user.UserName;
            //}


            return pwmodel
                .OrderByDescending(m => m.TotalPoints)
                .ThenBy(m => m.Name)
                .ToList();
        }

        public async Task<PredictionsWithStandingsModel> GetMemberPredictionStandings(int userid)
        {
            var pwsmodel = new PredictionsWithStandingsModel();
            var pwmodel =
            from pw in _context.PredictionWinners
            .Where(p => p.UserId == userid)
            join u in _context.Users
            on pw.UserId equals u.Id
            group pw by pw.UserId into g
            select new PredictionStandingsModel
            {
                Username = AuthModel.UserName,
                Name = g.First().Name,
                UserId = g.First().UserId,
                TotalPoints = g.Sum(x => x.Point)
            };


            //var returnModel = 
            //    .Where(p=>p.UserId == userid)
            //    .GroupBy(g => g.UserId)
            //    .Select(cl => new PredictionStandingsModel
            //    {
            //        Username=cl.First().UserName,
            //        Name = cl.First().Name,
            //        UserId = cl.Key,
            //        TotalPoints = cl.Sum(c => c.Point),
            //    })
            //    .FirstAsync();          
            var pmodel =
          from p in _context.Predictions
          join pw in _context.PredictionWinners
          on p.FixtureId equals pw.FixtureId
          join f in _context.Fixtures
          on p.FixtureId equals f.Id
          join t1 in _context.Teams
          on f.HomeTeamId equals t1.Id
          join t2 in _context.Teams
          on f.AwayTeamId equals t2.Id          
          where p.UserId == userid
          where pw.UserId == userid
          orderby f.Date 

          select new PredictionModel
          {
              Id = p.Id,
              HomeTeam=t1.Team,
              AwayTeam=t2.Team,
              HomeTeamScore= p.HomeTeamScore,
              AwayTeamScore= p.AwayTeamScore,
              FixtureId= f.Id,
              FixtureDate= f.Date,
              UserId =   userid,
              Point =pw.Point,
              
          };

            pwsmodel.Standing = pwmodel.FirstOrDefault();
            pwsmodel.Predictions = pmodel.Distinct().OrderBy(o=>o.FixtureDate).ToList();
            return pwsmodel;
        }
        public async Task<PredictionModel> GetMyPrediction(int fixtureId, int userid)
        {

            var pmodel =
                from f in _context.Fixtures.Where(p => p.Id == fixtureId)
                join t1 in _context.Teams
                on f.HomeTeamId equals t1.Id
                join t2 in _context.Teams
                on f.AwayTeamId equals t2.Id
                select new FixturePredictionModel
                {
                    Fixture = new FixtureModel() 
                    { 
                        Id = f.Id,
                        HomeTeam=t1.Team,
                        AwayTeam=t2.Team,
                        HomeTeamId=t1.Id,
                        AwayTeamId=t2.Id,
                        Type=f.Type,
                        Location=f.Location,
                        IsLocked=f.IsLocked,
                        Date=f.Date,
                        MatchWeek=f.MatchWeek,
                        Result= f.Result
                    },
                    Predictions = new List<PredictionModel>()
                };

            var viewModel = _context.Predictions.Where(p => p.FixtureId == fixtureId && p.UserId == userid).FirstOrDefault();
            if (viewModel == null)
            {
                viewModel = new PredictionModel() 
                { 
                    Id = 9999, 
                    FixtureId = fixtureId, 
                    UserId = AuthModel.UserId, 
                    UserName= AuthModel.UserName, 
                    HomeTeam = pmodel.FirstOrDefault().Fixture.HomeTeam,
                    AwayTeam= pmodel.FirstOrDefault().Fixture.AwayTeam,
                    AwayTeamScore = null };
            }
            return viewModel;
        }
        public async Task<bool> UpdateMyPrediction(PredictionModel predictionModel)
        {
            var ret = false;
            var pmodel = new PredictionModel();
            if (predictionModel.Id == 9999)
            {
                pmodel = new PredictionModel()
                {
                    FixtureId = predictionModel.FixtureId,
                    UserId = predictionModel.UserId,
                    UserName = predictionModel.UserName,
                    HomeTeamScore = predictionModel.HomeTeamScore,
                    AwayTeamScore = predictionModel.AwayTeamScore,
                    UpdatedDate = DateTime.Now
                };
                _context.Predictions.Add(pmodel);
                ret = true;
            }
            else
            {
                _context.Predictions.Update(predictionModel);
                ret = true;
            }
            
            await _context.SaveChangesAsync();
            return ret;
        }

        public async Task<PredictionModel> GetPrediction(int fixtureId)
        {

            var pmodel =
                from f in _context.Fixtures.Where(p => p.Id == fixtureId)
                join t1 in _context.Teams
                on f.HomeTeamId equals t1.Id
                join t2 in _context.Teams
                on f.AwayTeamId equals t2.Id
                select new FixturePredictionModel
                {
                    Fixture = new FixtureModel()
                    {
                        Id = f.Id,
                        HomeTeam = t1.Team,
                        AwayTeam = t2.Team,
                        HomeTeamId = t1.Id,
                        AwayTeamId = t2.Id,
                        Type = f.Type,
                        Location = f.Location,
                        IsLocked = f.IsLocked,
                        Date = f.Date,
                        MatchWeek = f.MatchWeek,
                        Result = f.Result
                    },
                    Predictions = new List<PredictionModel>()
                };
            var viewModel = _context.Predictions.Where(p => p.FixtureId == fixtureId).FirstOrDefault();
            if (viewModel == null)
            {
                viewModel = new PredictionModel()
                {
                    Id = 9999,
                   // xId= -1,
                    FixtureId = fixtureId,
                    UserId = 0,
                    UserName = null,
                    HomeTeam = pmodel.FirstOrDefault().Fixture.HomeTeam,
                    AwayTeam = pmodel.FirstOrDefault().Fixture.AwayTeam,
                    AwayTeamScore = null
                };
            }
            return viewModel;
        }


        public async Task<List<UserPredictionModel>> GetMembersPrediction(int fixId)
        {
            var umodel = _context.Users.Where(u=>u.IsActive==true).ToList();               
            //var pModel =   _context.Predictions
            //    .Where(p=>p.FixtureId==fixId).ToList();
            //if (pModel.Count==0)
            //{
            //   pModel.Add(new PredictionModel() { FixtureId=fixId,HomeTeamScore=null,AwayTeamScore=null });
            //}
            var fModel = from f in _context.Fixtures
                         join t1 in _context.Teams
                         on f.HomeTeamId equals t1.Id
                         join t2 in _context.Teams
                         on f.AwayTeamId equals t2.Id
                         where f.Id == fixId
                         select new FixtureModel
                         {
                            Id = f.Id,
                            HomeTeamId= f.Id,
                            AwayTeamId= f.Id,
                            HomeTeam=t1.Team,
                            AwayTeam=t2.Team,
                            Date= f.Date,
                            IsLocked= f.IsLocked, 
                            Location= f.Location,
                            MatchWeek= f.MatchWeek,  
                            Result=f.Result,
                            Type= f.Type, 
                         };

            var retModel= new List<UserPredictionModel>();
            foreach ( var user in umodel ) {
                var upmodel = new UserPredictionModel();
                upmodel.Id = user.Id;
                upmodel.UserId = user.Id;
                upmodel.UserName  = user.UserName;
                upmodel.Name = user.Name;                             
                upmodel.Fixture = fModel.First();

                //Add Prediction
                var p = new UserPredictionModel();
                var existp = _context.Predictions
                .Where(p => p.FixtureId == fixId && p.UserId==user.Id).ToList();
                if (existp.Count > 0)
                {
                    upmodel.Prediction=existp.First();
                    //pModel.Add(new PredictionModel() { FixtureId = fixId, HomeTeamScore = null, AwayTeamScore = null,UserId=user.Id });
                }
                else
                {
                    upmodel.Prediction = new PredictionModel() { FixtureId = fixId, HomeTeamScore = null, AwayTeamScore = null, UserId = user.Id };
                }
                //upmodel.Prediction = pModel.First();
                retModel.Add(upmodel);
            }

            return retModel;
        }
        public async Task<bool> SetMembersPrediction(List<UserPredictionModel> upmodel)
        {

            
            foreach(var item in upmodel) {
                var userId= item.UserId;
                var fixId = item.Prediction.FixtureId;
                var prediction = item.Prediction;
                var pModel = _context.Predictions.Where(p=>p.UserId==item.UserId && p.FixtureId==fixId);
                prediction.UpdatedDate= DateTime.Now;
                prediction.UserId = userId;
                
                if (prediction.Id>0)
                {                    
                    _context.Predictions.Update(prediction);
                }
                else
                {
                    if (prediction.HomeTeamScore != null)
                    {
                        _context.Predictions.Add(prediction);
                    }
                }
                _context.SaveChanges();
            }
            

            return true;
        }

    }
}