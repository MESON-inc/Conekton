using UnityEngine;
using Zenject;

using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Infrastructure;

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Application
{
    public class ARMarkerDetectorInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _editorMarkerDetectorPrefab = null;

        public override void InstallBindings()
        {
#if UNITY_EDITOR
            BindEditorDependency();
#elif UNITY_ANDROID && PLATFORM_NREAL
            Container.BindInterfacesAndSelfTo<NRARMarkerDetector>().AsCached();
#elif UNITY_ANDROID && PLATFORM_OCULUS
            BindEditorDependency();
#elif UNITY_IOS || UNITY_ANDROID
            Container.Bind<ARTrackedImageManagerProvider>().AsSingle();
            Container.BindInterfacesAndSelfTo<MobileARMarkerDetector>().AsCached();
#else
            Container.Bind<IARMarkerDetector>().To<EditorARMarkerDetector>().AsCached();
#endif
        }

        private void BindEditorDependency()
        {
            Container
                .Bind<IARMarkerDetector>()
                .To<EditorARMarkerDetector>()
                .FromComponentInNewPrefab(_editorMarkerDetectorPrefab)
                .AsCached()
                .NonLazy();
        }
    }
}
