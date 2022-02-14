using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Unity.Profiling;

public class Example : MonoBehaviour
{
    public GameObject prefab;
    private List<GameObject> gameObjects = new List<GameObject>(100);

    private ProfilerMarker profilerMarker = new ProfilerMarker(ProfilerCategory.Scripts, "MyProfileMarker");

    List<int> intList = new List<int>();

    private void Start()
    {

    }

    private void Update()
    {
        
    }

}
