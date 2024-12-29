using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FireballSkill : RepeatSkill
{
	public FireballSkill()
	{

	}

	protected override void DoSkillJob()
	{
		if (Managers.Game.Player == null)
			return;

		Vector3 spawnPos = Managers.Game.Player.FireSocket;
		Vector3 dir = Managers.Game.Player.ShootDir;

		GenerateProjectile(1, Owner, spawnPos, dir, Vector3.zero);
	}

	//protected override void GenerateProjectile(int templateID, CreatureController owner, Vector3 startPos, Vector3 dir, Vector3 targetPos)
	//{
	//	ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, templateID);
	//	pc.SetInfo(templateID, owner, dir);
	//}
}
