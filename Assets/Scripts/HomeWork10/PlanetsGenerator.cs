using Mechanics;
using Network;
using UnityEngine;


public class PlanetsGenerator : MonoBehaviour
{
    [SerializeField] private PlanetsInfo _planetsInfo;

    private void Awake()
    {
        GeneratePlanets();
    }

    public void GeneratePlanets()
    {
        foreach (var planet in _planetsInfo.Planets)
        {
            var planetObject = Instantiate(_planetsInfo.PlanetPrefab);
            var startPosition = new Vector3()
            {
                x = Mathf.Sin(planet.StartAngle) * planet.DistanceFromCenter + _planetsInfo.CenterPosition.x,
                y = _planetsInfo.CenterPosition.y,
                z = Mathf.Cos(planet.StartAngle) * planet.DistanceFromCenter + _planetsInfo.CenterPosition.z
            };
            planetObject.transform.localPosition = startPosition;
            planetObject.name = planet.Name;
            var planetOrbit = planetObject.GetComponent<PlanetOrbit>();
            planetOrbit.AroundPoint = _planetsInfo.CenterPosition;
            planetOrbit.CircleInSecond = planet.CircleInSecond;
            planetOrbit.RotationSpeed = planet.RotationSpeed;
        }
        
    }
}
