using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.UI.HUD
{
  public class ScoreVisual : MonoBehaviour
  {
    [SerializeField]
    private TMP_Text _scoreLabel;

    [Inject]
    public void Construct(SessionService session)
    {
      session.Score.Subscribe(ChangeScoreText);
    }

    private void ChangeScoreText(float score)
    {
      _scoreLabel.text = Mathf.RoundToInt(score).ToString();
    }
  }
}