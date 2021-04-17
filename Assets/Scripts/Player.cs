using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    public int Id => id;
    [SerializeField] private int id;
    public Color Color => color;
    [SerializeField] private Color color;

    [HideInInspector] public List<Planet> Planets = new List<Planet>();

    public Player(int id, Color color)
    {
        this.id = id;
        this.color = color;
    }
}