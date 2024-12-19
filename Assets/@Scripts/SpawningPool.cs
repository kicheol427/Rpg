using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    float _spawnInterval = 0.1f;
    int _maxMonsterCount = 100;
    Coroutine _coIUpdateSpawningPool;

    public bool Stopped { get; set; } = false;

    private void Start()
    {
        //_coUpdateSpwaningPool = StartCoroutine(CoUptadeSpawningPool());
    }

    IEnumerator CoUpdateSpawningPool()
    {
        while (true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    void TrySpawn()
    {
        if (Stopped)
            return;
        
        
    }
}
