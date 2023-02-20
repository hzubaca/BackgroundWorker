using FlowerSpot.SharedKernel.Contracts;
using System.Collections.Concurrent;

namespace FlowerSpot.SharedKernel.Services;
public class ProcessingQueue<T> : IProcessingQueue<T> where T : class
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly int ChunkSize = 10;

    public void Enqueue(T item)
    {
        _queue.Enqueue(item);
    }

    public T? Dequeue()
    {
        var isDequeued = _queue.TryDequeue(out var item);
        return isDequeued ? item : null;
    }

    public IReadOnlyCollection<T> DequeueChunk()
    {
        var items = new List<T>();

        for (var i = 0; i < ChunkSize; i++)
        {
            if (_queue.IsEmpty) break;

            var item = Dequeue();
            if (item != null)
            {
                items.Add(item);
            }
        }

        return items.AsReadOnly();
    }
}
