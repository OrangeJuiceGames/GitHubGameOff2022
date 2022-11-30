using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public event Action OnSpawnComplete;

    public void SetSpawning(bool isActive)
    {
        _IsSpawning = isActive;
    }

    public void Set()
    {

    }

    [SerializeField] Mob MobPrefab;
    private List<(MobType, int)> _percentChanceOfMobs;

    private int _mobPoolSize = 100;
    private Pool _mobPool;
    private bool _IsSpawning;
    private WTMK _Tool = WTMK.Instance;

    void Start()
    {
        _percentChanceOfMobs = SetMobPercentage();
        _mobPool = BuildMobPool(_mobPoolSize);
    }

    private Pool BuildMobPool(int poolSize)
    {
        List<IPoolable> mobs = new List<IPoolable>();
        while (poolSize > 0)
        {
            poolSize--;
            Mob mob = Instantiate(MobPrefab);
            mobs.Add(mob);

            mob.transform.position = transform.position;
        }

        return new Pool(mobs.ToArray());
    }

    public void SpawnMob(Vector3 pos, MobType mobType)
    {
        float randNum = _Tool.Rando.Next(0, 100);
        
        if (_mobPool.QueueCount > 0)
        {
            var mob = (Mob)_mobPool.GetPoolable();
            mob.Spawn(mobType, pos);
            OnSpawnComplete?.Invoke();
        }
        else
        {
            Debug.LogError("Failed to spawn");
        }
    }

    public void SpawnMob(Vector3 pos)
    {
        float randNum = _Tool.Rando.Next(0, 100);

        int sum = 0;
        MobType selectedMobType = MobType.Cat;

        for (int i = 0; i < _percentChanceOfMobs.Count; i ++)
        {
            sum += _percentChanceOfMobs[i].Item2;

            if (randNum <= sum)
            {
                selectedMobType = _percentChanceOfMobs[i].Item1;
                break;
            }
        }

        if(_mobPool.QueueCount > 0)
        {
            var mob = (Mob)_mobPool.GetPoolable();
            mob.Spawn(selectedMobType, pos);
            OnSpawnComplete?.Invoke();
        }
        else
        {
            Debug.LogError("Failed to spawn");
        }
    }

    private List<(MobType, int)> SetMobPercentage()
    {
        List<(MobType, int)> percentChances = new List<(MobType, int)>();
        percentChances.Add((MobType.CatWithHelmet, 60));
        percentChances.Add((MobType.Dog, 40));

        CheckPercentages(percentChances);

        return percentChances;
    }

    private void CheckPercentages(List<(MobType, int)> mobPercentages)
    {
        int total = 0;
        for (int i = 0; i < mobPercentages.Count; i++)
        {
            total += mobPercentages[i].Item2;
        }
        if (total != 100)
        {
            throw new System.Exception("Percent chances of mob do not add up to 100!");
        }
    }
    
}
