using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
	public float CoolTime { get; set; } = 1.0f;

	public RepeatSkill() : base(Define.SkillType.Repeat)
	{

	}

	public override bool Init()
	{
		base.Init();
		// TODO
		return true;
	}

	#region CoSkill
	Coroutine _coSkill;

	public override void ActivateSkill()
	{
		if (_coSkill != null)
			StopCoroutine(_coSkill);

		_coSkill = StartCoroutine(CoStartSkill());
	}

	protected abstract void DoSkillJob();//Áö¼Óµô

	protected virtual IEnumerator CoStartSkill()
	{
		WaitForSeconds wait = new WaitForSeconds(CoolTime);

		while (true)
		{
			DoSkillJob();

			yield return wait;
		}
	}
	#endregion
}
