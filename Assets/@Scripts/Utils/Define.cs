using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
		BeginDrag,
		Drag,
		EndDrag,
	}

	public enum Scene
	{
		Unknown,
		DevScene,
		GameScene,
	}

	public enum Sound
	{
		Bgm,
		Effect,
	}

	public enum ObjectType
	{
		Player,
		Monster,
		Projectile,
		Env
	}

	public enum SkillType
	{
		None,
		Sequence,
		Repeat
	}

	public enum StageType
	{
		Normal,
		Boss,
	}
	public enum CreatureState
	{
		Idle,
		Moving,
		Skill,
		Dead,
	}
}
