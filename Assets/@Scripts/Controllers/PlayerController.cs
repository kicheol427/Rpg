using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CreatureController
{
    Vector2 _moveDir = Vector2.zero;

    float EnvCollectDist { get; set; } = 1.0f;

    [SerializeField]
    Transform _indicator;
    [SerializeField]
    Transform _fireSocket;

    public Transform Indicator { get { return _indicator; } }
    public Vector3 FireSocket { get { return _fireSocket.position; } }
    public Vector3 ShootDir { get { return (_fireSocket.position - _indicator.position).normalized;} }

    public Vector2 vector2
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    public override bool Init()
    {
        if(base.Init() == false) return false;
        _speed = 5f;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        //StartProjectile();
        //StartEgoSword();

        return true;
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
        CollectEnv();
    }

    void MovePlayer()
    {
        Vector3 dir = _moveDir * _speed * Time.deltaTime; ;
        transform.position += dir;

        if (_moveDir != Vector2.zero)
        {
            _indicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);
        }
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    /*void CollectEnv()
    {
        List<GemController> gems =  Managers.Object.Gems.ToList();//리스트를 먼저 만든 후 밑 작업 시작 디스폰 함수시 문제발생위험
        foreach (GemController gem in gems)
        {
            Vector3 dir = gem.transform.position - transform.position;//젬의 위치 - 플레이어 위치
            if (dir.magnitude <= EnvCollectDist)//EnvCollectDist보다 작거나 같으면 습득
            {
                Managers.Game.Gem += 1;
                Managers.Object.Despawn(gem);
            }
        }
    }*/
    void CollectEnv()
    {
        List<GemController> gems = Managers.Object.Gems.ToList();

        float sqrCollectDist = EnvCollectDist * EnvCollectDist;

        var findGems = GameObject.Find("@Grid").GetComponent<GridController>().GatherObjects(transform.position, EnvCollectDist + 0.5f);

        foreach (var go in findGems)
        {
            GemController gem = go.GetComponent<GemController>();

            Vector3 dir = gem.transform.position - transform.position;
            if (dir.sqrMagnitude <= sqrCollectDist)
            {
                Managers.Game.Gem += 1;
                //ui갱신
                Managers.Object.Despawn(gem);
            }
        }
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
