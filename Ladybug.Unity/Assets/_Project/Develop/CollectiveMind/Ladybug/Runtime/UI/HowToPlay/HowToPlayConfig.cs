using System;
using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.Localization;

namespace CollectiveMind.Ladybug.Runtime.UI.HowToPlay
{
  [CreateAssetMenu(menuName = CAC.UI_MENU + "HowToPlay", fileName = nameof(HowToPlayConfig))]
  public class HowToPlayConfig : ScriptableObject
  {
    [ListDrawerSettings(ShowElementLabels = true)]
    public List<SlideEntry> SlideInfoList;
    public int DaysWithoutTutorial;
  }

  [Serializable]
  public class SlideEntry
  {
    [LabelText("Slide Header")]
    public LocalizedString SlideTitle;
    
    [ListDrawerSettings(ShowElementLabels = true)]
    public List<GifEntry> GifInfoList;
  }

  [Serializable]
  public class GifEntry
  {
    [LabelText("Gif Path")]
    public string VideoPath;

    [LabelText("Gif Text")]
    public string Caption;
  }
}