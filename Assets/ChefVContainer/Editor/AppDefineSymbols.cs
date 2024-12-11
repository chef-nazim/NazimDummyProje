using gs.chef.vcontainer.core.model;
using gs.chef.vcontainer.utility;
using UnityEditor;

namespace gs.chef.vcontainer.editor
{
    [InitializeOnLoad]
    public class AppDefineSymbolss
    {
        static AppDefineSymbolss()
        {
            AddDefineSymbolsExisting();
        }

        private static void AddDefineSymbolsExisting()
        {
            if (IsUniTaskAndDoTweenAvailable())
            {
                AddDefineSymbols("UNITASK_DOTWEEN_SUPPORT");
            }
        }

        private static bool IsUniTaskAndDoTweenAvailable()
        {
            return System.Type.GetType("Cysharp.Threading.Tasks.UniTask, UniTask") != null &&
                   System.Type.GetType("DG.Tweening.DOTween, DOTween") != null;
        }

        private static void AddDefineSymbols(string symbol)
        {
            BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;

            string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            if (!defineSymbols.Contains(symbol))
            {
                defineSymbols += ";" + symbol;
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTargetGroup, defineSymbols);
                CLogger.Log(LogState.Game, "Added define symbol: " + symbol);
            }
        }
    }
}