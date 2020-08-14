using UnityEngine;
using Zenject;

using Conekton.ARUtility.HandSystemUseCase.Domain;
using Conekton.ARUtility.HandSystemUseCase.Infrastructure;

namespace Conekton.ARUtility.HandSystemUseCase.Application
{
    public class HandSystemInstaller : MonoInstaller
    {
        [SerializeField] private bool _useStubMode = false;

        public override void InstallBindings()
        {
            Container
                .Bind<IHandSystem>()
                .To<HandSystem>()
                .AsCached();

#if UNITY_EDITOR
            if (_useStubMode)
            {
                Container
                    .BindInterfacesAndSelfTo<StubHandProvider>()
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
#else
            Container
                .BindInterfacesAndSelfTo<StubHandProvider>()
                .FromNewComponentOnNewGameObject()
                .AsCached()
                .NonLazy();
#endif
        }
    }
}

