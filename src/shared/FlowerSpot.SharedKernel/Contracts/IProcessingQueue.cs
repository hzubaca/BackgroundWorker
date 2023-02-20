namespace FlowerSpot.SharedKernel.Contracts;
public interface IProcessingQueue<T>
{
    void Enqueue(T item);
    T? Dequeue();
    IReadOnlyCollection<T> DequeueChunk();
}
