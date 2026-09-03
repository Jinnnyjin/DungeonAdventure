using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance { get; private set; }

    private Dictionary<GameObject, object> pools = new Dictionary<GameObject, object>();

    private void Awake()
    {
        // 혹시라도 씬에 중복 배치할 것을 대비
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // 컴포넌트를 가진 T가 아닌 컴포넌트 그 자체 T를 반환, 유니티 오브젝트로 쓰이는것들
    public T Get<T>(GameObject prefab) where T : Component
    {
        
        bool found = pools.TryGetValue(prefab, out var existingPool);
        ObjectPool<T> pool = (ObjectPool<T>) existingPool;

        if(!found)
        {
            pool = CreatePool<T>(prefab);
            pools[prefab] = pool;
        }

        return pool.Get() ;
    }

    public void Release<T>(GameObject prefab, T instance) where T : Component
    {
        bool found = pools.TryGetValue(prefab, out var existingPool);

        if (!found)
        {
            throw new InvalidOperationException($"프리팹 {prefab.name}에 대한 풀이 존재하지 않습니다.");
        }

        ObjectPool<T> pool = (ObjectPool<T>)existingPool;
        pool.Release(instance);
    }

    private ObjectPool<T> CreatePool<T>(GameObject prefab) where T : Component
    {
        return new ObjectPool<T>(
            createFunc: () => Instantiate(prefab).GetComponent<T>(),
            actionOnGet: (item) => item.gameObject.SetActive(true),
            actionOnRelease: (item) => item.gameObject.SetActive(false),
            maxSize: 20
        );
    }


}
