using System.Threading.Tasks;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Conekton.EditorUtility
{
    public static class DependencyResolver
    {
        private static readonly string[] _packageList =
        {
            "https://github.com/svermeulen/Extenject.git?path=/UnityProject/Assets/Plugins/Zenject",
        };

        public static async Task Resolve()
        {
            Debug.Log("============================");
            Debug.Log("Checking all dependent packages are in this project.");

            bool hasPackage = await CheckPackage();

            if (hasPackage)
            {
                Debug.Log("All dependent files are already installed.");
                Debug.Log("============================");
                return;
            }

            foreach (string package in _packageList)
            {
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
        }

        public static async Task<bool> CheckPackage()
        {
            foreach (string package in _packageList)
            {
                if (!(await CheckPackageExist(package)))
                {
                    return false;
                }
            }

            return true;
        }

        public static async Task<bool> CheckPackageExist(string targetPackage)
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
    }
}