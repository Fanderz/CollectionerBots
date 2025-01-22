using System;
using UnityEngine;

public class PersonsSpawner : MonoBehaviour
{
    [SerializeField] private Person _prefab;
    [SerializeField] private int _poolMaxSize;

    private Supermarket _base;
    private BaseObjectPool<Person> _pool;

    public bool HaveSubscribers =>
        AddingPersonToBase != null;

    public event Action<Person> AddingPersonToBase;

    private void Awake()
    {
        _base = GetComponent<Supermarket>();
        _pool = new BaseObjectPool<Person>(_poolMaxSize);

        _base.SpawningPerson += SpawnPerson;
    }

    private void SpawnPerson()
    {
        Person person = _pool.Get(_prefab, transform);

        if (person != null)
        {
            person.transform.position = transform.position;
            AddingPersonToBase?.Invoke(person);
            person.SetBasePosition(transform);
        }
    }
}