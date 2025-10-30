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
        private readonly Channel<Func<Task>> _queue = Channel.CreateUnbounded<Func<Task>>();

        // Se encola un "trabajo" (delegado async)
        public async Task QueueBackgroundWorkItemAsync(Func<Task> workItem)
        {
            await _queue.Writer.WriteAsync(workItem);
        }

        // El worker lo consume uno a uno
        public IAsyncEnumerable<Func<Task>> DequeueAsync(CancellationToken token)
            => _queue.Reader.ReadAllAsync(token);
    }

}
