using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Conekton.ARUtility.UseCase.ARMarkerIDSolver.Application
{
    [System.Serializable]
    public struct MLReferenceImage
    {
        public string Name;
        public int Index;
        public string ID;
        public Texture2D TargetTexture;
    }

    [CreateAssetMenu(fileName = "MLReferenceImageLibrary", menuName = "ARUtility/MLReferenceImageLibrary")]
    public class MLReferenceImageLibrary : ScriptableObject
    {
        [SerializeField] private List<MLReferenceImage> _dataList = null;

        public IReadOnlyList<MLReferenceImage> DataList => _dataList;

        public int IndexOf(string name)
        {
            if (!_dataList.Any(d => d.ID == name))
            {
                return -1;
            }
            
            return _dataList
                .Where(d => d.ID == name)
                .Select(d => d.Index)
                .First();
        }
    }
}