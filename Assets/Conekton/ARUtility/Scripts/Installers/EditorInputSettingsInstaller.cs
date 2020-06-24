using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.Input.Application
{
    [CreateAssetMenu(fileName = "EditorInputSettingsInstaller", menuName = "ARUtility/Installers/EditorInputSettingsInstaller")]
    public class EditorInputSettingsInstaller : ScriptableObjectInstaller<EditorInputSettingsInstaller>
    {
        [SerializeField] private EditorInputSettings _inputSettings = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_inputSettings);
        }
    }
}

