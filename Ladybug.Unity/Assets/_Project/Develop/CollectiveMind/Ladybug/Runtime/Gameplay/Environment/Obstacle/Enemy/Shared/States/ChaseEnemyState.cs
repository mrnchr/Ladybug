using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy
{
  public class ChaseEnemyStateData : BaseEnemyStateData
  {
    public Transform Target;
  }

  public class ChaseEnemyState : BaseEnemyState
  {
    public ChaseEnemyState(EnemyStateMachine owner, EnemyFacade facade) : base(owner, facade) { }

    public override void Enter(BaseEnemyStateData data)
    {
      Facade.StartMove();

      if (TryCastData<IdleEnemyStateData>(data, out var result))
      {
        UniTask.WaitForSeconds(result.Time)
          .ContinueWith(() => Owner.Switch(data.NextState))
          .SuppressCancellationThrow()
          .Forget();
      }
      else
      {
        Owner.Switch(data.NextState);
      }
    }

    public override void Step()
    {
      
    }
    
    public override void Exit() { }
  }
}