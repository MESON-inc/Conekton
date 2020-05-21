using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application
{
    [System.Serializable]
    public struct MarkerIDDatabaseItem
    {
        public string Name;
        public int Index;
        public string ID;
    }

    [CreateAssetMenu(fileName = "MarkerIDDatabase", menuName = "ARUtility/MakerIDDatabase")]
    public class MarkerIDDatabase : ScriptableObject
    {
        [SerializeField] private MarkerIDDatabaseItem[] _dataList = null;

        public MarkerIDDatabaseItem[] DataList => _dataList;

        public bool TryGetItemByIndex(int index, out MarkerIDDatabaseItem item)
        {
            foreach (var d in _dataList)
            {
                if (d.Index == index)
                {
                    item = d;
                    return true;
                }
            }

            item = default;
            return false;
        }
    }
}

