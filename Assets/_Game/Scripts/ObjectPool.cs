using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class ObjectPool<T> : IObjectPool<T> where T : MonoBehaviour
{
    private readonly Queue<T> _pool;
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly int _maxSize;
    private readonly DiContainer _container;

    public ObjectPool(T prefab, Transform parent, int maxSize, DiContainer container)
    {
        _prefab = prefab;
        _parent = parent;
        _maxSize = maxSize;
        _container = container;
        _pool = new Queue<T>(maxSize);
    }

    public async UniTask WarmupAsync(int count, CancellationToken ct = default)
    {
        for (int i = 0; i < count; i++)
        {
            var item = CreateInstance();
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
            await UniTask.Yield(ct);
        }
    }

    public T Get()
    {
        T item = _pool.Count > 0 ? _pool.Dequeue() : CreateInstance();
        item.gameObject.SetActive(true);
        return item;
    }

    public void Return(T item)
    {
        item.gameObject.SetActive(false);
        if (_pool.Count < _maxSize) 
            _pool.Enqueue(item);
    }

    private T CreateInstance()
    {
        var instance = Object.Instantiate(_prefab, _parent);
        _container.Inject(instance);
        return instance;
    }
}