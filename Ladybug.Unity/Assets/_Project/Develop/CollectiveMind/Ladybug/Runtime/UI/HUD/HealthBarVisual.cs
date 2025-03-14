using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using UnityEngine;
using Zenject;
using R3;
using UnityEngine.UI;

namespace CollectiveMind.Ladybug.Runtime.UI.HUD
{
  public class HealthBarVisual : MonoBehaviour
  {
    [SerializeField]
    private List<Image> _orderedHealthPoints;
    
    private GameSessionData _sessionData;

    [Inject]
    public void Construct(GameSessionData sessionData)
    {
      _sessionData = sessionData;
      _sessionData.Health.Subscribe(OnHealthChanged);
    }

    private void OnHealthChanged(int health)
    {
      for (int i = 0; i < _orderedHealthPoints.Count; i++)
      {
        _orderedHealthPoints[i].gameObject.SetActive(i < health);
      }
    }
  }
}