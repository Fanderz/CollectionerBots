using UnityEngine;

public class SuperMarketSpawner : MonoBehaviour
{
    [SerializeField] private Supermarket _supermarketPrefab;
    [SerializeField] private int _poolMaxSize;
    [SerializeField] private FlagSpawner _flagSpawner;
    [SerializeField] private GroundRebaker _groundRebaker;

    private BaseObjectPool<Supermarket> _pool;

    private void Awake()
    {
        _pool = new BaseObjectPool<Supermarket>(_poolMaxSize);
    }

    public void BuildSupermarket(Flag flag, SuperMarketBuilder builder)
    {
        Supermarket supermarket = _pool.Get(_supermarketPrefab);

        if (supermarket != null)
        {
            supermarket.transform.position = flag.transform.position;
            flag.gameObject.SetActive(false);

            if (builder.TryGetComponent(out Person person))
            {
                supermarket.SetObjectsOnSpawn(this, _flagSpawner);
                supermarket.AddPerson(person);
                person.transform.SetParent(supermarket.transform);
                person.ResetTarget();
                person.SetBasePosition(supermarket.transform);
            }

            _groundRebaker.Rebake();
        }
    }
}
