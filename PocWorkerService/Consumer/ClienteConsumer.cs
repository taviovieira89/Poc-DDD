using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocWorkerService.Consumer
{
    public class ClienteConsumer //: ResultConsumer
    {
        private readonly ClienteEnvelope _envelope;
        public ClienteConsumer(ClienteEnvelope envelope)
        {
            _envelope = envelope;
        }

    }
}
