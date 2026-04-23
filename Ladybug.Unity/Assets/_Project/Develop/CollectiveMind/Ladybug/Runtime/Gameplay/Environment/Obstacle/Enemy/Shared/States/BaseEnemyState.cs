using System;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy
{
  public class BaseEnemyStateData
  {
    public Type NextState;
  }

  public abstract class BaseEnemyState : IGameStep
  {
    protected EnemyStateMachine Owner;
    protected EnemyFacade Facade;

    protected BaseEnemyState(EnemyStateMachine owner, EnemyFacade facade)
    {
      Owner = owner;
      Facade = facade;
    }

    public abstract void Enter(BaseEnemyStateData data);
    public abstract void Step();
    public abstract void Exit();

    protected static T CastData<T>(BaseEnemyStateData data) where T : BaseEnemyStateData
    {
      return data as T;
    }

    protected static bool TryCastData<T>(BaseEnemyStateData data, out T result) where T : BaseEnemyStateData
    {
      result = CastData<T>(data);
      return result != null;
    }
  }
}