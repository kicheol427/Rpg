using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public interface ILoader<Key, Value>
{
	Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
	public Dictionary<int, Data.PlayerData> PlayerDic { get; private set; } = new Dictionary<int, Data.PlayerData>();
	public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.MonsterData> MonsterDic { get; private set; } = new Dictionary<int, Data.MonsterData>();
	public Dictionary<int, Data.CreatureData> CreatureDic { get; private set; } = new Dictionary<int, Data.CreatureData>();
	public Dictionary<int, Data.StageData> StageDic { get; private set; } = new Dictionary<int, Data.StageData>();
	public void Init()
	{
		//PlayerDic = LoadJson<Data.PlayerDataLoader, int, Data.PlayerData>("PlayerData.json").MakeDict();
		PlayerDic = LoadXml<Data.PlayerDataLoader, int, Data.PlayerData>("PlayerData.xml").MakeDict();
		SkillDic = LoadXml<Data.SkillDataLoader, int, Data.SkillData>("SkillData.xml").MakeDict();
        MonsterDic = LoadXml<Data.MonsterDataLoader, int, Data.MonsterData>("MonsterData.xml").MakeDict();
		CreatureDic = LoadXml<Data.CreatureDataLoader, int, Data.CreatureData>("CreatureData.xml").MakeDict();
		StageDic = LoadXml<Data.StageDataLoader, int, Data.StageData>("StageData.xml").MakeDict();
	}

	Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
	{
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
		return JsonUtility.FromJson<Loader>(textAsset.text);
	}

	Item LoadSingleXml<Item>(string name)
	{
		XmlSerializer xs = new XmlSerializer(typeof(Item));
		TextAsset textAsset = Managers.Resource.Load<TextAsset>(name);
		using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
			return (Item)xs.Deserialize(stream);
	}

	Loader LoadXml<Loader, Key, Item>(string name) where Loader : ILoader<Key, Item>, new()
	{
		XmlSerializer xs = new XmlSerializer(typeof(Loader));
		TextAsset textAsset = Managers.Resource.Load<TextAsset>(name);
		using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text)))
			return (Loader)xs.Deserialize(stream);
	}
}
