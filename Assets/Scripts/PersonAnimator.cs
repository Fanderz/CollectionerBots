using UnityEngine;

public class PersonAnimator : MonoBehaviour
{
    private Animator _animator;
    private PersonMover _mover;
    private ChestsPicker _picker;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _mover = GetComponent<PersonMover>();
        _picker = GetComponent<ChestsPicker>();

        _mover.AnimatorParameterChanged += ChangeParameter;
        _picker.PickingOrDroppingObject += ChangeParameter;
    }

    private void ChangeParameter(bool withChest, bool isPicking, float speed = 0)
    {
        _animator.SetFloat(PlayerAnimatorData.Params.Speed, speed);
        _animator.SetBool(PlayerAnimatorData.Params.WithChest, withChest);

        if (isPicking)
            _animator.SetTrigger(PlayerAnimatorData.Params.PickChest);
    }
}
