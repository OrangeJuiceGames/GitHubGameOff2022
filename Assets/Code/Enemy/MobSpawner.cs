using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Cat CatPrefab;
    [SerializeField] Dog DogPrefab;

    private List<(MobTypes, int)> _percentChanceOfMobs;
    private float _distanceBetweenSpawns = 1f;
    private Vector3 lastSpawnLocation = new Vector3();

    void Start()
    {
        _percentChanceOfMobs = SetMobPercentage();
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

    private bool ShouldSpawnMob()
    {
        var distance = Vector3.Distance(lastSpawnLocation, transform.position);

        return distance >= _distanceBetweenSpawns;
    }

    private void SpawnMob()
    {
        float randNum = Random.Range(0, 100);

        int sum = 0;
        Mob selectedMob = null;

        for (int i = 0; i < _percentChanceOfMobs.Count; i ++)
        {
            sum += _percentChanceOfMobs[i].Item2;

            if (randNum <= sum)
            {
                selectedMob = GetMobFromType(_percentChanceOfMobs[i].Item1);
            }
        }

        Instantiate(selectedMob, transform.position, Quaternion.identity);
        lastSpawnLocation = transform.position;
    }

    private List<(MobTypes, int)> SetMobPercentage()
    {
        List<(MobTypes, int)> percentChances = new List<(MobTypes, int)>();
        percentChances.Add((MobTypes.Cat, 60));
        percentChances.Add((MobTypes.Dog, 40));

        CheckPercentages(percentChances);

        return percentChances;
    }

    private void CheckPercentages(List<(MobTypes, int)> mobPercentages)
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

    private Mob GetMobFromType(MobTypes mobType)
    {
        return mobType switch
        {
            MobTypes.Cat => CatPrefab,
            MobTypes.Dog => DogPrefab
        };
    }

}
