using Zenject;

using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Infrastructure;
#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
using UnityEngine.XR.ARSubsystems;
#endif


namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application
{
    public class ARMarkerIDSolverInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMarkerIDRepository>().To<MarkerIDRepository>().AsCached();

#if UNITY_EDITOR
            Container.Bind<IMarkerIDSolver<int>>().To<EditorMarkerIDSolver>().AsCached();
#elif UNITY_ANDROID && PLATFORM_NREAL
            Container.Bind<IMarkerIDSolver<int>>().To<NRMarkerIDSolver>().AsCached();
#elif UNITY_ANDROID && PLATFORM_OCULUS
            Container.Bind<IMarkerIDSolver<int>>().To<EditorMarkerIDSolver>().AsCached();
#elif UNITY_IOS || UNITY_ANDROID
            Container.Bind<IMarkerIDSolver<XRReferenceImage>>().To<MobileMarkerIDSolver>().AsCached();
#else
            Container.Bind<IMarkerIDSolver<int>>().To<EditorMarkerIDSolver>().AsCached();
#endif
        }
    }
}

