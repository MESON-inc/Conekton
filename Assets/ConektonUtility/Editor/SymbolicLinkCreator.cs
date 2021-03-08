using System.IO;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UEditorUtility = UnityEditor.EditorUtility;

namespace Conekton.EditorUtility
{
    public static class SymbolicLinkCreator
    {
        public enum PlatformType
        {
            Nreal,
            Oculus,
        }

        public static void Create(PlatformType type)
        {
            (string folderName, string linkFolderName) = GetFolderNames(type);

            if (Directory.Exists(folderName))
            {
                return;
            }

            switch (type)
            {
                case PlatformType.Nreal:
                    RemoveSDKFolder(PlatformType.Oculus);
                    break;

                case PlatformType.Oculus:
                    RemoveSDKFolder(PlatformType.Nreal);
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

        private static void RemoveSDKFolder(PlatformType type)
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

        private static (string, string) GetFolderNames(PlatformType type)
        {
            string SDKName = "";

            switch (type)
            {
                case PlatformType.Nreal:
                    SDKName = "NRSDK";
                    break;

                case PlatformType.Oculus:
                    SDKName = "Oculus";
                    break;
            }

            string folderName = $"{Application.dataPath}/{SDKName}";
            string linkFolderName = $"{Application.dataPath}/../{SDKName}";

            return (folderName, linkFolderName);
        }
    }
}