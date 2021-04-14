using System.IO;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UEditorUtility = UnityEditor.EditorUtility;

namespace Conekton.EditorUtility
{
    public static class SymbolicLinkCreator
    {
        public static void Create(ConektonUtilityConstant.PlatformType type)
        {
            (string folderName, string linkFolderName) = GetFolderNames(type);

            if (Directory.Exists(folderName))
            {
                return;
            }

            if (!Directory.Exists(linkFolderName))
            {
                UEditorUtility.DisplayDialog("Target SDK folder is not found", "Please it locate under this project folder (not under the Assets folder).", "OK", "");
                Debug.LogWarning("This editor script will make a symbolic link of the target SDK folder under the Assets folder. Please put the target SDK folder under this project folder directly.");
                return;
            }

            switch (type)
            {
                case ConektonUtilityConstant.PlatformType.Nreal:
                    RemoveSDKFolder(ConektonUtilityConstant.PlatformType.Oculus);
                    break;

                case ConektonUtilityConstant.PlatformType.Oculus:
                    RemoveSDKFolder(ConektonUtilityConstant.PlatformType.Nreal);
                    break;
            }

            Process proc = new Process();
#if UNITY_EDITOR_WIN
            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            proc.StartInfo.Arguments = $"/c mklink /D \"{folderName}\" \"{linkFolderName}\"";
            proc.StartInfo.Verb = "RunAs";
            proc.Start();
#else
            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("SHELL");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.Verb = "RunAs";
            proc.Start();
            proc.StandardInput.WriteLine($"ln -s \"{linkFolderName}\" \"{folderName}\"");
            proc.StandardInput.WriteLine("exit");
            proc.StandardInput.Flush();
#endif
            proc.WaitForExit();
            proc.Close();

            AssetDatabase.Refresh();
        }

        public static void RemoveSDKFolder(ConektonUtilityConstant.PlatformType type)
        {
            (string folderName, string linkFolderName) = GetFolderNames(type);

            if (!Directory.Exists(folderName))
            {
                return;
            }

            Process proc = new Process();
#if UNITY_EDITOR_WIN
            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            proc.StartInfo.Arguments = $"/c rmdir \"{folderName}\"";
            proc.Start();
#else
            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("SHELL");
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.Start();
            proc.StandardInput.WriteLine($"unlink \"{folderName}\"");
            proc.StandardInput.WriteLine("exit");
            proc.StandardInput.Flush();
#endif
            
            proc.WaitForExit();
            proc.Close();
        }
        
        public static bool ExistSDKFolder(ConektonUtilityConstant.PlatformType type)
        {
            (string folderName, string linkFolderName) = GetFolderNames(type);

            return Directory.Exists(linkFolderName);
        }
        
        public static bool ExistSDK(ConektonUtilityConstant.PlatformType type)
        {
            (string folderName, string linkFolderName) = GetFolderNames(type);

            return Directory.Exists(folderName);
        }

        private static (string, string) GetFolderNames(ConektonUtilityConstant.PlatformType type)
        {
            string SDKName = "";

            switch (type)
            {
                case ConektonUtilityConstant.PlatformType.Nreal:
                    SDKName = "NRSDK";
                    break;

                case ConektonUtilityConstant.PlatformType.Oculus:
                    SDKName = "Oculus";
                    break;
            }

            string folderName = $"{Application.dataPath}/{SDKName}";
            string linkFolderName = $"{Application.dataPath}/../{SDKName}";

            return (folderName, linkFolderName);
        }
    }
}