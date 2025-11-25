using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class EmailBackgroundQueue
    {
        private readonly Channel<Func<Task>> _queue = Channel.CreateUnbounded<Func<Task>>();
        private readonly ILogger<EmailBackgroundQueue> _logger;

        public EmailBackgroundQueue(ILogger<EmailBackgroundQueue> logger)
        {
            _logger = logger;
        }

        // Encolar un trabajo
        public async Task QueueBackgroundWorkItemAsync(Func<Task> workItem)
        {
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            await _queue.Writer.WriteAsync(workItem);

            // 🔍 Log opcional para monitoreo
            _logger.LogDebug($"📥 Job encolado. Pendientes en cola: {_queue.Reader.Count}");
        }

        // El worker consume jobs
        public IAsyncEnumerable<Func<Task>> DequeueAsync(CancellationToken token)
            => _queue.Reader.ReadAllAsync(token);
    }
}