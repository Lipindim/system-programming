using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPool
{
    void Push(GameObject gameObject);

    GameObject Pull();
}
