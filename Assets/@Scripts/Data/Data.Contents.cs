using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Data
{
    #region CreatureData
	public class CreatureData
    {
		public int DataId;
		public string Name;
		public string Prefab;
		public int Level;
		public float MaxHp;
		public float MaxHpBonus;
		public float HpRate;
		public float Atk;
		public float AtkBonus;
		public float AtkRate;
		public float MoveSpeed;
		public float MoveSpeedRate;
		public string Type;
	}
	[Serializable]
	public class CreatureDataLoader : ILoader<int, CreatureData>
	{
		public List<CreatureData> creatures = new List<CreatureData>();
		public Dictionary<int, CreatureData> MakeDict()
		{
			Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
			foreach (CreatureData creature in creatures)
				dict.Add(creature.DataId, creature);
			return dict;
		}
	}

	#endregion

	#region StageData
	[Serializable]
	public class StageData
	{
		public int StageIndex = 1;
		public string StageName;
		public int StageLevel = 1;
		public string MapName;
		public int StageSkill;

		public int FirstWaveCountValue;
		public int FirstWaveClearRewardItemId;
		public int FirstWaveClearRewardItemValue;

		public int SecondWaveCountValue;
		public int SecondWaveClearRewardItemId;
		public int SecondWaveClearRewardItemValue;

		public int ThirdWaveCountValue;
		public int ThirdWaveClearRewardItemId;
		public int ThirdWaveClearRewardItemValue;

		public int ClearReward_Gold;
		public int ClearReward_Exp;
		public string StageImage;
		public List<int> AppearingMonsters;
		//public List<WaveData> WaveArray;
	}
	public class StageDataLoader : ILoader<int, StageData>
	{
		public List<StageData> stages = new List<StageData>();

		public Dictionary<int, StageData> MakeDict()
		{
			Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
			foreach (StageData stage in stages)
				dict.Add(stage.StageIndex, stage);
			return dict;
		}
	}
	#endregion

	#region PlayerData
	public class PlayerData
	{
		[XmlAttribute]
		public int level;
		[XmlAttribute]
		public int maxHp;
		[XmlAttribute]
		public int attack;
		[XmlAttribute]
		public int totalExp;
	}

	[Serializable, XmlRoot("PlayerDatas")]
	public class PlayerDataLoader : ILoader<int, PlayerData>
	{
		[XmlElement("PlayerData")]
		public List<PlayerData> stats = new List<PlayerData>();

		public Dictionary<int, PlayerData> MakeDict()
		{
			Dictionary<int, PlayerData> dict = new Dictionary<int, PlayerData>();
			foreach (PlayerData stat in stats)
				dict.Add(stat.level, stat);
			return dict;
		}
	}
	#endregion

	/*#region MonsterData

	public class MonsterData
	{
		[XmlAttribute]
		public string name;
		[XmlAttribute]
		public string prefab;
		[XmlAttribute]
		public int level;
		[XmlAttribute]
		public int maxHp;
		[XmlAttribute]
		public int attack;
		[XmlAttribute]
		public float speed;
		[XmlAttribute]
		public string type;
        // DropData
        // - 일정 확률로
        // - 어떤 아이템을 (보석, 스킬 가차, 골드, 고기)
        // - 몇 개 드랍할지?

    }
    
	[Serializable, XmlRoot("MonsterDatas")]
    public class MonsterDataLoader : ILoader<int, MonsterData>
    {
        [XmlElement("MonsterData")]
        public List<MonsterData> stats = new List<MonsterData>();

        public Dictionary<int, MonsterData> MakeDict()
        {
            Dictionary<int, MonsterData> dict = new Dictionary<int, MonsterData>();
            foreach (MonsterData stat in stats)
                dict.Add(stat.level, stat);
            return dict;
        }
    }

    #endregion*/

    #region SkillData

    [Serializable]
	public class HitEffect
	{
		[XmlAttribute]
		public string type;
		[XmlAttribute]
		public int templateID;
		[XmlAttribute]
		public int value;
	}

	public class SkillData
	{
		[XmlAttribute]
		public int templateID;

		//[XmlAttribute(AttributeName = "type")]
		//public string skillTypeStr;
		//public Define.SkillType skillType = Define.SkillType.None;

		[XmlAttribute]
		public int nextID;
		public int prevID = 0;

		[XmlAttribute]
		public string prefab;

		
		[XmlAttribute]
		public int damage;

	}

	[Serializable, XmlRoot("SkillDatas")]
	public class SkillDataLoader : ILoader<int, SkillData>
	{
		[XmlElement("SkillData")]
		public List<SkillData> skills = new List<SkillData>();

		public Dictionary<int, SkillData> MakeDict()
		{
			Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
			foreach (SkillData skill in skills)
				dict.Add(skill.templateID, skill);

			return dict;
		}
	}

	#endregion
}