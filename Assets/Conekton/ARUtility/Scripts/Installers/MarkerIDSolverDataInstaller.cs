using UnityEngine;

using Zenject;

#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
using UnityEngine.XR.ARSubsystems;
#endif

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application
{
    [CreateAssetMenu(fileName = "MarkerIDDatabaseInstaller", menuName = "Installers/MarkerIDDatabaseInstaller")]
    public class MarkerIDSolverDataInstaller : ScriptableObjectInstaller<MarkerIDSolverDataInstaller>
    {
        [SerializeField] private MarkerIDDatabase _markerIDDatabase = null;
#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
        [SerializeField] private XRReferenceImageLibrary _XRReferenceImageLibrary = null;
#endif

        public override void InstallBindings()
        {
            Container.BindInstance(_markerIDDatabase);
#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
            Container.BindInstance(_XRReferenceImageLibrary);
#endif
        }
    }
}

