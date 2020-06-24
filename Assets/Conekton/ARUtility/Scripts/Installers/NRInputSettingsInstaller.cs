using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.Input.Application
{
    [CreateAssetMenu(fileName = "NRInputSettingsInstaller", menuName = "ARUtility/Installers/NRInputSettingsInstaller")]
    public class NRInputSettingsInstaller : ScriptableObjectInstaller<NRInputSettingsInstaller>
    {
        [SerializeField] private NRInputSettings _inputSettings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_inputSettings);
        }
    }
}

