using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using System;
using System.Net.Http;

namespace Resume.Grpc
{
    internal static class GrpcChannelService
    {
        private static GrpcChannel grpcChannel;
        public static GrpcChannel GetGrpcChannel(string grpcServerUrl)
        {
            if (grpcChannel == null)
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                var httpHandler = new GrpcWebHandler(GrpcWebMode.GrpcWeb, new HttpClientHandler());
                grpcChannel = GrpcChannel.ForAddress(grpcServerUrl, new GrpcChannelOptions { HttpHandler = httpHandler });
            }
            return grpcChannel;
        }
    }
}
