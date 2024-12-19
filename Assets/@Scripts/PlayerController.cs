using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
    Vector2 _moveDir = Vector2.zero;

    float EnCollectDist { get; set; } = 1.0f;

    [SerializeField]
    Transform _indicator;

    public Transform Indicator { get { return _indicator; } }

    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _speed = 5.0f;
        //Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        return true;
    }
}
