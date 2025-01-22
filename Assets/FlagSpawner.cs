using UnityEngine;

public class FlagSpawner : MonoBehaviour
{
    [SerializeField] private Flag _flagPrefab;
    [SerializeField] private int _poolMaxSize;

    private BaseObjectPool<Flag> _pool;

    private void Awake()
    {
        _pool = new BaseObjectPool<Flag>(_poolMaxSize);
    }

    public Flag SpawnFlag(Vector3 position, Transform parent)
    {
        Flag flag = _pool.Get(_flagPrefab, parent);

        if (flag != null)
        {
            flag.transform.position = position;
        }

        return flag;
    }
}
