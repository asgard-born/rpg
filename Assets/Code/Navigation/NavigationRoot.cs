using Configs;
using Extensions;
using UniRx;
using UnityEngine;

namespace Navigation
{
    public class NavigationRoot : BaseDisposable
    {
        private readonly Ctx _ctx;

        public class Ctx
        {
            public Camera Camera;
            public NavigationConfig Config;
            
            public ReactiveCommand<Vector3> OnLeftMouseButtonDown;
            public ReactiveProperty<Vector3> RawMovePoint;
        }

        public NavigationRoot(Ctx ctx)
        {
            _ctx = ctx;
            
            InitializePm();
        }

        private void InitializePm()
        {
            AddUnsafe(new NavigationPm(new NavigationPm.Ctx
            {
                Config = _ctx.Config,
                Camera = _ctx.Camera,
                OnLeftMouseButtonDown = _ctx.OnLeftMouseButtonDown,
                RawMovePoint = _ctx.RawMovePoint
            }));
        }
    }
}