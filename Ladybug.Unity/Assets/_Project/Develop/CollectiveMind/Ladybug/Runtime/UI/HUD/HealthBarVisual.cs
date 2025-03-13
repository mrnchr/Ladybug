using System.Collections.Generic;
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
    
    private HealthBarFacade _facade;

    [Inject]
    public void Construct(HealthBarFacade facade)
    {
      _facade = facade;
      _facade.HP.Subscribe(OnHealthChanged);
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