using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetsGenerator : MonoBehaviour
{
    [SerializeField] private GameConfig gameConfig;

    [SerializeField] private Planet planetPrefab;
    [SerializeField] private Transform planetParent;

    [SerializeField] private Camera mainCamera;
    private Vector3 _screenBounds;

    private List<Planet> _planets;
    private HashSet<int> _uniquePlanetsIds;

    private void Awake()
    {
        _screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, -30));
    }

    public void SpawnPlanets()
    {
        _planets = new List<Planet>();
        _uniquePlanetsIds = new HashSet<int>();

        foreach (Transform child in planetParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < gameConfig.planetsCount; i++)
        {
            var newPlanet = Instantiate(planetPrefab, planetParent);
            _planets.Add(newPlanet);
            newPlanet.name = "Planet " + i;
            newPlanet.transform.localScale = Vector3.one *
                                             Random.Range(gameConfig.planetMinSize,
                                                 gameConfig.planetMaxSize);

            SetNewPosition(newPlanet.transform, i);
        }
    }

    private void SetNewPosition(Transform newPlanet, int i)
    {
        var searchIteration = 0;

        while (searchIteration < 100)
        {
            searchIteration++;

            newPlanet.localPosition = FindNewPosition();
            if (NeedFindNewPositionToPlanet(newPlanet, i))
            {
                continue;
            }

            break;
        }

        if (searchIteration >= 100)
            Debug.LogWarning("Too many searching iterations!");
    }

    private Vector3 FindNewPosition()
    {
        return new Vector3(Random.Range(-_screenBounds.x, _screenBounds.x), 0,
            Random.Range(-_screenBounds.z, _screenBounds.z));
    }

    private bool NeedFindNewPositionToPlanet(Transform planet, int currentPlanetId)
    {
        for (int i = 0; i < _planets.Count; i++)
        {
            if (i == currentPlanetId) continue;

            if (Vector3.Distance(planet.localPosition, _planets[i].transform.position) <
                gameConfig.misDistanceToClosestPlanet)
            {
                Debug.Log($"Planet {i} is too close to {planet.name}, need find random position again");
                return true;
            }
        }

        return false;
    }

    public Planet GetRandomUniquePlanet()
    {
        var randomIndex = Random.Range(0, _planets.Count);

        if (_uniquePlanetsIds.Contains(randomIndex))
            GetRandomUniquePlanet();

        _uniquePlanetsIds.Add(randomIndex);

        return _planets[randomIndex];
    }
}