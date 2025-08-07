using CameraLogic;
using Character;
using Configs;
using Extensions;
using InputControls;
using Navigation;
using UniRx;
using UnityEngine;

namespace Root
{
    public class GameRoot : BaseDisposable
    {
        private readonly Ctx _ctx;

        #region Controls

        private ReactiveCommand<Vector3> _onLeftMouseButtonDown;
        private ReactiveCommand _onScrollDown;
        private ReactiveCommand _onScrollUp;
        private ReactiveCommand _onPressedKeyW;
        private ReactiveCommand _onPressedKeyA;
        private ReactiveCommand _onPressedKeyS;
        private ReactiveCommand _onPressedKeyD;

        #endregion

        private ReactiveProperty<Vector3> _rawMovePoint;
        private ReactiveCommand<Vector3> _onStartMovingToTarget;
        private ReactiveCommand _onTargetReached;
        private ReactiveCommand<Transform> _onCharacterInitialized;
        private ReactiveCommand<Camera> _onCameraInitialized;

        public class Ctx
        {
            public ResourcesConfig ResourcesConfig;
            public CharacterConfig CharacterConfig;
            public CameraConfig CameraConfig;

            public Transform CharacterSpawnPoint;
            public Transform CameraSpawnPoint;
            public NavigationConfig NavigationConfig;
        }

        public GameRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeControls();
            InitializeCharacter();
        }

        private void InitializeRx()
        {
            _onLeftMouseButtonDown = AddUnsafe(new ReactiveCommand<Vector3>());
            _onScrollDown = AddUnsafe(new ReactiveCommand());
            _onScrollUp = AddUnsafe(new ReactiveCommand());
            _onPressedKeyW = AddUnsafe(new ReactiveCommand());
            _onPressedKeyA = AddUnsafe(new ReactiveCommand());
            _onPressedKeyS = AddUnsafe(new ReactiveCommand());
            _onPressedKeyD = AddUnsafe(new ReactiveCommand());

            _rawMovePoint = AddUnsafe(new ReactiveProperty<Vector3>());
            _onStartMovingToTarget = AddUnsafe(new ReactiveCommand<Vector3>());
            _onTargetReached = AddUnsafe(new ReactiveCommand());
            _onCharacterInitialized = AddUnsafe(new ReactiveCommand<Transform>());
            _onCameraInitialized = AddUnsafe(new ReactiveCommand<Camera>());

            AddUnsafe(_onCharacterInitialized.Subscribe(InitializeCamera));
            AddUnsafe(_onCameraInitialized.Subscribe(InitializeNavigation));
        }

        private void InitializeControls()
        {
            AddUnsafe(
                new InputControlsPm(
                    new InputControlsPm.Ctx
                    {
                        OnLeftMouseButtonDown = _onLeftMouseButtonDown,
                        OnPressedKeyW = _onPressedKeyW,
                        OnPressedKeyA = _onPressedKeyA,
                        OnPressedKeyS = _onPressedKeyS,
                        OnPressedKeyD = _onPressedKeyD,
                        OnScrollUp = _onScrollUp,
                        OnScrollDown = _onScrollDown,
                    }));
        }

        private void InitializeCharacter()
        {
            AddUnsafe(new CharacterRoot(
                new CharacterRoot.Ctx
                {
                    ViewReference = _ctx.ResourcesConfig.CharacterViewReference,
                    CharacterSpawnPoint = _ctx.CharacterSpawnPoint,
                    RunningSpeed = _ctx.CharacterConfig.RunningSpeed,

                    RawMovePoint = _rawMovePoint,
                    OnCharacterInitialized = _onCharacterInitialized,
                    OnStartMovingToTarget = _onStartMovingToTarget,
                    OnTargetReached = _onTargetReached,
                }));
        }

        private void InitializeCamera(Transform characterTransform)
        {
            AddUnsafe(new CameraRoot(
                new CameraRoot.Ctx
                {
                    ViewReference = _ctx.ResourcesConfig.CameraViewReference,
                    CameraConfig = _ctx.CameraConfig,
                    GroundLayer = _ctx.NavigationConfig.GroundLayer,
                    CameraSpawnPoint = _ctx.CameraSpawnPoint,
                    CharacterTransform = characterTransform,

                    OnCameraInitialized = _onCameraInitialized,
                    OnPressedKeyW = _onPressedKeyW,
                    OnPressedKeyA = _onPressedKeyA,
                    OnPressedKeyS = _onPressedKeyS,
                    OnPressedKeyD = _onPressedKeyD,
                    OnScrollUp = _onScrollUp,
                    OnScrollDown = _onScrollDown,
                }));
        }

        private void InitializeNavigation(Camera camera)
        {
            AddUnsafe(new NavigationRoot(new NavigationRoot.Ctx
            {
                Config = _ctx.NavigationConfig,
                Camera = camera,
                NavigationMarker = _ctx.ResourcesConfig.NavigationMarkerPrefab,

                RawMovePoint = _rawMovePoint,
                OnLeftMouseButtonDown = _onLeftMouseButtonDown,
                OnStartMovingToTarget = _onStartMovingToTarget,
                OnTargetReached = _onTargetReached
            }));
        }
    }
}