using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using MediatR;
using Resume.Application.ProtoAdapters;
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

            var vtr = new ProtoMessageResponseResultOfListOfLiveMatchViewModel() 
            {
                Result = ProtoMessageResult_Result_Adapter.Map(resultOfListOfLiveMatchViewModel)
            };

            if (resultOfListOfLiveMatchViewModel?.ReturnedObject?.Any() ?? false)
            {
                List<ProtoMessageLiveMatchViewModel> listToAdd = resultOfListOfLiveMatchViewModel.ReturnedObject.Select(lm => ProtoMessageLiveMatchViewModel_LiveMatchViewModel_Adapter.Map(lm)).ToList();

                vtr.ListOfLiveMatchViewModel.AddRange(listToAdd);
            }
            return vtr;
        }
    }
}
