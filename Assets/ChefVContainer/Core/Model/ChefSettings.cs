using System.Linq;
using UnityEngine;

namespace gs.chef.vcontainer.core.model
{
    public class ChefSettings : ScriptableObject
    {
        public static ChefSettings Instance { get; private set; }

        [field: SerializeField] public LogState LogState { get; private set; }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/CHEF Game/Chef Settings")]
        public static void CreateSettingsAsset()
        {
            var title = "Create ChefSettings";
            var assetName = "ChefSettings";
            var assetExtension = "asset";
            var path = UnityEditor.EditorUtility.SaveFilePanelInProject(title, assetName, assetExtension, string.Empty);

            if (string.IsNullOrEmpty(path))
                return;

            var settings = ScriptableObject.CreateInstance<ChefSettings>();
            UnityEditor.AssetDatabase.CreateAsset(settings, path);

            var preloadedAssets = UnityEditor.PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.RemoveAll(x => x is ChefSettings);
            preloadedAssets.Add(settings);
            UnityEditor.PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
        }

        public static void LoadInstanceFromPreloadAssets()
        {
            var preloadAsset = UnityEditor.PlayerSettings.GetPreloadedAssets().FirstOrDefault(x => x is ChefSettings);
            if (preloadAsset is ChefSettings instance)
            {
                instance.OnEnable();
            }
        }
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void LoadInstanceFromPreloadAssetsOnLoad()
        {
            LoadInstanceFromPreloadAssets();
        }
#endif

        private void OnEnable()
        {
            if (Application.isPlaying)
                Instance = this;
        }
    }
}