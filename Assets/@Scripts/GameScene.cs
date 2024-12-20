using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject _playerPrefab;
    public GameObject _joystickPrefab;

    GameObject _player;
    GameObject _joystick;

    void Start()
    {
        _player = GameObject.Instantiate(_playerPrefab);
        _joystickPrefab = GameObject.Instantiate(_joystickPrefab);

        GameObject go = new GameObject("@Player");
        _player.transform.parent = go.transform;

        _player.AddComponent<PlayerController>();
        


    }

    void Update()
    {
        
    }
}
