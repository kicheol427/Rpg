using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
    Vector2 _moveDir = Vector2.zero;
    float _speed = 5.0f;
        
    public Vector2 vector2
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    void Start()
    {
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;
    }
    void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        _moveDir = dir;
    }
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 dir = _moveDir * _speed * Time.deltaTime; ;
        transform.position += dir;
    }

    private void OnCollisionEnter2D(Collision2D collision)//데미지 설계
    {
        MonsterController target = collision.gameObject.GetComponent<MonsterController>();
        if (target == null)
            return;

    }
    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, damage);

        Debug.Log($"OnDamaged ! {Hp}");

        // TEMP 반사뎀.수정필요
        CreatureController cc = attacker as CreatureController;
        cc?.OnDamaged(this, 10000);
    }
}
