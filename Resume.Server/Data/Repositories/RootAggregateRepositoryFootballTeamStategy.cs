using Data;
using Resume.Domain;
using Resume.Domain.Interfaces;
using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.Data.Repositories
{
    public static class RootAggregateRepositoryFootballTeamStategy
    {
        public static async Task<Result> Update(ResumeBackgroundServiceDbContext db, FootballTeam footballTeam)
        {
            try
            {
                return await ProcessUpdate(e => { db.Attach(e); db.Update(e); }, async () => await db.SaveChangesAsync(), footballTeam);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static async Task<Result> Update(ResumeDbContext db, FootballTeam footballTeam)
        {
            return await ProcessUpdate(e => db.Update(e), async () => await db.SaveChangesAsync(), footballTeam);
        }

        public static Result<List<FootballTeam>> GetDetachedFromDatabase(ResumeDbContext db, Predicate<FootballTeam> predicate)
        {
            return ProcessGetDetachedFromDatabase(db.FootballTeam.AsEnumerable(), predicate);
        }

        public static Result<List<FootballTeam>> GetDetachedFromDatabase(ResumeBackgroundServiceDbContext db, Predicate<FootballTeam> predicate)
        {
            return ProcessGetDetachedFromDatabase(db.FootballTeam.AsEnumerable(), predicate);
        }

        private static Result<List<FootballTeam>> ProcessGetDetachedFromDatabase(IEnumerable<FootballTeam> footballTeams, Predicate<FootballTeam> predicate)
        {
            if (predicate == null)
            {
                predicate = (f) => true;
            }
            Result<List<FootballTeam>> vtr = new Result<List<FootballTeam>>();
            vtr.SetSuccessObject(footballTeams?.Where(f => predicate(f))?.ToList() ?? new List<FootballTeam>());
            return vtr;
        }

        public static async Task<Result> Insert(ResumeDbContext db, FootballTeam footballTeam)
        {
            Result vtr = new Result();
            vtr.SetError("Prohibited from inserting a new football team"); //<-- The only allowed place to insert a new team is in the hostedservice, which is a singleton and uses the singleton version of the dbcontext. I felt that I should not remove the method from the parent IRootAggregateRepository since there is also PaymentRecipt which can be inserted from a scoped perspective.
           
            return vtr;
        }

        public static async Task<Result> Insert(ResumeBackgroundServiceDbContext db, FootballTeam footballTeam)
        {
            return await ProcessInsert(e => db.Add(e), async () => await db.SaveChangesAsync(), footballTeam);
        }



        static async Task<Result> ProcessInsert(Action<IEntity> addEntityToDatabase, Func<Task<int>> saveChangesToDb, FootballTeam footBallTeam)
        {
            Func<Task<Result>> proccessAll = async () =>
            {
                addEntityToDatabase(footBallTeam);
                return await SaveChangesToDb(saveChangesToDb);
            };

            return await ProcessDbSaveFunctions(proccessAll);
        }

        static async Task<Result> ProcessUpdate(Action<IEntity> addToUpdateDb, Func<Task<int>> saveChangesToDb, FootballTeam footballTeam)
        {

            Func<Task<Result>> proccessAll = async () =>
            {
                AttachToDbByUpdate(addToUpdateDb, footballTeam);
                var r = await saveChangesToDb();
                return Result.Ok();
            };

            return await ProcessDbSaveFunctions(proccessAll);
        }

        static async Task<Result> ProcessDbSaveFunctions(Func<Task<Result>> processAllSteps)
        {
            Result vtr = new Result(true);
            try
            {
                vtr = await processAllSteps();
            }
            catch (DbException dbEx)
            {
                vtr.SetError(dbEx.Message);
            }
            catch (Exception ex)
            {
                vtr.SetError();
            }
            return vtr;
        }

        static void AttachToDbByUpdate(Action<IEntity> addUpdateToDatabaseTracking, FootballTeam footballTeam)
        {
            try
            {
                addUpdateToDatabaseTracking(footballTeam);
            }
            catch (Exception)
            {
            }

            //If FootballTeam had any Navigation Properties that are entites themselves, then we would try and update those as well
        }
        static async Task<Result> SaveChangesToDb(Func<Task<int>> saveChangesToDb)
        {
            try
            {
                int rowsAffected = await saveChangesToDb();
                return (rowsAffected > 0) ? Result.Ok() : Result.Error();
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        
    }
}
