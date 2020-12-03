using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Resume.Grpc.Protos.Football;
using Resume.Server.ProtoServer.FootballMatchProtoServices.Queries;
using Resume.Server.Services.ExceptionNotifierServices;

namespace Resume.Server.ProtoServer.FootballMatchProtoServices
{
    public class FootballMatchProtoServicesProtoImplementation : FootballMatchProtoServicesProto.FootballMatchProtoServicesProtoBase
    {
        private readonly IMediator mediator;

        public FootballMatchProtoServicesProtoImplementation(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public override async Task<ProtoMessageResponseResultOfListOfLiveMatchViewModel> RequestMatchesForTeam(ProtoMessageRequestMatches request, ServerCallContext context)
        {
            RequestMatchesForTeamQuery requestMatchesForTeamQuery = new RequestMatchesForTeamQuery(request.TeamId);
            var resultOfListOfLiveMatchViewModel = await mediator.Send(requestMatchesForTeamQuery);

            var protoResult = new ProtoMessageResult()
            {
                Succeeded = resultOfListOfLiveMatchViewModel.Succeeded
            };

            if (resultOfListOfLiveMatchViewModel?.Messages?.Any() ?? false)
            {
                protoResult.Messages.AddRange(resultOfListOfLiveMatchViewModel.Messages);
            }

            var vtr = new ProtoMessageResponseResultOfListOfLiveMatchViewModel()
            { 
                Result = protoResult
            };

            if (resultOfListOfLiveMatchViewModel?.ReturnedObject?.Any() ?? false)
            {
                vtr.ListOfLiveMatchViewModel.AddRange(resultOfListOfLiveMatchViewModel
                    .ReturnedObject
                    .Select(lm => new ProtoMessageLiveMatchViewModel() 
                    { 
                        AwayTeamLogo = lm.AwayTeamLogo,
                        HomeTeamLogo = lm.HomeTeamLogo,
                        AwayTeamName = lm.AwayTeamName,
                        GoalsAwayTeam = lm.GoalsAwayTeam,
                        GoalsHomeTeam = lm.GoalsHomeTeam,
                        HomeTeamName = lm.HomeTeamName,
                        MatchId = lm.MatchId,
                        MatchStatus = lm.MatchStatus,
                        Minute = lm.Minute,
                        PenaltiesScoredAwayTeam = lm.PenaltiesScoredAwayTeam,
                        PenaltiesScoredHomeTeam = lm.PenaltiesScoredHomeTeam
                    }));
            }

            return vtr;
        }
        //public override async Task<ProtoMessageLiveMatchViewModel> RequestMatchesForTeam(ProtoMessageRequestMatches request, ServerCallContext context)
        //{

        //    RequestMatchesForTeamQuery requestMatchesForTeamQuery = new RequestMatchesForTeamQuery(request.TeamId);
        //    var resultOfListOfLiveMatchViewModel = await mediator.Send(requestMatchesForTeamQuery);

        //    ProtoMessageResult resultProto = new ProtoMessageResult()
        //    {
        //        Succeeded = resultOfListOfLiveMatchViewModel.Succeeded
        //    };

        //    if (resultOfListOfLiveMatchViewModel.Messages?.Any() ?? false)
        //    {
        //        resultProto.Messages.AddRange(resultOfListOfLiveMatchViewModel.Messages);
        //    }

        //    return new ProtoMessageLiveMatchViewModel();
        //}
    }
}
