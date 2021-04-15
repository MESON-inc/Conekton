using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Conekton.EditorUtility
{
    public static class DefinitionResolver
    {
        public static bool HasNeedDefinitionSymbols(ConektonUtilityConstant.PlatformType type)
        {
            string[] defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');
            
            string targetSymbol = (type == ConektonUtilityConstant.PlatformType.Nreal)
                ? ConektonUtilityConstant.NREAL_SYMBOL
                : ConektonUtilityConstant.OCULUS_SYMBOL;

            foreach (var d in defineSymbols)
            {
                if (d == targetSymbol)
                {
                    return true;
                }
            }

            return false;
        }
        
        public static void AddDefinition(ConektonUtilityConstant.PlatformType type)
        {
            switch (type)
            {
                case ConektonUtilityConstant.PlatformType.Nreal:
                    AddSymbolDefinition(ConektonUtilityConstant.NREAL_SYMBOL);
                    break;
                
                case ConektonUtilityConstant.PlatformType.Oculus:
                    AddSymbolDefinition(ConektonUtilityConstant.OCULUS_SYMBOL);
                    break;
            }
        }

        private static void AddSymbolDefinition(string symbol)
        {
            List<string> defineSymbols = GetClearedSymbols();
            defineSymbols.Add(symbol);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", defineSymbols));
        }

        private static List<string> GetClearedSymbols()
        {
            List<string> result = new List<string>();
            string[] defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');

            foreach (var d in defineSymbols)
            {
                if (d == ConektonUtilityConstant.NREAL_SYMBOL || d == ConektonUtilityConstant.OCULUS_SYMBOL)
                {
                    continue;
                }

                result.Add(d);
            }

            return result;
        }
    }
}