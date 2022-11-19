using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Mob MobPrefab;

    private List<(MobType, int)> _percentChanceOfMobs;
    private float _distanceBetweenSpawns = 1f;
    private Vector3 lastSpawnLocation = new Vector3();

    private int _mobPoolSize = 20;
    private Pool _mobPool;

    void Start()
    {
        _percentChanceOfMobs = SetMobPercentage();
        _mobPool = BuildMobPool(_mobPoolSize);
    }

    // Update is called once per frame
    void Update()
    {
        RandSpawnMob();
    }

    private void RandSpawnMob()
    {
        if (ShouldSpawnMob())
        {
            SpawnMob();
        }
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

    private bool ShouldSpawnMob()
    {
        var distance = Vector3.Distance(lastSpawnLocation, transform.position);

        return distance >= _distanceBetweenSpawns;
    }

    private void SpawnMob()
    {
        float randNum = Random.Range(0, 100);

        int sum = 0;
        MobType selectedMobType = MobType.Cat;

        for (int i = 0; i < _percentChanceOfMobs.Count; i ++)
        {
            sum += _percentChanceOfMobs[i].Item2;

            if (randNum <= sum)
            {
                selectedMobType = _percentChanceOfMobs[i].Item1;
            }
        }

        var mob = (Mob)_mobPool.GetPoolable();

        mob.ChangeMobType(selectedMobType);
        mob.transform.position = transform.position;
        mob.SetActive(true);

        lastSpawnLocation = transform.position;
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
