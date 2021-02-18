﻿using Framework.Core.Commands;
using Framework.Core.Events;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Core.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        readonly IMediator _mediator;
        readonly List<Type> _eventTypes;
        readonly Dictionary<string, List<Type>> _handlers;
        readonly IServiceScopeFactory _serviceScopeFactory;

        void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory { HostName = "localhost", DispatchConsumersAsync = true, };

            var connection = factory.CreateConnection();
            IModel channel = connection.CreateModel();

            var eventName = typeof(T).Name;

            channel.QueueDeclare(eventName, false, false, false, null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += Consumer_Received;

            channel.BasicConsume(eventName, true, consumer);
        }
        public void Subscribe<T, TH>()
            where T : Event
            where TH : IEventHandler<T>
        {
            var handlerType = typeof(TH);
            var eventName = typeof(T).Name;

            if (!_eventTypes.Contains(typeof(T)))
                _eventTypes.Add(typeof(T));

            if (!_handlers.ContainsKey(eventName))
                _handlers.Add(eventName, new List<Type>());

            if (_handlers[eventName].Any(s => s.GetType() == handlerType))
                throw new ArgumentException($"Handler Type  {handlerType.Name} alredy is registered for '{eventName}'", nameof(handlerType));

            _handlers[eventName].Add(handlerType);
            StartBasicConsume<T>();
        }
        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var eventName = @event.GetType().Name;
                channel.QueueDeclare(eventName, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish("", eventName, null, body);
            }
        }
        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }
        async Task ProcessEvent(string eventName, string message)
        {
            if (_handlers.ContainsKey(eventName))
            {
                using (var scop = _serviceScopeFactory.CreateScope())
                {
                    var subscribtions = _handlers[eventName];
                    foreach (var subscribtion in subscribtions)
                    {
                        //var handler = Activator.CreateInstance(subscribtion);
                        var handler = scop.ServiceProvider.GetService(subscribtion);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                }
            }
        }
        async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventName = e.RoutingKey;
            var body = e.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                await ProcessEvent(eventName, message);
                //await ProcessEvent(eventName, message).CofigureAwait(false);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
            _mediator = mediator;
            _eventTypes = new List<Type>();
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
        }
    }
}