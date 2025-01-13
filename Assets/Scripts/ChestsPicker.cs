using System;
using System.Collections.Generic;
using UnityEngine;

public class ChestsPicker : MonoBehaviour
{
    private Vector3 _chestPickedPosition = new Vector3(0, 0.3f, 0.4f);
    private List<Chest> _pickedChests = new List<Chest>();

    private Person _person;
    private PersonMover _mover;
    private Chest _pickedChest;

    public bool HaveChest { get; private set; }
    public bool IsPicking { get; private set; }

    public event Action<Chest> ChestDropped;
    public event Action<bool, bool, float> PickingOrDroppingObject;
    public event Action MovingWithObject;

    private void Awake()
    {
        _person = GetComponent<Person>();
        _mover = GetComponent<PersonMover>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent(out Chest chest) && HaveChest == false)
        {
            if (_person.ChestTarget.position != null)
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
        if (_pickedChests.Find(pickedChest => pickedChest == chest) == null)
        {
            _mover.SetSpeed(0);
            IsPicking = true;
            PickingOrDroppingObject?.Invoke(HaveChest, IsPicking, _mover.PersonSpeed);

            SetChestChild(chest);
            _mover.SetSpeed();
            IsPicking = false;
            MovingWithObject?.Invoke();
        }
    }

    private void DropChest(Chest chest)
    {
        _mover.SetSpeed(0);
        HaveChest = false;
        IsPicking = true;
        PickingOrDroppingObject?.Invoke(HaveChest, IsPicking, _mover.PersonSpeed);
        IsPicking = false;

        _person.ResetTarget();
        ChestDropped?.Invoke(chest);
        _pickedChests.Remove(chest);
    }

    private void SetChestChild(Chest chest)
    {
        chest.transform.SetParent(this.transform);
        chest.transform.localPosition = _chestPickedPosition;
        HaveChest = true;
        _pickedChest = chest;
        _pickedChests.Add(chest);
    }
}
