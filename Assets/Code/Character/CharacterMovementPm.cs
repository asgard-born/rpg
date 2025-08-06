using Extensions;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace Character
{
    public class CharacterMovementPm : BaseDisposable
    {
        private readonly Ctx _ctx;
        private bool _isRunning;
        private const float CHECK_INTERVAL = 0.2f;

        public class Ctx
        {
            public NavMeshAgent NavMeshAgent;
            public float RunningSpeed;

            public ReactiveProperty<Vector3> WorldPointProperty;
            public ReactiveCommand<Vector3> OnTargetSelected;
            public ReactiveCommand OnTargetReached;
        }

        public CharacterMovementPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.NavMeshAgent.speed = _ctx.RunningSpeed;

            AddUnsafe(_ctx.WorldPointProperty.Subscribe(HandleTargetChanged));
            AddUnsafe(Observable.Interval(System.TimeSpan.FromSeconds(CHECK_INTERVAL)).Subscribe(CheckDestinationReached));
        }

        private void HandleTargetChanged(Vector3 target)
        {
            if (NavMesh.SamplePosition(target, out NavMeshHit hit, 5.0f, NavMesh.AllAreas))
            {
                _isRunning = true;
                _ctx.NavMeshAgent.SetDestination(hit.position);
                _ctx.OnTargetSelected?.Execute(hit.position);
            }
            else
            {
                Debug.Log("Pick another point");
                // Здесь может быть реактивная команда для обработки AudioPm'ом "не могу сюда пойти" и VisualEffectsPm'ом для крестика  
            }
        }

        private void CheckDestinationReached(long _)
        {
            if (!_isRunning) return;

            if (!_ctx.NavMeshAgent.pathPending && _ctx.NavMeshAgent.remainingDistance <= _ctx.NavMeshAgent.stoppingDistance
                                               && (!_ctx.NavMeshAgent.hasPath || _ctx.NavMeshAgent.velocity.sqrMagnitude == 0f))
            {
                _isRunning = false;
                _ctx.OnTargetReached?.Execute();
            }
        }
    }
}