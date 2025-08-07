using Extensions;
using UniRx;
using UnityEngine;

namespace InputControls
{
    public class InputControlsPm : BaseDisposable
    {
        private readonly Ctx _ctx;

        public class Ctx
        {
            public ReactiveCommand<Vector3> OnLeftMouseButtonDown;
            public ReactiveCommand OnScrollDown;
            public ReactiveCommand OnScrollUp;
            public ReactiveCommand OnPressedKeyW;
            public ReactiveCommand OnPressedKeyA;
            public ReactiveCommand OnPressedKeyS;
            public ReactiveCommand OnPressedKeyD;
        }

        public InputControlsPm(Ctx ctx)
        {
            _ctx = ctx;
            AddUnsafe(Observable.EveryUpdate().Subscribe(_ => CheckForInput()));
        }

        private void CheckForInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ctx.OnLeftMouseButtonDown?.Execute(Input.mousePosition);
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                _ctx.OnScrollDown?.Execute();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _ctx.OnScrollUp?.Execute();
            }

            if (Input.GetKey(KeyCode.W))
            {
                _ctx.OnPressedKeyW?.Execute();
            }

            if (Input.GetKey(KeyCode.A))
            {
                _ctx.OnPressedKeyA?.Execute();
            }

            if (Input.GetKey(KeyCode.S))
            {
                _ctx.OnPressedKeyS?.Execute();
            }

            if (Input.GetKey(KeyCode.D))
            {
                _ctx.OnPressedKeyD?.Execute();
            }
        }
    }
}