﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocWorkerService.Consumer
{
    public class ClienteConsumer : IKafkaConsumer<IntegrationEvent>
    {
        private readonly ResultConsumer _consumer;
        private readonly ILogger<ClienteConsumer> _logger;
        private readonly IObterClienteUseCase _obterClienteUseCase;
        private readonly IMessageMapper<ObterClienteDto> _mapper;
        public ClienteConsumer(ClienteEnvelope envelope,
        ILogger<ClienteConsumer> logger,
        IObterClienteUseCase obterClienteUseCase,
        IMessageMapper<ObterClienteDto> mapper)
        {
            _logger = logger;
            _logger.LogInformation("$ Entrou no ClienteConsumer.");
            _consumer = new ResultConsumer(ClienteEnvelope.PassValue(envelope));

            _obterClienteUseCase = obterClienteUseCase;
            _mapper = mapper;
            _logger.LogInformation($"Topic : {envelope.Topic},GroupId: {envelope.GroupId}, BootstrapServers: {envelope.BootstrapServers}");
        }

        public async Task ConsumeAsync(IntegrationEvent message, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _consumer.ConsumeAndReturn(cancellationToken);

                if (result != null)
                {
                    _logger.LogInformation($"[ClienteConsumer] Mensagem processada: Key={result.Key}, Value={result.Value}");
                    await _obterClienteUseCase.Execute(_mapper.MapToDto(result.Value));
                }

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Ocorreu um erro ao consumer o cliente: {ex.Message} , mais detalhes: {ex.InnerException!.Message}");
                throw new Exception($"Ocorreu um erro ao consumer o cliente: {ex.Message}");
            }
        }
    }
}
