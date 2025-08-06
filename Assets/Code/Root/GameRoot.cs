using CameraLogic;
using Character;
using Configs;
using Extensions;
using InputControls;
using UniRx;
using UnityEngine;

namespace Root
{
    public class GameRoot : BaseDisposable
    {
        private readonly Ctx _ctx;

        private ReactiveProperty<Camera> _cameraProperty;
        private ReactiveProperty<Vector3> _worldPointProperty;

        #region Controls

        private ReactiveCommand _onLeftMouseButtonDown;
        private ReactiveCommand _onScrollDown;
        private ReactiveCommand _onScrollUp;
        private ReactiveCommand _onPressedKeyW;
        private ReactiveCommand _onPressedKeyA;
        private ReactiveCommand _onPressedKeyS;
        private ReactiveCommand _onPressedKeyD;

        #endregion

        private ReactiveCommand<Vector3> _onTargetSelected;
        private ReactiveCommand _onTargetReached;

        public class Ctx
        {
            public ResourcesConfig ResourcesConfig;
            public CharacterConfig CharacterConfig;
            public Transform SpawnPoint;
        }

        public GameRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeRx();
            InitializeControls();
            InitializeCharacter();
            InitializeCamera();
        }

        private void InitializeRx()
        {
            _cameraProperty = AddUnsafe(new ReactiveProperty<Camera>());
            _worldPointProperty = AddUnsafe(new ReactiveProperty<Vector3>());

            _onLeftMouseButtonDown = AddUnsafe(new ReactiveCommand());
            _onScrollDown = AddUnsafe(new ReactiveCommand());
            _onScrollUp = AddUnsafe(new ReactiveCommand());
            _onPressedKeyW = AddUnsafe(new ReactiveCommand());
            _onPressedKeyA = AddUnsafe(new ReactiveCommand());
            _onPressedKeyS = AddUnsafe(new ReactiveCommand());
            _onPressedKeyD = AddUnsafe(new ReactiveCommand());

            _onTargetSelected = AddUnsafe(new ReactiveCommand<Vector3>());
            _onTargetReached = AddUnsafe(new ReactiveCommand());
        }

        private void InitializeControls()
        {
            AddUnsafe(
                new InputControlsPm(
                    new InputControlsPm.Ctx
                    {
                        OnLeftMouseButtonDown = _onLeftMouseButtonDown,
                        OnScrollDown = _onScrollDown,
                        OnScrollUp = _onScrollUp,
                        OnPressedKeyW = _onPressedKeyW,
                        OnPressedKeyA = _onPressedKeyA,
                        OnPressedKeyS = _onPressedKeyS,
                        OnPressedKeyD = _onPressedKeyD
                    }));
        }

        private void InitializeCharacter()
        {
            AddUnsafe(new CharacterRoot(
                new CharacterRoot.Ctx
                {
                    ViewReference = _ctx.ResourcesConfig.CharacterViewReference,
                    SpawnPoint = _ctx.SpawnPoint,
                    RunningSpeed = _ctx.CharacterConfig.RunningSpeed,

                    WorldPointProperty = _worldPointProperty,
                }));
        }

        private void InitializeCamera()
        {
            AddUnsafe(new CameraRoot(
                new CameraRoot.Ctx
                {
                    OnPressedKeyW = _onPressedKeyW,
                    OnPressedKeyA = _onPressedKeyA,
                    OnPressedKeyS = _onPressedKeyS,
                    OnPressedKeyD = _onPressedKeyD
                }));
        }
    }
}