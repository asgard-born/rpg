using Configs;
using Extensions;
using UniRx;
using UnityEngine;

namespace Navigation
{
    public class NavigationPm : BaseDisposable
    {
        private readonly Ctx _ctx;
        private readonly Vector3 _spawnMarkerOffset = Vector3.up * .2f;
        private readonly Quaternion _spawnMarkerRotation = Quaternion.Euler(-90, 0, 0);

        private NavigationMarker _navigationMarker;

        public class Ctx
        {
            public NavigationConfig Config;
            public Camera Camera;
            public NavigationMarker NavigationMarker;

            public ReactiveCommand<Vector3> OnLeftMouseButtonDown;
            public ReactiveProperty<Vector3> RawMovePoint;
            public ReactiveCommand<Vector3> OnStartMovingToTarget;
            public ReactiveCommand OnTargetReached;
        }

        public NavigationPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.OnLeftMouseButtonDown.Subscribe(NavigateThePoint));
            AddUnsafe(_ctx.OnStartMovingToTarget.Subscribe(CreateNavigationMarker));
            AddUnsafe(_ctx.OnTargetReached.Subscribe(_ => TryDestroyMarker()));
        }

        private void NavigateThePoint(Vector3 mousePosition)
        {
            Ray ray = _ctx.Camera.ScreenPointToRay(mousePosition);
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _ctx.Config.GroundLayer))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(_ctx.Config.MaxSearchDistance);
            }

            _ctx.RawMovePoint.Value = targetPoint;
        }

        private void CreateNavigationMarker(Vector3 target)
        {
            TryDestroyMarker();
            _navigationMarker = Object.Instantiate(_ctx.NavigationMarker, target + _spawnMarkerOffset, _spawnMarkerRotation);
        }

        private void TryDestroyMarker()
        {
            if (_navigationMarker != null)
            {
                Object.Destroy(_navigationMarker.gameObject);
            }
        }
    }
}