using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs.Host.Executors;
using Microsoft.Azure.WebJobs.Host.Listeners;
using StackExchange.Redis;

namespace Developerro.AzureFunctions.CustomBindings
{
    public class RedisSubListener : IListener
    {
        private readonly ITriggeredFunctionExecutor _executor;
        private readonly RedisSubTriggerAttribute _attribute;
        private readonly ConnectionMultiplexer _connection;
        private ISubscriber _subscriber = null;

        public RedisSubListener(ITriggeredFunctionExecutor executor, RedisSubTriggerAttribute attribute)
        {
            _executor = executor;
            _attribute = attribute;
            _connection = ConnectionMultiplexer.Connect(attribute.GetConnectionString());
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_subscriber != null) throw new InvalidOperationException("Redis Pub/Sub listener has alread been started");

            _subscriber = _connection.GetSubscriber();
            return _subscriber.SubscribeAsync(_attribute.Channel, OnMessageArrived);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_subscriber == null) throw new InvalidOperationException("Redis Pub/Sub listener has already been stopped");

            await _subscriber.UnsubscribeAllAsync();
            _subscriber = null;
        }

        public void Cancel()
        {
            if (_subscriber == null) return;

            _subscriber.UnsubscribeAll();
            _subscriber = null;
        }

        public void Dispose()
        {
            _connection.Dispose();
        }

        private void OnMessageArrived(RedisChannel channel, RedisValue value)
        {
            var triggerData = new TriggeredFunctionData
            {
                TriggerValue = value.ToString()
            };
            var task = _executor.TryExecuteAsync(triggerData, CancellationToken.None);
            task.Wait();
        }
    }
}
