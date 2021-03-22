using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UEditorUtility = UnityEditor.EditorUtility;

namespace Conekton.EditorUtility
{
    public static class SetupConekton
    {
        private static readonly string NREAL_SYMBOL = "PLATFORM_NREAL";
        private static readonly string OCULUS_SYMBOL = "PLATFORM_OCULUS";

        [MenuItem("Conekton/Setup")]
        private static void Setup()
        {
            ResolveDependency();
            SwitchPlatform();
        }

        [MenuItem("Conekton/SwitchPlatform")]
        private static void SwitchPlatform()
        {
            bool platformAndroid = EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;

            if (!platformAndroid)
            {
                UEditorUtility.DisplayDialog("Cancel adding a symbol process", "You are not in Android platform. This process needs to be on only Android platform.", "OK", "");
                Debug.LogWarning("Aborted adding a symbol process. If you want to switch platform between Nreal and Oculus, you have to switch build target first.\nYou can switch platform between Nreal and Oculus with the menu that is in \"Conekton/SwitchPlatform\".");
                return;
            }
            
            int option = UEditorUtility.DisplayDialogComplex("Platform checking", "Which are you setting up for?", "Nreal", "Cancel", "Oculus");

            CheckDefinition(option);
            CreateSDKSymbolicLinkIfNeeded(option);
        }

        private static async Task<bool> CheckPackageExist(string targetPackage)
        {
            ListRequest listRequest = Client.List();

            while (listRequest.Status == StatusCode.InProgress)
            {
                await Task.Delay(100);
            }

            if (listRequest.Status == StatusCode.Success)
            {
                foreach (var package in listRequest.Result)
                {
                    if (package.packageId.Contains(targetPackage))
                    {
                        return true;
                    }
                }
            }
            else if (listRequest.Status == StatusCode.Failure)
            {
                Debug.LogError("It's not able to get a installed package list.");
            }

            return false;
        }

        private static async void ResolveDependency()
        {
            Debug.Log("============================");
            Debug.Log("Checking all dependent packages are in this project.");

            string package = "https://github.com/svermeulen/Extenject.git?path=/UnityProject/Assets/Plugins/Zenject";
            bool hasPackage = await CheckPackageExist(package);

            if (hasPackage)
            {
                Debug.Log("All dependent files are already installed.");
                Debug.Log("============================");
                return;
            }

            AddRequest request = Client.Add(package);

            while (!request.IsCompleted)
            {
                await Task.Delay(100);
            }

            if (request.Status == StatusCode.Success)
            {
                Debug.Log($"Installed: {request.Result.packageId}");
            }
            else if (request.Status == StatusCode.Failure)
            {
                Debug.LogError($"Failed to install with an error {request.Error.message}");
            }

            Debug.Log("Done resolving dependencies.");
            Debug.Log("============================");
        }

        private static void CheckDefinition(int option)
        {
            switch (option)
            {
                // Nreal
                case 0:
                    AddSymbolDefinition(NREAL_SYMBOL);
                    break;

                // Cancel
                case 1:
                    Debug.Log("Canceled setting up definition symbol. You may have to add a definition symbol yourself.");
                    break;

                case 2:
                    AddSymbolDefinition(OCULUS_SYMBOL);
                    break;
            }
        }

        private static void CreateSDKSymbolicLinkIfNeeded(int option)
        {
            switch (option)
            {
                // Nreal
                case 0:
                    SymbolicLinkCreator.Create(SymbolicLinkCreator.PlatformType.Nreal);
                    break;

                case 2:
                    SymbolicLinkCreator.Create(SymbolicLinkCreator.PlatformType.Oculus);
                    break;
            }
        }

        private static List<string> GetClearedSymbols()
        {
            List<string> result = new List<string>();
            string[] defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';');

            foreach (var d in defineSymbols)
            {
                if (d == NREAL_SYMBOL || d == OCULUS_SYMBOL)
                {
                    continue;
                }

                result.Add(d);
            }

            return result;
        }

        private static void AddSymbolDefinition(string symbol)
        {
            List<string> defineSymbols = GetClearedSymbols();
            defineSymbols.Add(symbol);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup,
                string.Join(";", defineSymbols));
        }
    }
}