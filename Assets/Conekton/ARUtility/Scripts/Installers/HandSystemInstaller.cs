using UnityEngine;
using Zenject;

using Conekton.ARUtility.HandSystemUseCase.Domain;
using Conekton.ARUtility.HandSystemUseCase.Infrastructure;

namespace Conekton.ARUtility.HandSystemUseCase.Application
{
    public class HandSystemInstaller : MonoInstaller
    {
        [SerializeField] private bool _useEditorMode = false;

        public override void InstallBindings()
        {
            Container
                .Bind<IHandSystem>()
                .To<HandSystem>()
                .AsCached();

#if UNITY_EDITOR
            if (_useEditorMode)
            {
                Container
                    .BindInterfacesAndSelfTo<EditorHandProvider>()
                    .FromNewComponentOnNewGameObject()
                    .AsCached()
                    .NonLazy();
                return;
            }
#endif

#if UNITY_ANDROID && PLATFORM_OCULUS
            Container
                .BindInterfacesAndSelfTo<OculusHandProvider>()
                .FromNewComponentOnNewGameObject()
                .AsCached()
                .NonLazy();
#endif
        }
    }
}

