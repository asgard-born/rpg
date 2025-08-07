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

            public ReactiveProperty<Vector3> RawMovePoint;
            public ReactiveCommand<Vector3> OnStartMovingToTarget;
            public ReactiveCommand OnTargetReached;
        }

        public CharacterMovementPm(Ctx ctx)
        {
            _ctx = ctx;
            _ctx.NavMeshAgent.speed = _ctx.RunningSpeed;

            AddUnsafe(_ctx.RawMovePoint.Subscribe(TryMoveToPoint));
            AddUnsafe(Observable.Interval(System.TimeSpan.FromSeconds(CHECK_INTERVAL)).Subscribe(CheckDestinationReached));
        }

        private void TryMoveToPoint(Vector3 target)
        {
            if (NavMesh.SamplePosition(target, out NavMeshHit navHit, 5.0f, NavMesh.AllAreas))
            {
                _isRunning = true;
                _ctx.NavMeshAgent.SetDestination(navHit.position);
                _ctx.OnStartMovingToTarget?.Execute(navHit.position);
            }
            else
            {
                Debug.Log("Pick another point");
            }
        }

        private void CheckDestinationReached(long _)
        {
            if (!_isRunning) return;

            if (!_ctx.NavMeshAgent.pathPending && _ctx.NavMeshAgent.remainingDistance <= _ctx.NavMeshAgent.stoppingDistance
                                               && (!_ctx.NavMeshAgent.hasPath || _ctx.NavMeshAgent.velocity.sqrMagnitude == 0))
            {
                _isRunning = false;
                _ctx.OnTargetReached?.Execute();
            }
        }
    }
}