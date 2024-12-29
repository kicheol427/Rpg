using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSword : RepeatSkill
{
    [SerializeField]
    ParticleSystem[] _swingParticles;

	protected enum SwingType
	{
		First,
		Second,
		Third,
		Fourth
	}

	public EgoSword()
	{

	}

	protected override IEnumerator CoStartSkill()
	{
		WaitForSeconds wait = new WaitForSeconds(CoolTime);

		while (true)//레벨적용시 continue사용
		{
			SetParticles(SwingType.First);
			_swingParticles[(int)SwingType.First].gameObject.SetActive(true);
			yield return new WaitForSeconds(_swingParticles[(int)SwingType.First].main.duration);

			SetParticles(SwingType.Second);
			_swingParticles[(int)SwingType.Second].gameObject.SetActive(true);
			yield return new WaitForSeconds(_swingParticles[(int)SwingType.Second].main.duration);

			SetParticles(SwingType.Third);
			_swingParticles[(int)SwingType.Third].gameObject.SetActive(true);
			yield return new WaitForSeconds(_swingParticles[(int)SwingType.Third].main.duration);

			SetParticles(SwingType.Fourth);
			_swingParticles[(int)SwingType.Fourth].gameObject.SetActive(true);
			yield return new WaitForSeconds(_swingParticles[(int)SwingType.Fourth].main.duration);

			yield return wait;
		}
	}

	public override bool Init()
	{
		base.Init();

		return true;
	}

	void SetParticles(SwingType swingType)
	{
		if (Managers.Game.Player == null)
			return;

		Vector3 tempAngle = Managers.Game.Player.Indicator.transform.eulerAngles;
		transform.localEulerAngles = tempAngle;
		transform.position = Managers.Game.Player.transform.position;

		float radian = Mathf.Deg2Rad * tempAngle.z * -1;

		var main = _swingParticles[(int)swingType].main;
		main.startRotation = radian;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		MonsterController mc = collision.transform.GetComponent<MonsterController>();
		if (mc.IsValid() == false)
			return;

		mc.OnDamaged(Owner, Damage);
	}

	protected override void DoSkillJob()
	{

	}
}
