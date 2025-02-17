using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocWorkerService.Consumer
{
    public class ClienteConsumer
    {
        private readonly ResultConsumer _consumer;
        private CancellationTokenSource _cts;
        public ClienteConsumer(ClienteEnvelope envelope)
        {
            _consumer = new ResultConsumer(envelope);
            _cts = new CancellationTokenSource();
        }

        public async Task StartListening(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var envelope = await _consumer.ConsumeAndReturn(cancellationToken);

                if (envelope != null)
                {
                    Console.WriteLine($"[ClienteConsumer] Mensagem processada: Key={envelope.Key}, Value={envelope.Value}");
                }
            }
        }
        public void StopListening()
        {
            _cts.Cancel();
        }

    }
}
