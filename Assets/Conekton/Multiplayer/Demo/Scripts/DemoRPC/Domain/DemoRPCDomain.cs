using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Conekton.ARMultiplayer.Avatar.Domain;

namespace Conekton.ARMultiplayer.Demo.RPC.Domain
{
    [System.Serializable]
    public struct DemoID
    {
        public int ID;

        static public bool operator ==(DemoID a, DemoID b)
        {
            return a.ID == b.ID;
        }

        static public bool operator !=(DemoID a, DemoID b)
        {
            return a.ID != b.ID;
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return (DemoID)obj == this;
        }
    }

    public interface IDemoRPCService
    {
        void Set(IAvatar avatar, DemoID demoID);
    }
}

