using UnityEngine;
using System;

public class Person : MonoBehaviour
{
    [SerializeField] private Transform _base;

    public bool HaveTarget { get; private set; }

    private Chest _target;

    public Transform ChestTarget => _target.transform;
    public Vector3 BasePosition => _base.transform.position;

    public event Action<Chest> TargetSetted;

    private void Awake()
    {
        HaveTarget = false;
    }

    public void SetTargetChest(Chest target)
    {
        if (target != null)
        {
            _target = target;
            HaveTarget = true;
            target.SetBusy();

            TargetSetted?.Invoke(_target);
        }
    }

    public void ResetTarget() =>
        HaveTarget = false;
}
