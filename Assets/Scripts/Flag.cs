using System;
using UnityEngine;

public class Flag : MonoBehaviour
{
    public event Action<Person> SupermarketBuilded;

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent(out Person person) && person.Target.position == transform.position)
        {
            SupermarketBuilded?.Invoke(person);
        }
    }
}
