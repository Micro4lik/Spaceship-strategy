using System.Collections;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [SerializeField] private PlanetGraphics planetGraphics;

    public Player Ruler { get; private set; }

    public bool Selected { get; private set; }

    public int ShipCount
    {
        get => _shipCount;
        set
        {
            _shipCount = value;
            planetGraphics.UpdateText(value.ToString());
        }
    }

    private int _shipCount = 0;
    private PlanetState _planetState;
    private readonly int _maxShipCount = 50;

    public void Awake()
    {
        ShipCount = Random.Range(0, _maxShipCount);
        StartCoroutine(GenerateSpaceships());
    }

    public void SetPlanetAsInitial()
    {
        ShipCount = _maxShipCount;
    }

    public void Select()
    {
        Selected = true;
        planetGraphics.SetGraphicState(true);
    }

    public void UnSelect()
    {
        Selected = false;
        planetGraphics.SetGraphicState(false);
    }

    public void SetPlayer(Player player)
    {
        planetGraphics.SetPlanetColor(player.Color);
        Ruler = player;
        player.Planets.Add(this);
        _planetState = PlanetState.Сaptured;
    }

    public void LaunchSpaceshipsToAnotherPlanet(Planet planet)
    {
        if (planet == this) return;
        ShipCount /= 2;

        for (int i = 0; i < ShipCount; ++i)
        {
            if (ShipCount == 0) return;

            var spaceship = GameManager.Instance.spaceshipsPool.Spawn();
            spaceship.SetPlayer(Ruler);
            spaceship.SetActiveAndPosition(transform.position);
            spaceship.MoveToPlanet(planet);
        }
    }

    public void GetDamage(Player player)
    {
        if (ShipCount <= 0) SetPlayer(player);
        else if (Ruler != null && player.Id == Ruler.Id) ShipCount++;
        else ShipCount--;
    }

    private IEnumerator GenerateSpaceships()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (_planetState == PlanetState.Сaptured)
            {
                ShipCount++;
            }
        }
    }
}