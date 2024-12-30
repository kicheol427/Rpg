using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
	// 일종의 스킬 매니저
	public List<SkillBase> Skills { get; } = new List<SkillBase>();
	public List<SkillBase> RepeatedSkills { get; } = new List<SkillBase>();
	// 프리팹 만들까?
	public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();
    
    public T AddSkill<T>(Vector3 position, Transform parent = null) where T : SkillBase
    {
        System.Type type = typeof(T);

		if (type == typeof(EgoSword))//스폰하는 부분 넣기if안에
        {
			var egoSword = Managers.Object.Spawn<EgoSword>(position, Define.EGO_SWORD_ID);
			egoSword.transform.SetParent(parent);
			egoSword.ActivateSkill();

			Skills.Add(egoSword);
			RepeatedSkills.Add(egoSword);

			return egoSword as T;
		}
        else if (type == typeof(FireballSkill))
        {
			var fireball = Managers.Object.Spawn<FireballSkill>(position, Define.EGO_SWORD_ID);
            fireball.transform.SetParent(parent);
			fireball.ActivateSkill();

			Skills.Add(fireball);
			RepeatedSkills.Add(fireball);
			//return null;
			return fireball as T;
		}
		else if (type.IsSubclassOf(typeof(SequenceSkill)))
		{
			var skill = gameObject.GetOrAddComponent<T>();
			Skills.Add(skill); 
			SequenceSkills.Add(skill as SequenceSkill);

			return null;
			//return skill as T;
		}

		return null;
    }

	int _sequenceIndex = 0;

	public void StartNextSequenceSkill()
	{
		if (_stopped)
			return;
		if (SequenceSkills.Count == 0)
			return;
		
		SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
	}

	void OnFinishedSequenceSkill()
	{
		_sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;//스킬 순차적사용
		StartNextSequenceSkill();
	}

	bool _stopped = false;

	public void StopSkills()
	{
		_stopped = true;

		foreach (var skill in RepeatedSkills)
		{
			skill.StopAllCoroutines();
		}
	}
}
