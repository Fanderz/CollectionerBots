using System;
using UnityEngine;

public class ChestsPicker : MonoBehaviour
{
    private Vector3 _chestPickedPosition = new Vector3(0, 0.3f, 0.4f);

    private Person _person;
    private Chest _pickedChest;
    private Animator _animator;

    public bool HaveChest { get; private set; }

    public event Action ChestPicked;
    public event Action<Chest> ChestDropped;

    private void Awake()
    {
        _person = GetComponent<Person>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Chest chest) && HaveChest == false)
        {
            if (chest.picked == false && _person.ChestTarget.position != null)
            {
                if (chest.transform.position == _person.ChestTarget.position)
                    PickChest(chest);
            }
        }
        else if (collider.TryGetComponent(out ChestCollector collectorBase) && HaveChest == true)
        {
            if (_pickedChest != null)
                DropChest(_pickedChest);
        }
    }

    private void PickChest(Chest chest)
    {
        _animator.SetTrigger("PickChest");
        chest.transform.SetParent(this.transform);
        chest.transform.localPosition = _chestPickedPosition;
        chest.SetPicked();
        HaveChest = true;
        _pickedChest = chest;

        ChestPicked?.Invoke();
        _animator.SetBool("WithChest", HaveChest);
    }

    private void DropChest(Chest chest)
    {
        _animator.SetTrigger("PickChest");
        _animator.SetFloat("Speed", 0);
        _person.ResetTarget();
        ChestDropped?.Invoke(chest);
        HaveChest = false;
        _animator.SetBool("WithChest", HaveChest);
    }
}
