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
		//���� ��ȯ - �ݸ��� üũ �� �÷��̾�� ���˽� ������� �ɷ� ����& �ݸ��� ������ ���� ��ġ ��������
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

			yield return new WaitForSeconds(0.1f);//0.1�� ������
		}
	}
	protected override void OnDead()
	{
		base.OnDead();

		//Managers.Game.KillCount++; ų ��� ������ ����

		if (_coDotDamage != null)
			StopCoroutine(_coDotDamage);
		_coDotDamage = null;

		//GemController gc = Managers.Object.Spawn<GemController>(transform.position); ��� ����

		Managers.Object.Despawn(this);//������Ʈ ����
	}
}
