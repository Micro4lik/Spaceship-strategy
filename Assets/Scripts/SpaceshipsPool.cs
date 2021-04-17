using System.Collections.Generic;
using UnityEngine;

public class SpaceshipsPool : MonoBehaviour
{
    [SerializeField] private int initPoolCapacity = 100;
    [SerializeField] private Spaceship spaceshipPrefab;

    private readonly Stack<Spaceship> _inactive = new Stack<Spaceship>();
    private int _nextId = 1;

    public void ClearPrevData()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Preload()
    {
        for (int i = 0; i < initPoolCapacity; i++)
        {
            var obj = Instantiate(spaceshipPrefab, transform);
            obj.name = spaceshipPrefab.name + " (" + _nextId++ + ")";
            obj.gameObject.SetActive(false);
            _inactive.Push(obj);
        }
    }

    public Spaceship Spawn()
    {
        Spaceship obj;
        if (_inactive.Count == 0)
        {
            obj = Instantiate(spaceshipPrefab, transform);
            obj.name = spaceshipPrefab.name + " (" + _nextId++ + ")";
        }
        else
        {
            obj = _inactive.Pop();

            if (obj == null)
            {
                return Spawn();
            }
        }

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Despawn(Spaceship obj)
    {
        obj.gameObject.SetActive(false);
        _inactive.Push(obj);
    }
}