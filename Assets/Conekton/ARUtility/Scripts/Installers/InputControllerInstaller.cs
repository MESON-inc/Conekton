using UnityEngine;
using Zenject;

using Conekton.ARUtility.Input.Domain;
using Conekton.ARUtility.Input.Infrastructure;

namespace Conekton.ARUtility.Input.Application
{
    public class InputControllerInstaller : MonoInstaller
    {
        [SerializeField] private EditorInputController _editorInputPrefab = null;

        [SerializeField] private bool _enable3DoF = false;

        [Header("==== for Debug ====")]
        [SerializeField] private bool _useEditorController = true;

        public override void InstallBindings()
        {
#if UNITY_EDITOR
            if (_useEditorController)
            {
                Container.Bind<IInputController>().FromComponentInNewPrefab(_editorInputPrefab).AsCached();
                Container.BindInterfacesAndSelfTo<PointerHandlerController>().AsCached().NonLazy();
                return;
            }
#endif

#if UNITY_ANDROID && PLATFORM_NREAL
            Container.Bind<IInputController>().To<NRInputController>().AsCached();
#elif UNITY_ANDROID && PLATFORM_OCULUS
            if (_enable3DoF)
            {
                Container.BindInterfacesAndSelfTo<OculusInputController3DoF>().AsCached();
            }
            else
            {
                Container.BindInterfacesAndSelfTo<OculusInputController>().AsCached();
            }
            Container.BindInterfacesAndSelfTo<PointerHandlerController>().AsCached().NonLazy();
#elif UNITY_IOS || UNITY_ANDROID
            Container.BindInterfacesAndSelfTo<MobileInputController>().AsCached();
            Container.BindInterfacesAndSelfTo<PointerHandlerController>().AsCached().NonLazy();
#elif PLATFORM_LUMIN
            Container.BindInterfacesAndSelfTo<MLInputController>().AsCached();
            Container.BindInterfacesAndSelfTo<PointerHandlerController>().AsCached().NonLazy();
#else
            Container.Bind<IInputController>().FromComponentInNewPrefab(_editorInputPrefab).AsCached();
            Container.BindInterfacesAndSelfTo<PointerHandlerController>().AsCached().NonLazy();
#endif
        }
    }
}

