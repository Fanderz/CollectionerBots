using UnityEngine;

public static class PlayerAnimatorData
{
    public static class Params
    {
        public static readonly int Speed = Animator.StringToHash(nameof(Speed));
        public static readonly int WithChest = Animator.StringToHash(nameof(WithChest));
        public static readonly int PickChest = Animator.StringToHash(nameof(PickChest));
    }
}
