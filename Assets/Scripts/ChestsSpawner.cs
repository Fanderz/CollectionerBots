using UnityEngine;
using System;
using System.Collections;
using Rand = UnityEngine.Random;

public class ChestsSpawner : MonoBehaviour
{
    [SerializeField] private Chest _chestPrefab;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private float _spawnDelay;
    [SerializeField] private ChestCollector _collector;

    private float xStartPosition = 28;
    private float zStartPosition = 28;

    private bool isEnabled = false;

    private WaitForSeconds _wait;
    private Coroutine _chestsCoroutine;

    private BaseObjectPool<Chest> _pool;

    public event Action<Chest> SpawnedChest;

    private void Awake()
    {
        _wait = new WaitForSeconds(_spawnDelay);
        _pool = new BaseObjectPool<Chest>(_poolMaxSize);
    }

    private void OnEnable()
    {
        isEnabled = true;
        _chestsCoroutine = StartCoroutine(SpawnChestsCoroutine());
    }

    private void OnDisable()
    {
        isEnabled = false;

        if (_chestsCoroutine != null)
            StopCoroutine(_chestsCoroutine);
    }

    private IEnumerator SpawnChestsCoroutine()
    {
        while (isEnabled)
        {
            Vector3 startPosition = new Vector3(Rand.Range(-xStartPosition, xStartPosition), transform.position.y, Rand.Range(-zStartPosition, zStartPosition));
            Chest chest = _pool.Get(_chestPrefab, transform);

            if (chest != null)
            {
                chest.transform.position = startPosition;
                SpawnedChest?.Invoke(chest);
            }

            yield return _wait;
        }
    }
}
