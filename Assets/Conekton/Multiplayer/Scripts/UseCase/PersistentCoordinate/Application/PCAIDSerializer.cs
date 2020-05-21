using System.Text;
using UnityEngine;

using Conekton.ARMultiplayer.PersistentCoordinate.Domain;

namespace Conekton.ARMultiplayer.PersistentCoordinate.Application
{
    static public class PCAIDSerializer
    {
        static public object Deserialize(byte[] data)
        {
            PCAID result = new PCAID();
            result.ID = Encoding.UTF8.GetString(data);

            return result;
        }

        static public byte[] Serialize(object customType)
        {
            PCAID id = (PCAID)customType;

            return Encoding.UTF8.GetBytes(id.ID);
        }
    }
}

