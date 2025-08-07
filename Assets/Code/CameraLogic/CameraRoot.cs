using Configs;
using Cysharp.Threading.Tasks;
using Extensions;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CameraLogic
{
    public class CameraRoot : BaseDisposable
    {
        private readonly Ctx _ctx;
        private CameraView _view;
        private Camera _camera;

        public class Ctx
        {
            public CameraConfig CameraConfig;
            public AssetReference ViewReference;
            public LayerMask GroundLayer;
            public Transform CameraSpawnPoint;
            public Transform CharacterTransform;

            public ReactiveCommand<Camera> OnCameraInitialized;
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
            _ctx.OnCameraInitialized?.Execute(_camera);
        }

        private async UniTask CreateViewAsync()
        {
            var cameraPrefab = await LoadAndTrackPrefab<CameraView>(_ctx.ViewReference);
            _view = Object.Instantiate(cameraPrefab, _ctx.CameraSpawnPoint.position, _ctx.CameraSpawnPoint.rotation);

            _camera = _view.GetComponent<Camera>();

            if (_camera == null)
            {
                Debug.LogError("Camera component is null");
                _camera = _view.AddComponent<Camera>();
            }
        }

        private void InitializePm()
        {
            AddUnsafe(new CameraMovementPm(new CameraMovementPm.Ctx
            {
                Config = _ctx.CameraConfig,
                GroundLayer = _ctx.GroundLayer,
                CameraTransform = _view.transform,
                CharacterTransform = _ctx.CharacterTransform,
                
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