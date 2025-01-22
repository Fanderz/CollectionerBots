using System;
using UnityEngine;

public class SuperMarketBuilder : MonoBehaviour
{
    private ChestsPicker _picker;
    private Person _person;

    public event Action<Flag, SuperMarketBuilder> StartBuilding;

    private void Awake()
    {
        _picker = GetComponent<ChestsPicker>();
        _person = GetComponent<Person>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Flag flag) && _picker.HaveChest == false && _person.Target.position == flag.transform.position)
            StartBuilding?.Invoke(flag, this);
    }
}