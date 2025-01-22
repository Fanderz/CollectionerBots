using UnityEngine;
using System;

public class Person : MonoBehaviour
{
    [SerializeField] private Transform _base;

    private ChestsPicker _picker;
    private PersonMover _mover;
    private GameObject _target;

    public bool HaveTarget { get; private set; }

    public Transform Target => _target?.transform;
    public Vector3 BasePosition => _base.transform.position;

    public event Action<Vector3> TargetSetted;
    public event Action<bool, bool, float> AnimatorParameterChanged;

    private void Awake()
    {
        _picker = GetComponent<ChestsPicker>();
        _mover = GetComponent<PersonMover>();
    }

    public void SetTarget(GameObject target)
    {
        if (target != null)
        {
            _target = target;
            HaveTarget = true;
            
            if(target.TryGetComponent(out Chest chest))
                chest.SetBusy();

            TargetSetted?.Invoke(_target.transform.position);
        }
    }

    public void ResetTarget()
    {
        _mover.SetSpeed(0);
        AnimatorParameterChanged?.Invoke(_picker.HaveChest, _picker.IsPicking, _mover.PersonSpeed);
        HaveTarget = false;
    }

    public void SetBasePosition(Transform supermarket) =>
        _base = supermarket;
}
