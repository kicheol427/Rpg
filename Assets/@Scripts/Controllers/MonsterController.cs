using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using static UnityEngine.GraphicsBuffer;

public class MonsterController : CreatureController
{
    #region State Pattern

    
    protected Animator _animator;
    public override void UpdateController()
    {
        base.UpdateController();

        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                UpdateIdle();
                break;
            case Define.CreatureState.Skill:
                UpdateSkill();
                break;
            case Define.CreatureState.Moving:
                UpdateMoving();
                break;
            case Define.CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateDead() { }

    #endregion
    public event Action<MonsterController> MonsterInfoUpdate;
    private void OnEnable()
    {
        if (DataId != 0)
            SetInfo(DataId);
    }
    public override bool Init()
	{
		if (base.Init())
			return false;

		_animator = GetComponent<Animator>();
		ObjectType = Define.ObjectType.Monster;
		CreatureState = Define.CreatureState.Moving;

		return true;
	}
    Vector3 _moveDir;

	void FixedUpdate()
    {
		if (CreatureState != Define.CreatureState.Moving)//state가 moving일때 이쪽으로 들어옴
			return;
		PlayerController pc = Managers.Object.Player;
		if (pc == null)
			return;
		//몬스터 소환 - 콜리전 체크 후 플레이어와 접촉시 따라오는 걸로 변경& 콜리전 나오고 몬스터 위치 조정생각
		_moveDir = pc.transform.position - transform.position;
		Vector3 newPos = transform.position + _moveDir.normalized * Time.deltaTime * MoveSpeed;
		GetComponent<Rigidbody2D>().MovePosition(newPos);

		GetComponent<SpriteRenderer>().flipX = _moveDir.x > 0;
    }
    public override void OnDamaged(BaseController attacker, float damage = 0)
    {
        float totalDmg = Managers.Game.Player.Atk;
        base.OnDamaged(attacker, totalDmg);
        InvokeMonsterData();
        if (ObjectType == ObjectType.Monster)
        {
            if (_coDotDamage == null)
            {
                _coDotDamage = StartCoroutine(CoStartDotDamage());
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        if (_coDotDamage != null)
			StopCoroutine(_coDotDamage);
		_coDotDamage = StartCoroutine(CoStartDotDamage());
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		PlayerController target = collision.gameObject.GetComponent<PlayerController>();
		if (target.IsValid() == false)
			return;
		if (this.IsValid() == false)
			return;

		if (_coDotDamage != null)
			StopCoroutine(_coDotDamage);
		_coDotDamage = null;
	}
	Coroutine _coDotDamage;
	public IEnumerator CoStartDotDamage()
	{
        float elapsed = 0;
        CreatureState = CreatureState.OnDamaged;

        while (true)
		{
            elapsed += Time.deltaTime;
            if (elapsed > KNOCKBACK_TIME)
                break;

            Vector3 dir = _moveDir * -1f;
            Vector2 nextVec = dir.normalized * KNOCKBACK_SPEED * Time.fixedDeltaTime;
            _rigidBody.MovePosition(_rigidBody.position + nextVec);

            yield return null;
		}
	}
	protected override void OnDead()
	{
		base.OnDead();

		Managers.Game.KillCount++;

		if (_coDotDamage != null)
			StopCoroutine(_coDotDamage);
		_coDotDamage = null;

		GemController gc = Managers.Object.Spawn<GemController>(transform.position); //드랍 설계

		Managers.Object.Despawn(this);//오브젝트 삭제
	}
    public void InvokeMonsterData()
    {
        if (this.IsValid() && gameObject.IsValid() && ObjectType != ObjectType.Monster)
        {
            MonsterInfoUpdate?.Invoke(this);
        }
    }
}
