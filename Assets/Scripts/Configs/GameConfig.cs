using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public int planetsCount = 10;
    public float planetMinSize = 1f;
    public float planetMaxSize = 3f;
    public float misDistanceToClosestPlanet = 3f;

    [Space] public List<Player> enemyAI = new List<Player>();
}