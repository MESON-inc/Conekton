#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
using Zenject;

using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;
using UnityEngine.XR.ARSubsystems;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Infrastructure
{
    public class MobileMarkerIDSolver : IMarkerIDSolver<XRReferenceImage>
    {
        [Inject] private IMarkerIDRepository _idRepository = null;
        [Inject] private XRReferenceImageLibrary _XRReferenceImageLibrary = null;

        string IMarkerIDSolver<XRReferenceImage>.Solve(XRReferenceImage args)
        {
            int index = _XRReferenceImageLibrary.indexOf(args);
            return _idRepository.Solve(index);
        }
    }
}
#endif

