using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private PlanetsGenerator planetsGenerator;
    [SerializeField] private PlanetSelection planetSelection;
    public SpaceshipsPool spaceshipsPool;
    public GameConfig gameConfig;

    internal Player FirstPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        FirstPlayer = new Player(0, Color.blue);
        StartNewGame();
    }

    public void StartNewGame()
    {
        ClearPrevData();

        planetsGenerator.SpawnPlanets();
        spaceshipsPool.Preload();

        var firstPlayerPlanet = planetsGenerator.GetRandomUniquePlanet();
        firstPlayerPlanet.SetPlanetAsInitial();
        firstPlayerPlanet.SetPlayer(FirstPlayer);
        
        InitAI();
    }

    private void ClearPrevData()
    {
        spaceshipsPool.ClearPrevData();
        planetSelection.Clear();
        FirstPlayer.Planets.Clear();
    }
    
    private void InitAI()
    {
        for (int i = 0; i < gameConfig.enemyAI.Count; i++)
        {
            var enemyPlayer = gameConfig.enemyAI[i];

            var enemyPlanet = planetsGenerator.GetRandomUniquePlanet();
            enemyPlanet.SetPlanetAsInitial();
            enemyPlanet.SetPlayer(enemyPlayer);
        }
    }
}