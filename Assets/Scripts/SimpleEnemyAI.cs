using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyAI : MonoBehaviour
{
    private Player _player;

    public void Init(Player player)
    {
        _player = player;
    }
}
