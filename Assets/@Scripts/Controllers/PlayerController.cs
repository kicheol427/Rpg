using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;


[Serializable]
public class PlayerStat
{
    public int DataId;
    public float Hp;
    public float MaxHp;
    public float Atk;
    public float MoveSpeed;
}
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
    public PlayerStat Stat = new PlayerStat();

    #region Stat
    public override int DataId
    {
        get { return Managers.Game.AbilityInfo.PlayerDataId; }
        set { Managers.Game.AbilityInfo.PlayerDataId = Stat.DataId = value; }
    }
    public override float Hp
    {
        get { return Managers.Game.AbilityInfo.Hp; }
        set
        {
            if (value > MaxHp)
                Managers.Game.AbilityInfo.Hp = Stat.Hp = MaxHp;
            else
                Managers.Game.AbilityInfo.Hp = Stat.Hp = value;
        }
    }
    public override float MaxHp
    {
        get { return Managers.Game.AbilityInfo.MaxHp; }
        set { Managers.Game.AbilityInfo.MaxHp = Stat.MaxHp = value; }
    }
    public override float Atk
    {
        get { return Managers.Game.AbilityInfo.Atk; }
        set { Managers.Game.AbilityInfo.Atk = Stat.Atk = value; }
    }
    public override float MoveSpeed
    {
        get { return Managers.Game.AbilityInfo.MoveSpeed; }
        set { Managers.Game.AbilityInfo.MoveSpeed = Stat.MoveSpeed = value; }
    }
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    #endregion

    public override bool Init()
    {
        if(base.Init() == false)
            return false;

        ObjectType = Define.ObjectType.Player;

        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        //Skills.AddSkill<FireballSkill>(transform.position);
        Skills.AddSkill<EgoSword>(_indicator.position);

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
        Vector3 dir = _moveDir * MoveSpeed * Time.deltaTime; ;
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
        List<GemController> gems = Managers.Object.Gems.ToList();//coin변경

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
    public override void OnDamaged(BaseController attacker, float damage)
    {
        float totalDamage = 0;
        CreatureController cc = attacker as CreatureController;
        if (cc != null)
        {
            totalDamage = cc.Atk;
        }
        
        base.OnDamaged(attacker, totalDamage);

        Debug.Log($"OnDamaged ! {Hp}");
    }
    public override void InitCreatureStat(bool isFullHp = true)
    {
        // 현재 케릭터의 Stat 가져오기
        MaxHp = Managers.Game.AbilityInfo.MaxHp;
        Atk = Managers.Game.AbilityInfo.Atk;
        MoveSpeed = CreatureData.MoveSpeed * CreatureData.MoveSpeedRate;


        MaxHp *= MaxHpBonusRate;
        Atk *= AttackRate;
        MoveSpeed *= MoveSpeedRate;

        if (isFullHp == true)
            Hp = MaxHp;
    }

}
