using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MWL.ReviewService.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ReviewService.Messaging
{
    public class RPCServerManager : IHostedService
    {
        private readonly List<RPCServer> _servers;
        private readonly IReviewService _reviewService;
        private readonly IServiceProvider _serviceProvider;

        public RPCServerManager(IServiceProvider serviceProvider)
        {
            _servers = new List<RPCServer>();
            _serviceProvider = serviceProvider;
            AddQueues();
        }

        private void AddQueues()
        {
            _servers.Add(new RPCServer("reviewservice_messaging", _serviceProvider));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await StartListening();
        }

        private async Task StartListening()
        {
            foreach (var server in _servers)
            {
                await server.StartListening();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            StopListening();
            return Task.CompletedTask;
        }

        private void StopListening()
        {
            foreach (var server in _servers)
            {
                server.Close();
            }
        }
    }
}
