using UnityEngine;

public class PersonAnimator : MonoBehaviour
{
    private Animator _animator;
    private PersonMover _mover;
    private ChestsPicker _picker;
    private Person _person;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<PersonMover>();
        _picker = GetComponent<ChestsPicker>();
        _person = GetComponent<Person>();

        _mover.AnimatorParameterChanged += ChangeParameters;
        _person.AnimatorParameterChanged += ChangeParameters;
        _picker.PickingOrDroppingObject += ChangeParameters;
    }

    private void ChangeParameters(bool withChest, bool isPicking, float speed)
    {
        _animator.SetFloat(PlayerAnimatorData.Params.Speed, speed);
        _animator.SetBool(PlayerAnimatorData.Params.WithChest, withChest);

        if (isPicking)
            _animator.SetTrigger(PlayerAnimatorData.Params.PickChest);
    }
}
