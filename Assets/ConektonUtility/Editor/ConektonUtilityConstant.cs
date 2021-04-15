using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Conekton.EditorUtility
{
    public static class ConektonUtilityConstant
    {
        public enum PlatformType
        {
            Nreal,
            Oculus,
        }
        
        public static readonly string NREAL_SYMBOL = "PLATFORM_NREAL";
        public static readonly string OCULUS_SYMBOL = "PLATFORM_OCULUS";
    }
}