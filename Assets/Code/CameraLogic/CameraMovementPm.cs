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
            public Transform CameraTransform;
            public Transform CharacterTransform;

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
            InitFirstPosition();
            InitializeRx();
        }

        private void InitFirstPosition()
        {
            _currentHeight = _targetHeight = _ctx.CameraTransform.position.y;
            UpdateVerticalMovement();
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
            Vector3 localDirection = _ctx.CameraTransform.TransformDirection(direction);
            localDirection.y = 0;
            localDirection.Normalize();

            Vector3 newPosition = _ctx.CameraTransform.position + localDirection * _ctx.Config.MovingSpeed * Time.deltaTime;

            Vector3 offsetXZ = newPosition - _ctx.CharacterTransform.position;
            offsetXZ.y = 0;

            if (offsetXZ.magnitude > _ctx.Config.MaxDistanceFromPlayerXZ)
            {
                offsetXZ = offsetXZ.normalized * _ctx.Config.MaxDistanceFromPlayerXZ;
                newPosition = _ctx.CharacterTransform.position + offsetXZ;
                newPosition.y = _ctx.CameraTransform.position.y;
            }

            _ctx.CameraTransform.position = newPosition;
        }

        private void AddVerticalStep(float step)
        {
            _targetHeight += step;
        }

        private void UpdateVerticalMovement()
        {
            _targetHeight = CalculateClampedHeight();

            _currentHeight = Mathf.SmoothDamp(
                _currentHeight,
                _targetHeight,
                ref _verticalVelocity,
                _ctx.Config.VerticalSmoothTime
            );

            Vector3 newPosition = _ctx.CameraTransform.position;
            newPosition.y = _currentHeight;
            _ctx.CameraTransform.position = newPosition;
        }

        private float CalculateClampedHeight()
        {
            if (Physics.SphereCast(
                    _ctx.CameraTransform.position + Vector3.up * _ctx.Config.CheckingSphereRadius,
                    _ctx.Config.CheckingSphereRadius,
                    Vector3.down,
                    out RaycastHit hit,
                    Mathf.Infinity,
                    _ctx.GroundLayer))
            {
                float relativeHeight = Mathf.Abs(hit.point.y - _targetHeight);
                relativeHeight = Mathf.Clamp(relativeHeight, _ctx.Config.MinHeight, _ctx.Config.MaxHeight);
                return hit.point.y + relativeHeight;
            }

            return _targetHeight;
        }
    }
}