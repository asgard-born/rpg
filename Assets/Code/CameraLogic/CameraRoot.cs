using Configs;
using Cysharp.Threading.Tasks;
using Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CameraLogic
{
    public class CameraRoot : BaseDisposable
    {
        private readonly Ctx _ctx;
        private CameraView _view;

        public class Ctx
        {
            public CameraConfig CameraConfig;
            public Transform CameraSpawnPoint;
            public AssetReference ViewReference;

            public ReactiveCommand OnPressedKeyW;
            public ReactiveCommand OnPressedKeyA;
            public ReactiveCommand OnPressedKeyS;
            public ReactiveCommand OnPressedKeyD;
            public ReactiveCommand OnScrollDown;
            public ReactiveCommand OnScrollUp;
        }

        public CameraRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeAsync().Forget(ex => Debug.LogError($"{GetType().Name} Error: {ex}"));
        }

        private async UniTask InitializeAsync()
        {
            await CreateViewAsync();
            InitializePm();
        }

        private async UniTask CreateViewAsync()
        {
            var cameraPrefab = await LoadAndTrackPrefab<CameraView>(_ctx.ViewReference);
            _view = Object.Instantiate(cameraPrefab, _ctx.CameraSpawnPoint.position, _ctx.CameraSpawnPoint.rotation);
        }

        private void InitializePm()
        {
            AddUnsafe(new CameraMovementPm(new CameraMovementPm.Ctx
            {
                Config = _ctx.CameraConfig,
                Transform = _view.transform,
                OnPressedKeyW = _ctx.OnPressedKeyW,
                OnPressedKeyA = _ctx.OnPressedKeyA,
                OnPressedKeyS = _ctx.OnPressedKeyS,
                OnPressedKeyD = _ctx.OnPressedKeyD,
                OnScrollDown = _ctx.OnScrollDown,
                OnScrollUp = _ctx.OnScrollUp,
            }));
        }
    }
}