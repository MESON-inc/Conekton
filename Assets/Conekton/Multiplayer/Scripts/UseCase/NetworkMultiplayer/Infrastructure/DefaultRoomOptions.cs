using System.Collections;
using System.Collections.Generic;
using Conekton.ARMultiplayer.NetworkMultiplayer.Domain;
using UnityEngine;

namespace Conekton.ARMultiplayer.NetworkMultiplayer.Infrastructure
{
    [CreateAssetMenu(fileName = "DefaultRoomOptions", menuName = "ARUtility/DefaultRoomOptions")]
    public class DefaultRoomOptions : ScriptableObject, IRoomOptions
    {
        [SerializeField] private string _roomName = "Room";
        [SerializeField] private int _playerTtl = 60000;
        [SerializeField] private int _emptyRoomTtl = 0;

        public string RoomName => _roomName;
        public int PlayerTtl => _playerTtl;
        public int EmptyRoomTtl => _emptyRoomTtl;
    }
}