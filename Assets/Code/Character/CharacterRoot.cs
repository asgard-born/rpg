using Cysharp.Threading.Tasks;
using Extensions;
using UniRx;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

namespace Character
{
    public class CharacterRoot : BaseDisposable
    {
        private Ctx _ctx;
        private CharacterView _view;

        public struct Ctx
        {
            public AssetReference ViewReference;
            public Transform SpawnPoint;
            public float RunningSpeed;

            public ReactiveProperty<Vector3> WorldPointProperty;
            public ReactiveCommand<Vector3> OnTargetSelected;
            public ReactiveCommand OnTargetReached;
        }

        public CharacterRoot(Ctx ctx)
        {
            _ctx = ctx;

            InitializeAsync().Forget(ex => Debug.LogError($"{GetType().Name} Error: {ex}"));
        }

        private async UniTask InitializeAsync()
        {
            await CreateViewAsync();
            InitializeMovements();
            InitializeAnimator();
        }

        private async UniTask CreateViewAsync()
        {
            CharacterView prefab = await LoadAndTrackPrefab<CharacterView>(_ctx.ViewReference);
            _view = Object.Instantiate(prefab, _ctx.SpawnPoint.position, _ctx.SpawnPoint.rotation);
        }

        private void InitializeMovements()
        {
            var navMeshAgent = _view.GetComponent<NavMeshAgent>();

            if (navMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent cannot be null!");
                navMeshAgent = _view.AddComponent<NavMeshAgent>();
            }

            AddUnsafe(new CharacterMovementPm(new CharacterMovementPm.Ctx
            {
                RunningSpeed = _ctx.RunningSpeed,
                NavMeshAgent = navMeshAgent,

                WorldPointProperty = _ctx.WorldPointProperty,
                OnTargetSelected = _ctx.OnTargetSelected,
                OnTargetReached = _ctx.OnTargetReached
            }));
        }

        private void InitializeAnimator()
        {
            var animator = _view.GetComponent<Animator>();

            if (animator == null)
            {
                Debug.LogError("NavMeshAgent cannot be null!");
                return;
            }

            AddUnsafe(new CharacterAnimatorPm(new CharacterAnimatorPm.Ctx
            {
                Animator = animator
            }));
        }
    }
}