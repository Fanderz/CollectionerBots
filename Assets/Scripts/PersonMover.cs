using System;
using UnityEngine;
using UnityEngine.AI;

public class PersonMover : MonoBehaviour
{
    private const float DefaultSpeed = 3.5f;

    private Person _person;
    private ChestsPicker _picker;
    private NavMeshAgent _agent;

    public float PersonSpeed => _agent.speed;

    public event Action<bool, bool, float> AnimatorParameterChanged;

    private void Awake()
    {
        _person = GetComponent<Person>();
        _picker = GetComponent<ChestsPicker>();
        _agent = GetComponent<NavMeshAgent>();

        _person.TargetSetted += MoveToTarget;
        _picker.MovingWithObject += MoveToBase;
    }

    public void SetSpeed(float speed = DefaultSpeed) =>
        _agent.speed = speed;

    private void MoveToTarget(Chest target)
    {
        if (target.enabled)
        {
            _agent.speed = DefaultSpeed;
            _agent.SetDestination(target.transform.position);
            AnimatorParameterChanged?.Invoke(_picker.HaveChest, _picker.IsPicking, _agent.speed);
        }
    }

    private void MoveToBase()
    {
        _agent.speed = DefaultSpeed;
        _agent.SetDestination(_person.BasePosition);
        AnimatorParameterChanged?.Invoke(_picker.HaveChest, _picker.IsPicking, _agent.speed);
    }
}
