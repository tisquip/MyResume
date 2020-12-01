using Microsoft.Extensions.Hosting;
using Resume.Domain.Interfaces;
using Resume.Server.Services.FootballWorkerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Resume.Server.Services
{
    public class BackgroundFootballService : IHostedService
    {
        private readonly IFootBallWorker footBallWorker;

        public BackgroundFootballService(IExceptionNotifier exceptionNotifier, IFootBallWorker footBallWorker)
        {
            this.footBallWorker = footBallWorker;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
