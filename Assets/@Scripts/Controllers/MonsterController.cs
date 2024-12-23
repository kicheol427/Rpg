using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
	public override bool Init()
	{
		if (base.Init())
			return false;

		//_animator = GetComponent<Animator>();
		ObjectType = Define.ObjectType.Monster;
		//CreatureState = Define.CreatureState.Moving;

		return true;
	}

	// Update is called once per frame
	void FixedUpdate()
    {
		PlayerController pc = Managers.Object.Player;
		if (pc == null)
			return;
		//몬스터 소환 - 콜리전 체크 후 플레이어와 접촉시 따라오는 걸로 변경& 콜리전 나오고 몬스터 위치 조정생각
		Vector3 dir = pc.transform.position - transform.position;
		Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * _speed;
		GetComponent<Rigidbody2D>().MovePosition(newPos);

		GetComponent<SpriteRenderer>().flipX = dir.x > 0;
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
		PlayerController target = collision.gameObject.GetComponent<PlayerController>();
		if (target == null)
			return;

		if (_coDotDamage != null)
			StopCoroutine(_coDotDamage);
		_coDotDamage = StartCoroutine(CoStartDotDamage(target));
	}

	public void OnCollisionExit2D(Collision2D collision)
	{
		PlayerController target = collision.gameObject.GetComponent<PlayerController>();
		if (target == null)
			return;

		if (_coDotDamage != null)
			StopCoroutine(_coDotDamage);
		_coDotDamage = null;
	}
	Coroutine _coDotDamage;
	public IEnumerator CoStartDotDamage(PlayerController target)
	{
		while (true)
		{
			target.OnDamaged(this, 2);

			yield return new WaitForSeconds(0.1f);//0.1초 데미지
		}
	}
	protected override void OnDead()
	{
		base.OnDead();

		//Managers.Game.KillCount++; 킬 대신 돈으로 변경

		if (_coDotDamage != null)
			StopCoroutine(_coDotDamage);
		_coDotDamage = null;

		//GemController gc = Managers.Object.Spawn<GemController>(transform.position); 드랍 설계

		Managers.Object.Despawn(this);//오브젝트 삭제
	}
}
