using UnityEngine;
using UnityEngine.AI;

public class PersonMover : MonoBehaviour
{
    private Animator _animator;
    private Person _person;
    private ChestsPicker _picker;
    private NavMeshAgent _agent;

    private void Awake()
    {
        _person = GetComponent<Person>();
        _picker = GetComponent<ChestsPicker>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();

        _person.TargetSetted += MoveToTarget;
        _picker.ChestPicked += MoveToBase;
    }

    private void MoveToTarget(Chest target)
    {
        if (target.enabled)
        {
            _agent.SetDestination(target.transform.position);
            _animator.SetFloat("Speed", _agent.speed);
        }
    }

    private void MoveToBase() =>
        _agent.SetDestination(_person.BasePosition);
}
