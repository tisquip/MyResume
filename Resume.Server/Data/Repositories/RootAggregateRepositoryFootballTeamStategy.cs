using Data;
using Resume.Domain;
using Resume.Domain.Interfaces;
using Resume.Domain.Response;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace Resume.Server.Data.Repositories
{
    public static class RootAggregateRepositoryFootballTeamStategy
    {
        public static async Task<Result> Update(ResumeBackgroundServiceDbContext db, FootballTeam footballTeam)
        {
            return await Process(e => db.Attach(e), async () => await db.SaveChangesAsync(), footballTeam);
        }

        public static async Task<Result> Update(ResumeDbContext db, FootballTeam footballTeam)
        {
            return await Process(e => db.Attach(e), async () => await db.SaveChangesAsync(), footballTeam);
        }

        static async Task<Result> Process(Action<IEntity> attachToDatabse, Func<Task<int>> saveChangesToDb, FootballTeam footballTeam)
        {
            Result vtr = new Result(true);
            try
            {
                AttachToDb(attachToDatabse, footballTeam);
                vtr = await SaveChangesToDb(saveChangesToDb);
            }
            catch (DbException dbEx)
            {
                vtr.SetError(dbEx.Message);
            }
            catch (Exception)
            {
                vtr.SetError();
            }
            return vtr;
        }

        static void AttachToDb(Action<IEntity> attachToDatabse, FootballTeam footballTeam)
        {
            try
            {
                attachToDatabse(footballTeam);
            }
            catch (Exception)
            {
            }

            //If FootballTeam had any Navigation Properties that are entites themselves, then we would try and attach those as well
        }
        static async Task<Result> SaveChangesToDb(Func<Task<int>> saveChangesToDb)
        {
            int rowsAffected = await saveChangesToDb();
            return (rowsAffected > 0) ? Result.Ok() : Result.Error();
        }
        
    }
}
