using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : IObjectPool
{
    private GameObject _prefab;
    private int _numberOfInstances;

    private Queue<GameObject> _instances = new Queue<GameObject>();

    public ObjectPool(GameObject prefab, int numberOfInstances)
    {
        _prefab = prefab;
        _numberOfInstances = numberOfInstances;

        for(int i = 0; i<numberOfInstances; ++i)
        {
            var instance = GameObject.Instantiate(prefab);
            instance.SetActive(false);

            _instances.Enqueue(instance);
        }
    }

    public GameObject Pull()
    {
        if (_instances.Count > 0)
        {
            var instance = _instances.Dequeue();
            instance.SetActive(true);
            return instance;
        }
        else
            return null;
    }

    public void Push(GameObject gameObject)
    {
        gameObject.SetActive(false);
        _instances.Enqueue(gameObject);
    }
}
