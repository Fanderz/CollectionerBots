using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Supermarket : MonoBehaviour
{
    [SerializeField] private List<Person> _persons;
    [SerializeField] private int _personCost;
    [SerializeField] private int _baseCost;
    [SerializeField] private SuperMarketSpawner _supermarketSpawner;

    private int _sendedPersonsToBuild = 0;
    private int _maxSendedPersonsToBuild = 1;
    private int _minPersonsCountToBuild = 1;

    private NavMeshPath _navMeshPath;
    private ChestsScanner _scanner;
    private PersonsSpawner _personSpawner;
    private ChestCollector _collector;
    private FlagSetter _flagSetter;

    public List<Person> Persons
    {
        get
        {
            return new List<Person>(_persons);
        }
    }

    public event Action SpawningPerson;
    public event Action SpawnNewBase;
    public event Action<int> PayingPersonSpawn;
    public event Action<int> PayingSupermarketSpawn;

    private void Awake()
    {
        if (_persons == null)
            _persons = new List<Person>();

        _navMeshPath = new NavMeshPath();

        _scanner = GetComponent<ChestsScanner>();
        _collector = GetComponent<ChestCollector>();
        _personSpawner = GetComponent<PersonsSpawner>();
        _flagSetter = GetComponent<FlagSetter>();

        if (_scanner.HaveSubscribers == false)
            _scanner.ChestFinded += SendPerson;

        if (_personSpawner.HaveSubscribers == false)
            _personSpawner.AddingPersonToBase += AddPerson;

        foreach (Person person in _persons)
            if (person.TryGetComponent(out SuperMarketBuilder builder))
                builder.StartBuilding += _supermarketSpawner.BuildSupermarket;
    }

    private void FixedUpdate()
    {
        if (_collector.Score >= _personCost && (_flagSetter.HaveFlag == false || _persons.Count <= _minPersonsCountToBuild))
        {
            PayingPersonSpawn?.Invoke(_personCost);
            SpawningPerson?.Invoke();
            _sendedPersonsToBuild = 0;
        }
        else
        {
            if (_collector.Score >= _baseCost && _sendedPersonsToBuild < _maxSendedPersonsToBuild && _persons.Count > _minPersonsCountToBuild)
            {
                SendPerson(_flagSetter.FlagGameObject);
                _flagSetter.PayingForBuilding -= PaySupermarketSpawn;
                _flagSetter.PayingForBuilding += PaySupermarketSpawn;
                _sendedPersonsToBuild++;
            }
        }
    }

    public void AddPerson(Person person)
    {
        _persons.Add(person);

        if (person.TryGetComponent(out ChestsPicker picker) && TryGetComponent(out ChestCollector _collector))
            picker.ChestDropped += _collector.CollectChest;

        if (person.TryGetComponent(out SuperMarketBuilder builder))
            builder.StartBuilding += _supermarketSpawner.BuildSupermarket;
    }

    public void RemovePerson(Person person) =>
        _persons.Remove(person);

    public void SetObjectsOnSpawn(SuperMarketSpawner supermarketSpawner, FlagSpawner flagSpawner)
    {
        if (_supermarketSpawner == null)
            _supermarketSpawner = supermarketSpawner;

        if (_flagSetter.FlagSpawner == null)
            _flagSetter.SetSpawner(flagSpawner);
    }

    private void SendPerson(GameObject target)
    {
        Person freePerson = _persons.Find(person => person.HaveTarget == false);

        if (freePerson != null)
        {
            if (freePerson.TryGetComponent(out NavMeshAgent agent))
            {
                agent.CalculatePath(target.transform.position, _navMeshPath);

                if (_navMeshPath.status != NavMeshPathStatus.PathInvalid)
                {
                    freePerson.SetTarget(target);
                }
            }
        }
    }

    private void PaySupermarketSpawn() =>
        PayingSupermarketSpawn?.Invoke(_baseCost);
}