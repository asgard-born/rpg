using Configs;
using Extensions;
using UniRx;
using UnityEngine;

namespace CameraLogic
{
    public class CameraMovementPm : BaseDisposable
    {
        private readonly Ctx _ctx;
        private float _currentHeight;

        public class Ctx
        {
            public CameraConfig Config;
            public Transform Transform;

            public ReactiveCommand OnPressedKeyW;
            public ReactiveCommand OnPressedKeyA;
            public ReactiveCommand OnPressedKeyS;
            public ReactiveCommand OnPressedKeyD;
            public ReactiveCommand OnScrollUp;
            public ReactiveCommand OnScrollDown;
        }

        public CameraMovementPm(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
        }

        private void InitializeRx()
        {
            AddUnsafe(_ctx.OnPressedKeyW.Subscribe(_ => MoveHorizontal(Vector3.forward)));
            AddUnsafe(_ctx.OnPressedKeyS.Subscribe(_ => MoveHorizontal(Vector3.back)));
            AddUnsafe(_ctx.OnPressedKeyA.Subscribe(_ => MoveHorizontal(Vector3.left)));
            AddUnsafe(_ctx.OnPressedKeyD.Subscribe(_ => MoveHorizontal(Vector3.right)));

            AddUnsafe(_ctx.OnScrollUp.Subscribe(_ => MoveVertical(_ctx.Config.VerticalStep)));
            AddUnsafe(_ctx.OnScrollDown.Subscribe(_ => MoveVertical(-_ctx.Config.VerticalStep)));
        }

        private void MoveHorizontal(Vector3 direction)
        {
            _ctx.Transform.position += direction * _ctx.Config.MovingSpeed * Time.deltaTime;
        }

        private void MoveVertical(float step)
        {
            _ctx.Transform.position += Vector3.up * step;
        }
    }
}