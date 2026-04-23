using CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HowToPlay
{
  public class HowToPlayWindow : BaseWindow
  {
    [SerializeField]
    private SlideElement _slideElement;

    [SerializeField]
    private Button _closeButton;
    
    [SerializeField]
    private Button _nextButton;

    [SerializeField]
    private LocalizeStringEvent _nextButtonText;

    [SerializeField]
    private LocalizedString _nextText;

    [SerializeField]
    private LocalizedString _lastSlideText;
    
    private HowToPlayConfig _howToPlayConfig;
    private IWindowManager _windowManager;
    private int _currentSlideIndex;

    [Inject]
    private void Construct(HowToPlayConfig howToPlayConfig, IWindowManager windowManager)
    {
      _howToPlayConfig = howToPlayConfig;
      _windowManager = windowManager;
    }

    protected override UniTask OnOpened()
    {
      if (_howToPlayConfig.SlideInfoList.Count > 0)
      {
        _currentSlideIndex = 0;
        SetSlide();
      }
      
      return UniTask.CompletedTask;
    }

    protected override void AddListeners()
    {
      _closeButton.onClick.AddListener(CloseThisWindow);
      _nextButton.onClick.AddListener(OnNextSlide);
    }

    protected override void RemoveListeners()
    {
      _closeButton.onClick.RemoveListener(CloseThisWindow);
      _nextButton.onClick.RemoveListener(OnNextSlide);
    }

    protected override UniTask OnClosed()
    {
      _slideElement.Clear();
      return UniTask.CompletedTask;
    }

    private void OnNextSlide()
    {
      if (_currentSlideIndex == _howToPlayConfig.SlideInfoList.Count - 1)
      {
        CloseThisWindow();
        return;
      }
      
      _currentSlideIndex++;
      SetSlide();
    }

    private void CloseThisWindow()
    {
      _windowManager.CloseLastOpenedWindow().Forget();
    }

    private void SetSlide()
    {
      _slideElement.SetSlide(_howToPlayConfig.SlideInfoList[_currentSlideIndex]);
      UpdateButton();
    }

    private void UpdateButton()
    {
      _nextButtonText.StringReference =
        _currentSlideIndex == _howToPlayConfig.SlideInfoList.Count - 1 ? _lastSlideText : _nextText;
    }
  }
}