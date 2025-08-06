using Extensions;
using UnityEngine;

namespace Character
{
    public class CharacterAnimatorPm : BaseDisposable
    {
        private readonly Ctx _ctx;

        public class Ctx
        {
            public Animator Animator;
        }

        public CharacterAnimatorPm(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}