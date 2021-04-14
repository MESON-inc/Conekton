using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Conekton.EditorUtility
{
    public class ConektonUtilityWindow : EditorWindow
    {
        private bool _resolvedDependencies = false;
        private bool _isResolvingDependencies = false;

        private Texture2D _logo = null;

        private static bool IsNrealPlatform => DefinitionResolver.HasNeedDefinitionSymbols(ConektonUtilityConstant.PlatformType.Nreal);
        private static bool IsQuestPlatform => DefinitionResolver.HasNeedDefinitionSymbols(ConektonUtilityConstant.PlatformType.Oculus);
        private static bool IsAndroid => EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android;

        private static string LogoPath => $"{Application.dataPath}/ConektonUtility/Logo/Logo.png";

        [MenuItem("Conekton/Utility")]
        private static void Init()
        {
            ConektonUtilityWindow window = GetWindow<ConektonUtilityWindow>("Conekton Utility");

            if (window._logo == null)
            {
                window._logo = new Texture2D(1, 1);
                window._logo.LoadImage(File.ReadAllBytes(LogoPath));
                window._logo.filterMode = FilterMode.Point;
            }

            window.Show();

            window.CheckDependencies();
        }

        private async Task CheckDependencies()
        {
            Debug.Log("Checking all depend packages.");
            _resolvedDependencies = await DependencyResolver.CheckPackage();
        }

        private async void ResolveDependencies()
        {
            _isResolvingDependencies = true;
            await DependencyResolver.Resolve();
            await CheckDependencies();
            _isResolvingDependencies = false;
        }

        private void OnGUI()
        {
            if (_logo != null)
            {
                EditorGUILayout.Space(5);
                GUIStyle style = new GUIStyle(GUI.skin.label) {alignment = TextAnchor.MiddleCenter};
                GUILayout.Label(_logo, style);
            }

            DrawDependencyGUI();

            DrawLine();

            DrawCheckPlatform();
        }

        private void DrawLine()
        {
            EditorGUILayout.Space(10);
            Rect rect = EditorGUILayout.GetControlRect(false, 1);
            rect.height = 1;
            EditorGUI.DrawRect(rect, Color.gray);
            EditorGUILayout.Space(10);
        }

        private void DrawTitle(string title)
        {
            EditorGUILayout.Space(10);
            GUILayout.Label($"<<< {title} >>>", EditorStyles.boldLabel);
            EditorGUILayout.Space(5);
        }

        private void DrawDependencyGUI()
        {
            DrawTitle("Package dependencies");
            EditorGUI.BeginDisabledGroup(_isResolvingDependencies);
            if (GUILayout.Button("Check dependencies"))
            {
                CheckDependencies();
            }

            EditorGUI.BeginDisabledGroup(_resolvedDependencies);
            if (GUILayout.Button("Resolve dependencies"))
            {
                ResolveDependencies();
            }

            EditorGUI.EndDisabledGroup();
            EditorGUI.EndDisabledGroup();
        }

        private void DrawCheckPlatform()
        {
            DrawSymbolsGUI();
            if (IsAndroid)
            {
                DrawLine();

                DrawSDKLinkGUI();
            }
        }

        private void DrawSymbolsGUI()
        {
            DrawTitle("Choice platform");
            GUILayout.Label($"Current platform is **{EditorUserBuildSettings.activeBuildTarget}**.");
            EditorGUILayout.Space(10);
            if (!IsAndroid)
            {
                GUILayout.Label("Do you want to switch target?");

                if (GUILayout.Button("Switch target to Android"))
                {
                    EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
                }

                return;
            }

            if (IsNrealPlatform)
            {
                CustomLabel($"You choose Nreal platform", Color.white, Color.clear, 14);

                EditorGUILayout.Space(10);

                GUILayout.Label("Do you want to change platform?");

                if (GUILayout.Button("Change platform to Oculus"))
                {
                    DefinitionResolver.AddDefinition(ConektonUtilityConstant.PlatformType.Oculus);
                }

                return;
            }

            if (IsQuestPlatform)
            {
                CustomLabel($"You choose Oculus platform", Color.white, Color.clear, 14);

                EditorGUILayout.Space(10);

                GUILayout.Label("Do you want to change platform?");

                if (GUILayout.Button("Change platform to Nreal"))
                {
                    DefinitionResolver.AddDefinition(ConektonUtilityConstant.PlatformType.Nreal);
                }

                return;
            }

            GUILayout.Label("You have to choice platform.");
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Nreal"))
            {
                DefinitionResolver.AddDefinition(ConektonUtilityConstant.PlatformType.Nreal);
            }

            if (GUILayout.Button("Oculus"))
            {
                DefinitionResolver.AddDefinition(ConektonUtilityConstant.PlatformType.Oculus);
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        private void DrawSDKLinkGUI()
        {
            DrawTitle("SDK");

            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            if (target != BuildTarget.Android)
            {
                return;
            }

            if (IsNrealPlatform)
            {
                DrawTargetSDKLinkGUI(ConektonUtilityConstant.PlatformType.Nreal);
            }

            if (IsQuestPlatform)
            {
                DrawTargetSDKLinkGUI(ConektonUtilityConstant.PlatformType.Oculus);
            }
        }

        private void DrawTargetSDKLinkGUI(ConektonUtilityConstant.PlatformType type)
        {
            if (SymbolicLinkCreator.ExistSDK(type))
            {
                GUILayout.Label("Target SDK is imported.");

                if (!SymbolicLinkCreator.ExistSDKFolder(type))
                {
                    EditorGUILayout.Space(10);
                    CustomLabel($"You imported the SDK under the Assets folder directoly.", Color.yellow, Color.clear, 14);
                    GUILayout.Label("The SDK folder is expected as a symbolic link.");
                    GUILayout.Label("This may occur a problem changing platform.");
                }

                return;
            }

            string platformName = (type == ConektonUtilityConstant.PlatformType.Nreal) ? "Nreal" : "Oculus";
            string oppositePlatformName = (type == ConektonUtilityConstant.PlatformType.Nreal) ? "Oculus" : "Nreal";

            ConektonUtilityConstant.PlatformType oppositeType = (type == ConektonUtilityConstant.PlatformType.Nreal)
                ? ConektonUtilityConstant.PlatformType.Oculus
                : ConektonUtilityConstant.PlatformType.Nreal;
            if (SymbolicLinkCreator.ExistSDK(oppositeType))
            {
                CustomLabel($"{oppositePlatformName} SDK is already imported. You have to remove it.", Color.red, Color.clear, 14);
                GUILayout.Label("Do you want to remove it?");

                if (GUILayout.Button($"Remove {oppositePlatformName} SDK folder"))
                {
                    SymbolicLinkCreator.RemoveSDKFolder(oppositeType);
                }
            }

            if (SymbolicLinkCreator.ExistSDKFolder(type))
            {
                GUILayout.Label($"You have to import {platformName} SDK under the Assets folder.");

                if (GUILayout.Button($"Create {platformName} SDK symbolic link"))
                {
                    SymbolicLinkCreator.Create(type);
                }
            }
            else
            {
                CustomLabel($"{platformName} SDK is not found.", Color.red, Color.clear, 14);
                GUILayout.Label("Please import it to the same hierarchy of the Assets folder.");
                GUILayout.Label("This utility will create a symbolic link.");
            }
        }

        private void CustomLabel(string text, Color textColor, Color backColor, int fontSize, FontStyle fontStyle = FontStyle.Bold)
        {
            Color beforeBackColor = GUI.backgroundColor;
            GUIStyle guiStyle = new GUIStyle();
            GUIStyleState styleState = new GUIStyleState();
            styleState.textColor = textColor;
            styleState.background = Texture2D.whiteTexture;
            GUI.backgroundColor = backColor;
            guiStyle.normal = styleState;
            guiStyle.fontSize = fontSize;
            guiStyle.padding = new RectOffset(5, 5, 0, 0);
            GUILayout.Label(text, guiStyle);
            GUI.backgroundColor = beforeBackColor;
        }
    }
}