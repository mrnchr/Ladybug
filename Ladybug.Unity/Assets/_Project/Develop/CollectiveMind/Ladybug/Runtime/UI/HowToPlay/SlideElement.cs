using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HowToPlay
{
  public class SlideElement : MonoBehaviour
  {
    [SerializeField]
    private LocalizeStringEvent _title;

    [SerializeField]
    private Transform _gifContainer;
    
    [SerializeField]
    private GifElement _gifElementPrefab;

    private readonly List<GifElement> _gifs = new List<GifElement>();
    private IInstantiator _instantiator;

    [Inject]
    private void Construct(IInstantiator instantiator)
    {
      _instantiator = instantiator;
    }
    
    public void SetSlide(SlideEntry slideEntry)
    {
      _title.StringReference = slideEntry.SlideTitle;
      
      foreach (GifElement gif in _gifs)
      {
        gif.gameObject.SetActive(false);
      }
      
      for (int i = 0; i < _gifs.Count && i < slideEntry.GifInfoList.Count; i++)
      {
        _gifs[i].gameObject.SetActive(true);
        _gifs[i].SetGif(slideEntry.GifInfoList[i]);
      }
      
      for (int i = _gifs.Count; i < slideEntry.GifInfoList.Count; i++)
      {
        _gifs.Add(_instantiator.InstantiatePrefabForComponent<GifElement>(_gifElementPrefab, _gifContainer));
        _gifs[i].SetGif(slideEntry.GifInfoList[i]);
      }
    }

    public void Clear()
    {
      foreach (GifElement gif in _gifs)
      {
        Destroy(gif.gameObject);
      }
      
      _gifs.Clear();
    }
  }
}