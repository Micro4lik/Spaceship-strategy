using UnityEngine;

public class Spaceship : MonoBehaviour
{
    [SerializeField] private SpaceshipGraphics spaceshipGraphics;
    [SerializeField] private new Rigidbody rigidbody;

    [SerializeField] private float speed = 5f;
    private bool _needMoving;
    private Planet _targetPlanet;
    
    private Player _ruler;

    public void SetPlayer(Player player)
    {
        _ruler = player;
        spaceshipGraphics.SetSpaceshipColor(player.Color);
    }

    public void SetActiveAndPosition(Vector3 position)
    {
        gameObject.SetActive(true);
        position.x += Random.Range(0f, 1f);
        position.z += Random.Range(0f, 1f);
        position.y = 1f;
        transform.position = position;
    }

    public void MoveToPlanet(Planet planet)
    {
        transform.LookAt(planet.transform.position);
        _targetPlanet = planet;
        _needMoving = true;
    }

    private void Update()
    {
        if (!_needMoving) return;
        var position = _targetPlanet.transform.position - transform.position;
        position = position.normalized * speed;
        rigidbody.velocity = position;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        var planet = collision.gameObject.GetComponent<Planet>();
        if (!planet || planet != _targetPlanet) return;
        planet.GetDamage(_ruler);
        GameManager.Instance.spaceshipsPool.Despawn(this);
    }
}