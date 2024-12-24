using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager
{
	public PlayerController Player { get; private set; }
	public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
	public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

	public T Spawn<T>(/*Vector3 position,*/ int templateID = 0) where T : BaseController
	{
		System.Type type = typeof(T);

		if (type == typeof(PlayerController))
		{
			// TODO : Data
			GameObject go = Managers.Resource.Instantiate("Player.prefab", pooling: true);
			//go.transform.position = position;

			PlayerController pc = go.GetOrAddComponent<PlayerController>();
			Player = pc;
			pc.Init();

			return pc as T;
		}
		else if (type == typeof(MonsterController))
		{
			//string name = "";

			/*switch (templateID)
			{
				case Define.GOBLIN_ID:
					name = "Goblin_01";
					break;
				case Define.SNAKE_ID:
					name = "Snake_01";
					break;
				case Define.BOSS_ID:
					name = "Boss_01";
					break;
			};*/

			string name = (templateID == 0 ? "Goblin_01" : "Snake_01");
			GameObject go = Managers.Resource.Instantiate(name + ".prefab", pooling: true);
			//go.transform.position = position;

			MonsterController mc = go.GetOrAddComponent<MonsterController>();
			Monsters.Add(mc);
			mc.Init();

			return mc as T;
		}
		return null;
	}
	public void Despawn<T>(T obj) where T : BaseController
	{
		System.Type type = typeof(T);

		if (type == typeof(PlayerController))
		{
			// ?
		}
		else if (type == typeof(MonsterController))
		{
			Monsters.Remove(obj as MonsterController);
			Managers.Resource.Destroy(obj.gameObject);
		}
		else if (type == typeof(ProjectileController))
		{
			Projectiles.Remove(obj as ProjectileController);
			Managers.Resource.Destroy(obj.gameObject);
		}
	}


}
