using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key}{count}/{totalCount}");
            if (count == totalCount)
            {
                StartLoaded();
            }
        });
    }

    SpawningPool _spawningPool;
    void StartLoaded()
    {
        Managers.Data.Init();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;
    }
}
