using System.Collections.Generic;
using System.Linq;
using CollectiveMind.Ladybug.Runtime;
using LudensClub.GeoChaos.Editor.Monitoring.Component;
using TriInspector;
using UnityEngine;

namespace LudensClub.GeoChaos.Editor.Monitoring.Entity
{
  public class EcsEntityView : MonoBehaviour
  {
    public List<EcsComponentView> ComponentPull = new List<EcsComponentView>();

    [SerializeReference]
    [HideReferencePicker]
    public List<IEcsComponentView> Components = new List<IEcsComponentView>();

    private IEcsEntityPresenter _presenter;

    private bool AnyComponents => ComponentPull.Any();

    public void SetController(IEcsEntityPresenter presenter)
    {
      _presenter = presenter;
    }

    [EnableIf(nameof(AnyComponents))]
    [GUIColor(CC.GREEN)]
    [PropertyOrder(1)]
    [Button("Add Components")]
    public void AddComponents()
    {
      _presenter.AddComponents();
    }

    [EnableIf(nameof(AnyComponents))]
    [GUIColor(CC.LIGHT_RED)]
    [PropertyOrder(1)]
    [Button("Remove Components")]
    public void RemoveComponents()
    {
      _presenter.RemoveComponents();
    }

    [GUIColor(CC.LIGHT_BLUE)]
    [PropertyOrder(1)]
    [Button("Clear Pull")]
    public void Clear()
    {
      ComponentPull.Clear();
    }
  }
}