using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Define
{
    public enum UIEvent
    {
        Click,
    }
    public enum Scene
    {
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
        Boss,
        Env
    }
    public enum SkillType
    {
        None,
    }
    public enum StageType
    {
        Town,
        Normal,
        Boss,
    }
    public enum CreaturState
    {
        Idle,
        Moving,
        Skill,
        Dead,
    }
    public const int PLAYER_DATA_ID = 1;
}
