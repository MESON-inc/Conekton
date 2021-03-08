using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;
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

            string arguments = $"/c mklink /D \"{folderName}\" \"{linkFolderName}\"";

            Process proc = new Process();
            proc.StartInfo.FileName = System.Environment.GetEnvironmentVariable("ComSpec");
            proc.StartInfo.Arguments = arguments;
            proc.StartInfo.Verb = "RunAs";
            proc.Start();
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