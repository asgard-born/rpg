using Configs;
using Extensions;
using UniRx;
using UnityEngine;

namespace Navigation
{
    public class NavigationPm : BaseDisposable
    {
        private readonly Ctx _ctx;

        public class Ctx
        {
            public NavigationConfig Config;
            public Camera Camera;
            
            public ReactiveCommand<Vector3> OnLeftMouseButtonDown;
            public ReactiveProperty<Vector3> RawMovePoint;
        }

        public NavigationPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(_ctx.OnLeftMouseButtonDown.Subscribe(NavigateThePoint));
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
    }
}