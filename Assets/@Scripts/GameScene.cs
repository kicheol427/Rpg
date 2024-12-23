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
        var player = Managers.Resource.Instantiate("Player.prefab");
        player.AddComponent<PlayerController>();

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
