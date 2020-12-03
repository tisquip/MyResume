using MediatR;
using Resume.Application.ViewModels;
using Resume.Domain;
using Resume.Domain.Response;
using Resume.Server.Data.Repositories;
using Resume.Server.Services.ExceptionNotifierServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Server.ProtoServer.FootballMatchProtoServices.Queries
{
    public class RequestMatchesForTeamQueryHandler : IRequestHandler<RequestMatchesForTeamQuery, Result<List<LiveMatchViewModel>>>
    {
        private readonly IRootAggregateRepositoryScoped<FootballTeam> rootAggregateRepositoryScopedFootBallTeam;
        private readonly IExceptionNotifierScoped exceptionNotifierScoped;

        public RequestMatchesForTeamQueryHandler(IRootAggregateRepositoryScoped<FootballTeam> rootAggregateRepositoryScopedFootBallTeam, IExceptionNotifierScoped exceptionNotifierScoped)
        {
            this.rootAggregateRepositoryScopedFootBallTeam = rootAggregateRepositoryScopedFootBallTeam;
            this.exceptionNotifierScoped = exceptionNotifierScoped;
        }
        public async Task<Result<List<LiveMatchViewModel>>> Handle(RequestMatchesForTeamQuery request, CancellationToken cancellationToken)
        {

            Result<List<LiveMatchViewModel>> vtr = new Result<List<LiveMatchViewModel>>();
            try
            {
                var team = rootAggregateRepositoryScopedFootBallTeam.GetDetachedFromDatabase(t => t.TeamId == request.TeamId)?.ReturnedObject?.FirstOrDefault();
                if (team == null)
                {
                    vtr.SetError($"Team with id {request.TeamId} was not found");
                }
                else
                {
                    vtr.SetSuccessObject(new List<LiveMatchViewModel>());

                    var footballMatches = team.JSListOfFootBallMatch_Matches.DeserializeFromJson<List<FootBallMatch>>() ?? new List<FootBallMatch>();

                    if (footballMatches.Any())
                    {
                        footballMatches.ForEach(f => vtr.ReturnedObject.Add(new LiveMatchViewModel(f)));
                    }
                }
            }
            catch (Exception ex)
            {
                await exceptionNotifierScoped.Notify(ex, vtr);
            }
            return vtr;
        }
    }
}
