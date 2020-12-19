using UnityEngine;
using Zenject;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Domain;
using Conekton.ARUtility.UseCase.ARMarkerDetector.Infrastructure;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Domain;
using Conekton.ARUtility.UseCase.ARMarkerIDSolver.Infrastructure;

#if (UNITY_IOS || UNITY_ANDROID) && !(PLATFORM_NREAL || PLATFORM_OCULUS)
using UnityEngine.XR.ARSubsystems;
#endif

namespace Conekton.ARUtility.UseCase.ARMarkerDetector.Application
{
    public class ARMarkerDetectorInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _editorMarkerDetectorPrefab = null;
        [SerializeField] private GameObject _MLARMarkerDetectorPrefab = null;

        [SerializeField] private bool _useEditorDetector = true;

        public override void InstallBindings()
        {
            Container
                .Bind<IMarkerIDRepository>()
                .To<MarkerIDRepository>()
                .AsCached();

#if UNITY_EDITOR
            if (_useEditorDetector)
            {
                BindEditorDependency();
                return;
            }
#endif

#if UNITY_ANDROID
    #if PLATFORM_NREAL
            BindNrealDependency();
    #elif PLATFORM_OCULUS
            BindEditorDependency();
    #else
            BindMobileDependency();
    #endif
#elif UNITY_IOS
            BindMobileDependency();
#elif PLATFORM_LUMIN
            BindMagicLeapDependency();
#else
            BindEditorDependency();
#endif
        }

        #region ### For detector ###

#if UNITY_IOS || UNITY_ANDROID
        private void BindMobileDependency()
        {
            Container
                .Bind<IMarkerIDSolver<XRReferenceImage>>()
                .To<MobileMarkerIDSolver>()
                .AsCached();
            
            Container
                .Bind<ARTrackedImageManagerProvider>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<MobileARMarkerDetector>()
                .AsCached();
        }
#endif

#if PLATFORM_NREAL
        private void BindNrealDependency()
        {
            Container
                .Bind<IMarkerIDSolver<int>>()
                .To<NRMarkerIDSolver>()
                .AsCached();
            
            Container
                .BindInterfacesAndSelfTo<NRARMarkerDetector>()
                .AsCached();
        }
#endif

#if PLATFORM_LUMIN
        private void BindMagicLeapDependency()
        {
            Container
                .Bind<IMarkerIDSolver<string>>()
                .To<MLMarkerIDSolver>()
                .AsCached();

            Container
                .BindInterfacesAndSelfTo<MLARMarkerDetector>()
                .FromComponentInNewPrefab(_MLARMarkerDetectorPrefab)
                .AsCached()
                .NonLazy();
        }
#endif

        private void BindEditorDependency()
        {
            Container
                .Bind<IMarkerIDSolver<int>>()
                .To<EditorMarkerIDSolver>()
                .AsCached();

            Container
                .Bind<IARMarkerDetector>()
                .To<EditorARMarkerDetector>()
                .FromComponentInNewPrefab(_editorMarkerDetectorPrefab)
                .AsCached()
                .NonLazy();
        }

        #endregion ### For detector ###
    }
}