using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CreatureController
{
    Vector2 _moveDir = Vector2.zero;

    float EnvCollectDist { get; set; } = 1.0f;

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

        StartProjectile();

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

                Managers.Object.Despawn(gem);
            }
        }

        Debug.Log($"SearchGems({findGems.Count}) TotalGems({gems.Count})");
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

    #region FireProjectile

    Coroutine _coFireProjectile;

    void StartProjectile()
    {
        if (_coFireProjectile != null)
            StopCoroutine(_coFireProjectile);

        _coFireProjectile = StartCoroutine(CoStartProjectile());
        
    }

    IEnumerator CoStartProjectile()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);//시간 data 시트 참조

        while (true)
        {
            ProjectileController pc = Managers.Object.Spawn<ProjectileController>(_fireSocket.position, 1);
            pc.SetInfo(1, this, (_fireSocket.position - _indicator.position).normalized);

            yield return wait;
        }
    }

    #endregion

    #region EgoSword
    EgoSword _egoSword;
    void StartEgoSword()
    {
        if (_egoSword.IsValid())
            return;

        _egoSword = Managers.Object.Spawn<EgoSword>(_indicator.position, Define.EGO_SWORD_ID);
        _egoSword.transform.SetParent(_indicator);

        _egoSword.ActivateSkill();
    }

    #endregion
}
