using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using WorkEvents.Models;
using Microsoft.AspNetCore.SignalR.Client;

namespace WorkEventsProcess
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly ParametrosExecucaoServiceBus _parametrosExecucao;
        private readonly ServiceBusClient _client;
        private readonly string _subscription;
        private readonly string _topic;
        private HubConnection _connection;

        public Worker(ILogger<Worker> logger,
            ParametrosExecucaoServiceBus parametrosExecucao)
        {
            _logger = logger;
            _parametrosExecucao = parametrosExecucao;
            _subscription = parametrosExecucao.Subscription;
            _topic = parametrosExecucao.Topic;
            string nomeTopic = parametrosExecucao.Topic;
            
            // Criação do Client do Service Bus
            _client = new ServiceBusClient(
                parametrosExecucao.ConnectionString);

            _logger.LogInformation($"Topic = {_topic}");
            _logger.LogInformation($"Subscription = {_subscription}");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Run(async () =>
            {
                await RegisterOnMessageHandlerAndReceiveMessages();
                await CreateConnectionSignalR();
            });
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            await _client.DisposeAsync();
            _logger.LogInformation(
                "Conexao com o Azure Service Bus fechada!");

            await _connection.DisposeAsync();
            _logger.LogInformation(
                "Conexao com o SignalR fechada!");
        }

        private async Task RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new ServiceBusProcessorOptions()
            {
                AutoCompleteMessages = true,
                MaxConcurrentCalls = 3
            };

            // Cria a instância do processador
            var processor = _client.CreateProcessor(_topic, _subscription, messageHandlerOptions);      
            
            // Adiciona handler para processar as mensagens
            processor.ProcessMessageAsync += MessageHandler;

            // Adiciona handler para processar qualquer erro
            processor.ProcessErrorAsync += ErrorHandler;

            // Inicia o processo
            await processor.StartProcessingAsync();

            _logger.LogInformation("Aguardando mensagens...");
        }

        private async Task CreateConnectionSignalR()
        {
            var connection = new HubConnectionBuilder()
                    .WithUrl("https://localhost:44305/hub", options =>
                    {
                        options.Headers["Application"] = "WorkEventsProcess";
                    })
                    .WithAutomaticReconnect()
                    .Build();
            await connection.StartAsync();

            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };

            _connection = connection;
            _logger.LogInformation("Conexão criada com o SignalR");
        }

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            _logger.LogInformation($"Nova mensagem recebida. MessageID (Sbus): {args.Message.MessageId}");

            var messageDto = JsonConvert.DeserializeObject<MessageCreateDto>(args.Message.Body.ToString());
            var message = await Message.CreateMessage(messageDto.Title, messageDto.Description);

            _logger.LogInformation($"Message com o Id {message.Id} processada.");
           
            await _connection.SendAsync($"BroadcastMessage", message);
            _logger.LogInformation("Mensagem enviada ao SignalR.");
            
            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);            
        }

        // handle any errors when receiving messages
        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}