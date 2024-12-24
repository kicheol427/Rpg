using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    GameObject _player;
    GameObject _joystick;

    void Start()
    {
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

    void StartLoaded()
    {
        var player = Managers.Object.Spawn<PlayerController>();

        for(int i = 0;i < 10;i++)//TODO:: ¼ÒÈ¯ µô·¹ÀÌ 
        {
            MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
            mc.transform.position = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        }


        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("Map.prefab");
        map.name = "@Map";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;

        /*Managers.Data.Init();
        
        Managers.UI.ShowSceneUI<UI_GameScene>();
        
        _spawningPool = gameObject.AddComponent<SpawningPool>();
        
        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);


        
        
        foreach (var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Lvl : {playerData.level}, Hp{playerData.maxHp}");
        }

        Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers.Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers.Game.OnGemCountChanged += HandleOnGemCountChanged;*/

    }

    void Update()
    {
        
    }
}
