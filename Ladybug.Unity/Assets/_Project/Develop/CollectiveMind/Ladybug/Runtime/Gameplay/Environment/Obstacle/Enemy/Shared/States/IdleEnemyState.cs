using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy
{
  public class IdleEnemyStateData : BaseEnemyStateData
  {
    public float Time;
  }

  public class IdleEnemyState : BaseEnemyState
  {
    public IdleEnemyState(EnemyStateMachine owner, EnemyFacade facade) : base(owner, facade) { }

    public override void Enter(BaseEnemyStateData data)
    {
      Facade.StopMove();

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

    public override void Step() { }
    public override void Exit() { }
  }
}