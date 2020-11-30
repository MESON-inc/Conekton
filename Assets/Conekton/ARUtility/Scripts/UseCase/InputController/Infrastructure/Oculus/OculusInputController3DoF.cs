#if UNITY_ANDROID && PLATFORM_OCULUS
using UnityEngine;
using Zenject;
using Conekton.ARUtility.Player.Domain;
using Conekton.ARUtility.Input.Domain;

namespace Conekton.ARUtility.Input.Infrastructure
{
    public class OculusInputController3DoF : OculusInputController
    {
        [Inject] private IPlayer _player = null;

        private readonly Vector3 VECTOR3_FORWARD = Vector3.forward;
        private readonly Vector3 PIVOT_OFFSET = new Vector3(0.1f, -0.35f, 0f);
        private readonly float HAND_DISTANCE = 0.3f;

        protected override Vector3 GetPosition(ControllerType type)
        {
            Vector3 pivot = _player.Position + PIVOT_OFFSET;
            Vector3 handPos = (_player.Rotation * VECTOR3_FORWARD) * HAND_DISTANCE;
            return pivot + handPos;
        }
    }
}
#endif