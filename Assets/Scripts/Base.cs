using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Person> _persons;
    [SerializeField] private float _searchDelay;

    private bool _isEnabled;
    private List<Chest> _finded;

    private WaitForSeconds _searchWait;
    private Coroutine _searchCoroutine;

    public List<Person> Persons 
    { 
        get 
        {
            return new List<Person>(_persons); 
        } 
    }

    private void Awake()
    {
        _searchWait = new WaitForSeconds(_searchDelay);
        _finded = new List<Chest>();
    }

    private void OnEnable()
    {
        _isEnabled = true;

        _searchCoroutine = StartCoroutine(SearchCoroutine());
    }

    private void OnDisable()
    {
        _isEnabled = false;

        if (_searchCoroutine != null)
            StopCoroutine(_searchCoroutine);
    }

    private IEnumerator SearchCoroutine()
    {
        while (_isEnabled)
        {
            Person freePerson = _persons.Find(person => person.HaveTarget == false);

            if (freePerson != null)
            {
                FindChests();

                if (_finded.Count > 0)
                    freePerson.SetTargetChest(_finded.Find(chest => chest.busy == false));
            }

            yield return _searchWait;
        }
    }

    private void FindChests()
    {
        _finded.Clear();

        Collider[] finded = Physics.OverlapSphere(transform.position, 50f);

        for (int i = 0; i < finded.Length; i++)
            if (finded[i].TryGetComponent(out Chest chest))
                _finded.Add(chest);
    }
}