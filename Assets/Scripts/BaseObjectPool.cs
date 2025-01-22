using UnityEngine;
using System.Collections.Generic;

public class BaseObjectPool<T> where T : MonoBehaviour, new()
{
    private int _poolMaxSize;

    private List<T> _objects;

    public BaseObjectPool(int maxSize)
    {
        _objects = new List<T>();
        _poolMaxSize = maxSize;
    }

    public T Get(T prefab, Transform parent = null)
    {
        T result = null;

        if (_objects.FindAll(obj => obj.gameObject.activeSelf == false).Count > 0)
        {
            result = _objects.Find(obj => obj.gameObject.activeSelf == false);

            if (result != null)
            {
                result.gameObject.SetActive(true);
                result.gameObject.transform.parent = parent;
            }
        }
        else
        {
            if (_objects.Count < _poolMaxSize)
            {
                result = Create(prefab, parent);
                _objects.Add(result);
            }
        }

        return result;
    }

    private T Create(T prefab, Transform parent = null)
    {
        return Object.Instantiate(prefab, parent);
    }
}
