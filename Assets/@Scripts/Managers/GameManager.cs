using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class GameData
{
	public string UserName = "Player";

	public int Coin = 0;
	public int Gem = 0;
	public int KillCount;

	public AbilityData AbilityInfo = new AbilityData();
	public StageData CurrentStage = new StageData();
	public List<Character> Characters = new List<Character>();
}
public class AbilityData
{
	public int PlayerDataId;
	public int Level;
	public float Hp;
	public float MaxHp;
	public float Atk;
	public float MoveSpeed;

	public void Clear()
	{
		Hp = 0f;
		MaxHp = 0f;
		Atk = 0f;
		MoveSpeed = 0f;
	}
}

public class GameManager
{
	public PlayerController Player { get { return Managers.Object?.Player; } }
	public GameData _gameData = new GameData();

	public AbilityData AbilityInfo
    {
        get { return _gameData.AbilityInfo; }
		set
        {
			_gameData.AbilityInfo = value;
        }
    }
	public StageData CurrentStageData
	{
		get { return _gameData.CurrentStage; }
		set { _gameData.CurrentStage = value; }
	}
	public List<Character> Characters
    {
        get { return _gameData.Characters; }
        set
        {
			_gameData.Characters = value;
        }
    }
    #region 재화

    public event Action<int> OnGemCountChanged;
	public int Gem
	{
		get { return _gameData.Gem; }
		set
		{
			_gameData.Gem = value;
			OnGemCountChanged?.Invoke(value);//젬 갱신코드->재료or돈
		}
	}

	#endregion

	#region 전투
	public event Action<int> OnKillCountChanged;

	public int KillCount
	{
		get { return _gameData.KillCount; }
		set
		{
			_gameData.KillCount = value; OnKillCountChanged?.Invoke(value);
		}
	}
	
    #endregion

    #region 이동
    Vector2 _moveDir;

	public event Action<Vector2> OnMoveDirChanged;

	public Vector2 MoveDir
	{
		get { return _moveDir; }
		set
		{
			_moveDir = value;
			OnMoveDirChanged?.Invoke(_moveDir);
		}
	}
    #endregion

}
