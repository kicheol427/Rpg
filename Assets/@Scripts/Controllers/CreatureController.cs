using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;
using static Define;

public abstract class CreatureController : BaseController
{
    public CreatureData CreatureData;

    protected bool isPlayDamagedAnim = false;
    public virtual int DataId { get; set; }
    public virtual float MoveSpeed { get; set; }
    public virtual float Hp { get; set; }
    public virtual float MaxHp { get; set; }
    public virtual float Atk { get; set; }
    public virtual float MaxHpBonusRate { get; set; } = 1;
    public virtual float AttackRate { get; set; } = 1;
    public virtual float MoveSpeedRate { get; set; } = 1;
    public SkillBook Skills { get; protected set; }
    public Collider2D _offset;
    public Rigidbody2D _rigidBody { get; set; }
    public Vector3 CenterPosition
    {
        get
        {
            return _offset.bounds.center;
        }
    }
    Define.CreatureState _creatureState = Define.CreatureState.Moving;
    public virtual Define.CreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }
    void Awake()
    {
        Init();
    }
    public virtual void InitCreatureStat(bool isFullHp = true)
    {
        float stageRate = Managers.Game.CurrentStageData.StageLevel;
        //보스, 플레이어빼고  엘리트, 몬스터만
        MaxHp = (CreatureData.MaxHp + (CreatureData.MaxHpBonus * Managers.Game.CurrentStageData.StageLevel)) * (CreatureData.HpRate + stageRate);
        Atk = (CreatureData.Atk + (CreatureData.AtkBonus * Managers.Game.CurrentStageData.StageLevel)) * CreatureData.AtkRate;
        Hp = MaxHp;
        MoveSpeed = CreatureData.MoveSpeed * CreatureData.MoveSpeedRate;
    }

    public void SetInfo(int creatureId)
    {
        DataId = creatureId;
        Dictionary<int, Data.CreatureData> dict = Managers.Data.CreatureDic;
        CreatureData = dict[creatureId];
        InitCreatureStat();

        Init();
    }
    public override bool Init()
    {
        base.Init();

        Skills = gameObject.GetOrAddComponent<SkillBook>();
        _offset = GetComponent<Collider2D>();

        return true;
    }
    public virtual void UpdateAnimation() { }
    public virtual void OnDamaged(BaseController attacker, float damage = 0)
    {
        if (Hp <= 0)
            return;

        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
        }
        Managers.Object.ShowDamageFont(CenterPosition, damage, 0, transform);
        if (gameObject.IsValid() || this.IsValid())
            StartCoroutine(PlayDamageAnimation());
    }
    protected virtual void OnDead()
    {
        _rigidBody.simulated = false;
        transform.localScale = new Vector3(1, 1, 1);
        CreatureState = CreatureState.Dead;
    }

    IEnumerator PlayDamageAnimation()
    {
        if (isPlayDamagedAnim == false)
        {
            isPlayDamagedAnim = true;

            yield return new WaitForSeconds(0.1f);


            if (Hp <= 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                switch (ObjectType)
                {
                    case ObjectType.Player:
                        OnDead();
                        break;
                }
            }
            isPlayDamagedAnim = false;
        }
    }
}
