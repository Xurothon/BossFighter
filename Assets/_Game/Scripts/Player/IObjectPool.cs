using System.Threading;
using Cysharp.Threading.Tasks;

public interface IObjectPool<T> where T : class
{
    T Get();
    void Return(T item);
    UniTask WarmupAsync(int count, CancellationToken ct = default);
}