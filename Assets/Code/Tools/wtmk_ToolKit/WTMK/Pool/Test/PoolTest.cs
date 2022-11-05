using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolTest : MonoBehaviour
{
    [SerializeField]
    private GameObject _PoolablePrefab;
    [SerializeField]
    private int _Size = 100;

    private Dood _Debug = Dood.Instance;
    private Pool _GameObjectPool;
    private PoolableGameObject _CurrentPoolable;

    private void Awake()
    {
        Dood.IsLogging = true;

        IPoolable[] _Pool = new IPoolable[_Size];

        for (int i = 0; i < _Pool.Length; i++)
        {
            GameObject clone = Instantiate(_PoolablePrefab);
            clone.gameObject.transform.position = Vector3.zero;
            _Pool[i] = clone.GetComponent<IPoolable>();

            clone.gameObject.SetActive(false);
        }

        _GameObjectPool = new Pool(_Pool);
    }

    public void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _CurrentPoolable = (PoolableGameObject)_GameObjectPool.GetPoolable();
            _CurrentPoolable.Spawn();

            _Debug.Log($"{_GameObjectPool.QueueCount}");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _CurrentPoolable.Kill();
            _Debug.Log($"{_GameObjectPool.QueueCount}");
        }
        */
    }
}
