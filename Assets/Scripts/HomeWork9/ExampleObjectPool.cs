using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleObjectPool : MonoBehaviour
{
    public GameObject prefab;
    ObjectPool objectPool;

    private List<GameObject> gameObjects = new List<GameObject>(2000);

    private void Start()
    {
        objectPool = new ObjectPool(prefab, 2000);
    }

    public void Update()
    {
        CreateObjects();
        //GameLogic
        DestroyObjects();
    }

    private void CreateObjects()
    {
        for (int i = 0; i < 2000; ++i)
            gameObjects.Add(objectPool.Pull());
    }

    private void DestroyObjects()
    {
        for (int i = 0; i < gameObjects.Count; ++i)
            objectPool.Push(gameObjects[i]);

        gameObjects.Clear();
    }
}
