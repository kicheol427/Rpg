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
		Melee,
		Projectile,
		Etc,
		Sequence,
		Repeat,
	}

	public enum StageType
	{
		Normal,
		Boss,
		Home,
	}
	public enum CreatureState
	{
		Idle,
		Moving,
		Skill,
		OnDamaged,
		Dead,
	}

	public static float KNOCKBACK_TIME = 0.1f;// 밀려나는시간
	public static float KNOCKBACK_SPEED = 10;  // 속도 
	public static float KNOCKBACK_COOLTIME = 0.5f;

	public const int GOBLIN_ID = 1;
	public const int SNAKE_ID = 2;
	public const int BOSS_ID = 3;

	public const int PLAYER_DATA_ID = 1;
	public const string EXP_GEM_PREFAB = "EXPGem.prefab";

    public const int FIREBALL_ID = 11;
    public const int EGO_SWORD_ID = 10;
}
