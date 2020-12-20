using UnityEngine;

using Zenject;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application
{
    [CreateAssetMenu(fileName = "MLReferenceImageLibraryInstaller", menuName = "ARUtility/Installers/MLReferenceImageLibraryInstaller")]
    public class MLReferenceImageLibraryInstaller : ScriptableObjectInstaller<MLReferenceImageLibraryInstaller>
    {
        [SerializeField] private MLReferenceImageLibrary _MLReferenceImageLibrary = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_MLReferenceImageLibrary);
        }
    }
}

