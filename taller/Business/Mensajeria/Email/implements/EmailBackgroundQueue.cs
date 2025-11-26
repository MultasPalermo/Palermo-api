using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Business.Mensajeria.Email.implements
{
    public class EmailBackgroundQueue
    {
        private readonly Channel<Func<IServiceProvider, Task>> _queue =
      Channel.CreateUnbounded<Func<IServiceProvider, Task>>();
        private readonly ILogger<EmailBackgroundQueue> _logger;

        public EmailBackgroundQueue(ILogger<EmailBackgroundQueue> logger)
        {
            _logger = logger;
        }

        // Encolar un trabajo
        public async Task QueueBackgroundWorkItemAsync(
      Func<IServiceProvider, Task> workItem)
        {
            await _queue.Writer.WriteAsync(workItem);
        }

        public IAsyncEnumerable<Func<IServiceProvider, Task>> DequeueAsync(CancellationToken token)
            => _queue.Reader.ReadAllAsync(token);
    }

}
