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
    void StartLoaded()
    {
        Managers.Data.Init();

        _spawningPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        for(int i = 0;i < 10;i++)//TODO:: 소환 딜레이 
        {
            Vector3 randPos = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
            MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos, Random.Range(0, 2));
            
        }


        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("Map.prefab");
        map.name = "@Map";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;

        
        foreach (var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Lvl : {playerData.level}, Hp{playerData.maxHp}");
        }

        /*Managers.UI.ShowSceneUI<UI_GameScene>();
        
        
        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);


        
        

        Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers.Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers.Game.OnGemCountChanged += HandleOnGemCountChanged;*/

    }

    void Update()
    {
        
    }
}
