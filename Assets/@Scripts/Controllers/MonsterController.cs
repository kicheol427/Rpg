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
	void Update()
    {
        
    }
}
