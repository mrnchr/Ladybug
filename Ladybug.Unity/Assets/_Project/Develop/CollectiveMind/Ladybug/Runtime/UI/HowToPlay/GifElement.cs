using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HowToPlay
{
  public class GifElement : MonoBehaviour
  {
    [SerializeField]
    private RawImage _rawImage;

    [SerializeField]
    private VideoPlayer _videoPlayer;

    [SerializeField]
    private TMP_Text _caption;

    private HowToPlayConfig _howToPlayConfig;
    private RenderTexture _renderTexture;

    [Inject]
    private void Construct(HowToPlayConfig howToPlayConfig)
    {
      _howToPlayConfig = howToPlayConfig;
    }

    private void Start()
    {
      _renderTexture = new RenderTexture((int)_rawImage.rectTransform.rect.width,
        (int)_rawImage.rectTransform.rect.height, 0);
      _renderTexture.Create();
      
      _videoPlayer.targetTexture = _renderTexture;
      _rawImage.texture = _renderTexture;
    }

    public void SetGif(GifEntry gifEntry)
    {
      _videoPlayer.url = Path.Combine(Application.streamingAssetsPath, _howToPlayConfig.GifFolderPath,
        gifEntry.GifFileName);
      _videoPlayer.Play();
      _caption.text = gifEntry.Caption;
    }

    private void OnDestroy()
    {
      _renderTexture.Release();
    }
  }
}