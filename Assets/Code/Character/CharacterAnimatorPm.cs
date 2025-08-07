using Extensions;
using UniRx;
using UnityEngine;

namespace Character
{
    public class CharacterAnimatorPm : BaseDisposable
    {
        private readonly Ctx _ctx;
        
        private static readonly int IsRunning = Animator.StringToHash("IsRunning");
        private static readonly int IsIdle = Animator.StringToHash("IsIdle");

        public class Ctx
        {
            public Animator Animator;
            public ReactiveCommand<Vector3> OnStartMovingToTarget;
            public ReactiveCommand OnTargetReached;
        }

        public CharacterAnimatorPm(Ctx ctx)
        {
            _ctx = ctx;

            AddUnsafe(_ctx.OnStartMovingToTarget.Subscribe(_ => OnStartMovingToTarget()));
            AddUnsafe(_ctx.OnTargetReached.Subscribe(_ => OnTargetReached()));
        }

        private void OnStartMovingToTarget()
        {
            _ctx.Animator.SetBool(IsIdle, false);
            _ctx.Animator.SetBool(IsRunning, true);
        }

        private void OnTargetReached()
        {
            _ctx.Animator.SetBool(IsRunning, false);
            _ctx.Animator.SetBool(IsIdle, true);
        }
    }
}