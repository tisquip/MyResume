using Resume.Grpc.Protos.Football;

namespace Resume.Grpc
{
    public static class GrpcClients
    {
        private static FootballMatchProtoServicesProto.FootballMatchProtoServicesProtoClient grpcClientFootballMatch;
        public static FootballMatchProtoServicesProto.FootballMatchProtoServicesProtoClient GrpcClientFootballMatch(string grpcServerUrl)
        {
            if (grpcClientFootballMatch == null)
            {
                grpcClientFootballMatch = new FootballMatchProtoServicesProto.FootballMatchProtoServicesProtoClient(GrpcChannelService.GetGrpcChannel(grpcServerUrl));
            }
            return grpcClientFootballMatch;
        }
    }
}
