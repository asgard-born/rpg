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
        private float _targetHeight;
        private float _verticalVelocity;

        public class Ctx
        {
            public CameraConfig Config;
            public LayerMask GroundLayer;
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
            _currentHeight = _targetHeight = _ctx.Transform.position.y;
            InitializeRx();
        }

        private void InitializeRx()
        {
            AddUnsafe(_ctx.OnPressedKeyW.Subscribe(_ => MoveHorizontal(Vector3.forward)));
            AddUnsafe(_ctx.OnPressedKeyS.Subscribe(_ => MoveHorizontal(Vector3.back)));
            AddUnsafe(_ctx.OnPressedKeyA.Subscribe(_ => MoveHorizontal(Vector3.left)));
            AddUnsafe(_ctx.OnPressedKeyD.Subscribe(_ => MoveHorizontal(Vector3.right)));
            AddUnsafe(_ctx.OnScrollUp.Subscribe(_ => AddVerticalStep(_ctx.Config.VerticalStep)));
            AddUnsafe(_ctx.OnScrollDown.Subscribe(_ => AddVerticalStep(-_ctx.Config.VerticalStep)));
            
            AddUnsafe(Observable.EveryLateUpdate().Subscribe(_ => UpdateVerticalMovement()));
        }

        private void MoveHorizontal(Vector3 direction)
        {
            Vector3 localDirection = _ctx.Transform.TransformDirection(direction);
            localDirection.y = 0;
            localDirection.Normalize();

            _ctx.Transform.position += localDirection * _ctx.Config.MovingSpeed * Time.deltaTime;
        }
        
        private void AddVerticalStep(float step)
        {
            _targetHeight += step;
        }
        
        private void UpdateVerticalMovement()
        {
            _currentHeight = Mathf.SmoothDamp(
                _currentHeight,
                _targetHeight,
                ref _verticalVelocity,
                _ctx.Config.VerticalSmoothTime
            );

            Vector3 newPosition = _ctx.Transform.position;
            newPosition.y = _currentHeight;
            _ctx.Transform.position = newPosition;
        }

        private Vector3 FindGroundPoint()
        {
            if (Physics.Raycast(_ctx.Transform.position, Vector3.down, out RaycastHit hit, _ctx.Config.RayDistance, _ctx.GroundLayer))
            {
                return hit.point;
            }

            return _ctx.Transform.position - Vector3.up * _currentHeight;
        }
    }
}