using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

namespace gs.chef.vcontainer.editor
{
    public class PostBuildStep
    {
#if UNITY_IOS
        const string userTrackingUsageDescription = "Your data will only be used to deliver personalized ads to you.";

        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget buildTarget, string pathXCodeProject)
        {
            if (buildTarget == BuildTarget.iOS)
            {
                AddPListValues(pathXCodeProject);
                DisableBitcode(pathXCodeProject);
            }
        }

        private static void DisableBitcode(string pathToBuildProject)
        {
            string projectPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            PBXProject pbxProject = new PBXProject();
            pbxProject.ReadFromFile(projectPath);

            //Disabling Bitcode on all targets
            //Main
            string target = pbxProject.GetUnityMainTargetGuid();
            var result = pbxProject.GetBuildPropertyForAnyConfig(target, "ENABLE_BITCODE");
            if (!string.IsNullOrEmpty(result))
            {
                pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            }

            //Unity Tests
            target = pbxProject.TargetGuidByName(PBXProject.GetUnityTestTargetName());
            result = pbxProject.GetBuildPropertyForAnyConfig(target, "ENABLE_BITCODE");
            if (!string.IsNullOrEmpty(result))
            {
                pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            }
            result = pbxProject.GetBuildPropertyForAnyConfig(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES");
            if (!string.IsNullOrEmpty(result))
            {
                pbxProject.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            }

            //Unity Framework
            target = pbxProject.GetUnityFrameworkTargetGuid();
            result = pbxProject.GetBuildPropertyForAnyConfig(target, "ENABLE_BITCODE");
            if (!string.IsNullOrEmpty(result))
            {
                pbxProject.SetBuildProperty(target, "ENABLE_BITCODE", "NO");
            }

            result = pbxProject.GetBuildPropertyForAnyConfig(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES");
            if (!string.IsNullOrEmpty(result))
            {
                pbxProject.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");
            }

            pbxProject.WriteToFile(projectPath);
        }

        private static void AddPListValues(string pathXCodeProject)
        {
            string plistPath = pathXCodeProject + "/Info.plist";
            PlistDocument plistDocument = new PlistDocument();

            plistDocument.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict plistElementDict = plistDocument.root;

            bool isChanged = false;

            if (plistElementDict["NSUserTrackingUsageDescription"] == null)
            {
                plistElementDict.SetString("NSUserTrackingUsageDescription", userTrackingUsageDescription);
                isChanged = true;
            }

            if (plistElementDict["ITSAppUsesNonExemptEncryption"] == null)
            {
                plistElementDict.SetBoolean("ITSAppUsesNonExemptEncryption", false);
                isChanged = true;
            }

            if (isChanged)
            {
                File.WriteAllText(plistPath, plistDocument.WriteToString());
            }
        }
#endif
    }
}