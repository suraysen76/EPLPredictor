using Microsoft.EntityFrameworkCore;

namespace SS1892.EPLPredictor.Models
{
    public class DBCtx:DbContext
    {
        public DBCtx(DbContextOptions<DBCtx> options)
       : base(options)
        {
        }
        
        public DbSet<UserModel> Users { get; set; }
        public DbSet<FixtureModel> Fixtures { get; set; }
        public DbSet<PredictionModel> Predictions { get; set; }
        public DbSet<ScoreModel> Scores { get; set; }
        public DbSet<PointModel> Points { get; set; }
        public DbSet<TeamModel> Teams { get; set; }
        public DbSet<TeamStatModel> TeamStats { get; set; }

        public DbSet<ResultModel> Results { get; set; }
    }
}
