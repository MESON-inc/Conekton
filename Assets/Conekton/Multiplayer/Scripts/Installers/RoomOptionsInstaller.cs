using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using UnityEngine;
using Zenject;

namespace Conekton.ARUtility.Input.Application
{
    [CreateAssetMenu(fileName = "RoomOptionsInstaller", menuName = "Installers/RoomOptionsInstaller")]
    public class RoomOptionsInstaller : ScriptableObjectInstaller<RoomOptionsInstaller>
    {
        [SerializeField] private ScriptableObject _roomOptions = null;

        public override void InstallBindings()
        {
            Container.BindInstance(_roomOptions as IRoomOptions);
        }
    }
}