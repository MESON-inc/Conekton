using UnityEngine;
using Zenject;
using Conekton.ARUtility.Player.Domain;
using Conekton.ARUtility.Player.Infrastructure;

namespace Conekton.ARUtility.Player.Application
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _editorPlayerPrefab = null;
        [SerializeField] private GameObject _nrPlayerPrefab = null;
        [SerializeField] private GameObject _mlPlayerPrefab = null;
        [SerializeField] private GameObject _mobilePlayerPrefab = null;
        [SerializeField] private GameObject _oculusPlayerPrefab = null;

        [Header("==== Camera control settings ====")] [SerializeField]
        private float _moveSpeed = 3f;
        [SerializeField] private float _rotateSpeed = 10f;
        [SerializeField] private float _boost = 2f;

        public override void InstallBindings()
        {
#if UNITY_ANDROID && PLATFORM_NREAL
            Container.Bind<IPlayer>().FromComponentInNewPrefab(_nrPlayerPrefab).AsCached().NonLazy();
#elif UNITY_ANDROID && PLATFORM_OCULUS
            Container.Bind<IPlayer>().FromComponentInNewPrefab(_oculusPlayerPrefab).AsCached().NonLazy();
#elif UNITY_IOS || UNITY_ANDROID
            Container.Bind<IPlayer>().FromComponentInNewPrefab(_mobilePlayerPrefab).AsCached().NonLazy();
#elif PLATFORM_LUMIN
            Container.Bind<IPlayer>().FromComponentInNewPrefab(_mlPlayerPrefab).AsCached().NonLazy();
#else
            Container.Bind<IPlayer>().FromComponentInNewPrefab(_editorPlayerPrefab).AsCached().NonLazy();
#endif

#if UNITY_EDITOR
            Container
                .BindInterfacesAndSelfTo<CameraController>()
                .AsCached()
                .WithArguments(_moveSpeed, _rotateSpeed, _boost);
#endif
        }
    }
}