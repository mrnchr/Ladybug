using System;
using System.Collections.Generic;
using TriInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Signal
{
  [CreateAssetMenu(menuName = CAC.ENTITY_MENU + "Signal", fileName = "SignalConfig")]
  public class SignalConfig : ScriptableObject
  {
    [SerializeField]
    private List<SignalDataEntry> _data = new List<SignalDataEntry>();

    public SignalData GetSignalData(SignalType type)
    {
      return _data.Find(x => x.Type == type).Data;
    }
  }

  [Serializable]
  [DeclareFoldoutGroup(nameof(SignalDataEntry), Title = "$Name")]
  public class SignalDataEntry
  {
    [Group(nameof(SignalDataEntry))]
    [HideLabel]
    public SignalType Type;

    [Group(nameof(SignalDataEntry))]
    [InlineProperty]
    [HideLabel]
    public SignalData Data;

#if UNITY_EDITOR
    private string Name => ObjectNames.NicifyVariableName(Type.ToString());
#endif
  }
}