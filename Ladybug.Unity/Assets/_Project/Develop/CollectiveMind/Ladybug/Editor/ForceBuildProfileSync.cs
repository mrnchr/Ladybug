using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CollectiveMind.Ladybug.Editor
{
  public class ForceBuildProfileSync : IPreprocessBuildWithReport
  {
    public int callbackOrder => 0;

    public void OnPreprocessBuild(BuildReport report)
    {
      BuildTargetGroup group = EditorUserBuildSettings.selectedBuildTargetGroup;
      var target = NamedBuildTarget.FromBuildTargetGroup(group);
      string defines = PlayerSettings.GetScriptingDefineSymbols(target);
      PlayerSettings.SetScriptingDefineSymbols(target, defines);
      AssetDatabase.SaveAssets();
      UnityEngine.Debug.Log("[ForceBuildProfileSync] Build profile synced before build.");
    }
  }
}