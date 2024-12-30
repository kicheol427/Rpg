using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    GameObject _player;
    GameObject _joystick;

    void Start()
    {                                         //어드레써블 라벨
        Managers.Resource.LoadAllAsync<Object>("Preload", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            if (count == totalCount)
            {
                StartLoaded();
            }
        });

        //GameObject go = new GameObject("@Player");
        //_player.transform.parent = go.transform;
        //
        //_player.AddComponent<PlayerController>();
        


    }

    SpawningPool _spawningPool;

    Define.StageType _stageType;
    public Define.StageType StageType
    {
        get { return _stageType; }
        set
        {
            _stageType = value;

            if (_spawningPool != null)
            {
                switch (value)
                {
                    case Define.StageType.Normal:
                        _spawningPool.Stopped = false;
                        break;
                    case Define.StageType.Boss:
                        _spawningPool.Stopped = true;
                        break;
                    case Define.StageType.Home:
                        _spawningPool.Stopped = true;
                        break;
                }
            }
        }
    }
    void StartLoaded()
    {
        Managers.Data.Init();

        Managers.UI.ShowSceneUI<UI_GameScene>();

        _spawningPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        /*for(int i = 0;i < 10;i++)//TODO:: 소환 딜레이 
        {
            Vector3 randPos = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos, Random.Range(0, 2));
            
        }*/


        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("@Map.prefab");
        map.name = "@Map";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;

        
        foreach (var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Lvl : {playerData.level}, Hp{playerData.maxHp}");
        }

        Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers.Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers.Game.OnGemCountChanged += HandleOnGemCountChanged;

    }
    int _collectedGemCount = 0;
    int _remainingTotalGemCount = 10;

    public void HandleOnGemCountChanged(int gemCount)
    {
        _collectedGemCount++;

        if (_collectedGemCount == _remainingTotalGemCount)
        {
            Managers.UI.ShowPopup<UI_SkillSelectPopup>();
            _collectedGemCount = 0;
            _remainingTotalGemCount *= 2;
        }

        Managers.UI.GetSceneUI<UI_GameScene>().SetGemCountRatio((float)_collectedGemCount / _remainingTotalGemCount);

    }

    public void HandleOnKillCountChanged(int killCount)
    {
        Managers.UI.GetSceneUI<UI_GameScene>().SetKillCount(killCount);

        if (killCount == 5)//보스입장 설정
        {
            StageType = Define.StageType.Boss;

            Managers.Object.DespawnAllMonsters();

            Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.transform.position, 5, 10);

            Managers.Object.Spawn<MonsterController>(spawnPos, Define.BOSS_ID);
        }
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
    }

}
