using Resume.Application.ViewModels;
using Resume.Grpc.Protos.Football;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resume.Application.ProtoAdapters
{
    public static class ProtoMessageLiveMatchViewModel_LiveMatchViewModel_Adapter
    {
        public static ProtoMessageLiveMatchViewModel Map(LiveMatchViewModel liveMatchViewModel)
        {
            if (liveMatchViewModel == null)
                return null;

            return new ProtoMessageLiveMatchViewModel()
            {
                AwayTeamLogo = liveMatchViewModel.AwayTeamLogo,
                HomeTeamLogo = liveMatchViewModel.HomeTeamLogo,
                AwayTeamName = liveMatchViewModel.AwayTeamName,
                GoalsAwayTeam = liveMatchViewModel.GoalsAwayTeam,
                GoalsHomeTeam = liveMatchViewModel.GoalsHomeTeam,
                HomeTeamName = liveMatchViewModel.HomeTeamName,
                MatchId = liveMatchViewModel.MatchId,
                MatchStatus = liveMatchViewModel.MatchStatus,
                Minute = liveMatchViewModel.Minute,
                PenaltiesScoredAwayTeam = liveMatchViewModel.PenaltiesScoredAwayTeam,
                PenaltiesScoredHomeTeam = liveMatchViewModel.PenaltiesScoredHomeTeam
            };
        }

        public static LiveMatchViewModel Map(ProtoMessageLiveMatchViewModel protoMessageLiveMatchViewModel)
        {
            if (protoMessageLiveMatchViewModel == null)
                return null;

            return new LiveMatchViewModel()
            {
                AwayTeamLogo = protoMessageLiveMatchViewModel.AwayTeamLogo,
                HomeTeamLogo = protoMessageLiveMatchViewModel.HomeTeamLogo,
                AwayTeamName = protoMessageLiveMatchViewModel.AwayTeamName,
                GoalsAwayTeam = protoMessageLiveMatchViewModel.GoalsAwayTeam,
                GoalsHomeTeam = protoMessageLiveMatchViewModel.GoalsHomeTeam,
                HomeTeamName = protoMessageLiveMatchViewModel.HomeTeamName,
                MatchId = protoMessageLiveMatchViewModel.MatchId,
                MatchStatus = protoMessageLiveMatchViewModel.MatchStatus,
                Minute = protoMessageLiveMatchViewModel.Minute,
                PenaltiesScoredAwayTeam = protoMessageLiveMatchViewModel.PenaltiesScoredAwayTeam,
                PenaltiesScoredHomeTeam = protoMessageLiveMatchViewModel.PenaltiesScoredHomeTeam
            };
        }
    }
}
