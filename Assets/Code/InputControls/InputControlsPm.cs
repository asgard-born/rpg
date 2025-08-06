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
            public ReactiveCommand OnLeftMouseButtonDown;
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
            AddUnsafe(Observable.EveryUpdate().Subscribe(CheckForInput));
        }

        private void CheckForInput(long _)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _ctx.OnLeftMouseButtonDown?.Execute();
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                _ctx.OnScrollDown?.Execute();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                _ctx.OnScrollUp?.Execute();
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                _ctx.OnPressedKeyW?.Execute();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                _ctx.OnPressedKeyA?.Execute();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                _ctx.OnPressedKeyS?.Execute();
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                _ctx.OnPressedKeyD?.Execute();
            }
        }
    }
}