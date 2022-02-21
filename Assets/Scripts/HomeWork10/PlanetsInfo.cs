using System;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/PlanetsInfo")]
public class PlanetsInfo : ScriptableObject
{
    public GameObject PlanetPrefab;
    public Vector3 CenterPosition;
    public PlanetInfo[] Planets;
}

