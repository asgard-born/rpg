using Extensions;
using UniRx;

namespace CameraLogic
{
    public class CameraRoot : BaseDisposable
    {
        private readonly Ctx _ctx;

        public class Ctx
        {
            public ReactiveCommand OnPressedKeyW;
            public ReactiveCommand OnPressedKeyA;
            public ReactiveCommand OnPressedKeyS;
            public ReactiveCommand OnPressedKeyD;
        }

        public CameraRoot(Ctx ctx)
        {
            _ctx = ctx;
        }
    }
}