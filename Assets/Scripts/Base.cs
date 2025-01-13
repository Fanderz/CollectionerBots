using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private List<Person> _persons;

    private ChestsScanner _scanner;

    public List<Person> Persons 
    { 
        get 
        {
            return new List<Person>(_persons); 
        } 
    }

    private void Awake()
    {
        _scanner = GetComponent<ChestsScanner>();

        _scanner.ChestFinded += SendPerson;
    }

    private void SendPerson(Chest chest)
    {
        Person freePerson = _persons.Find(person => person.HaveTarget == false);

        if (freePerson != null)
        {
            if (_scanner.FindedChests.Count > 0)
                freePerson.SetTargetChest(chest);
        }
    }
}