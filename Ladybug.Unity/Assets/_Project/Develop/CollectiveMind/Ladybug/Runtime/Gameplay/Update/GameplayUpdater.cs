using System.Collections.Generic;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public class GameplayUpdater : IFixedTickable, ITickable
  {
    private readonly List<IGameStep> _steps = new List<IGameStep>();
    private readonly List<IGameFixedStep> _fixedSteps = new List<IGameFixedStep>();

    public bool IsActive { get; private set; }

    public void Add(IGameCycle cycle)
    {
      if (cycle is IGameFixedStep fixedStep)
        _fixedSteps.Add(fixedStep);
      
      if (cycle is IGameStep step)
        _steps.Add(step);
    }

    public void Remove(IGameCycle cycle)
    {
      if (cycle is IGameFixedStep fixedStep)
        _fixedSteps.Remove(fixedStep);
      
      if (cycle is IGameStep step)
        _steps.Remove(step);
    }

    public void SetActive(bool value)
    {
      IsActive = value;
    }

    public void FixedTick()
    {
      if (!IsActive)
        return;

      List<IGameFixedStep> steps = UnityEngine.Pool.ListPool<IGameFixedStep>.Get();
      steps.AddRange(_fixedSteps);
      foreach (IGameFixedStep step in steps)
        step.FixedStep();
      
      UnityEngine.Pool.ListPool<IGameFixedStep>.Release(steps);
    }

    public void Tick()
    {
      if (!IsActive)
        return;
      
      List<IGameStep> steps = UnityEngine.Pool.ListPool<IGameStep>.Get();
      steps.AddRange(_steps);
      foreach (IGameStep step in steps)
        step.Step();

      UnityEngine.Pool.ListPool<IGameStep>.Release(steps);
    }
  }
}