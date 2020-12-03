using MediatR;
using Resume.Application.ViewModels;
using Resume.Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resume.Server.ProtoServer.FootballMatchProtoServices.Queries
{
    public class RequestMatchesForTeamQuery : IRequest<Result<List<LiveMatchViewModel>>>
    {
        public string TeamId { get; set; }
        public RequestMatchesForTeamQuery()
        {
        }
        public RequestMatchesForTeamQuery(string teamId)
        {
            TeamId = teamId;
        }
    }
}
