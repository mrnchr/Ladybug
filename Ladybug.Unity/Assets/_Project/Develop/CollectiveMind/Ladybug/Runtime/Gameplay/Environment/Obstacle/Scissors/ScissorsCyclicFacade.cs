using System.Threading;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Scissors
{
  public class ScissorsCyclicFacade : IFacade, IBindable, IEntityInitializable
  {
    private readonly ScissorsConfig _config;
    
    private ScissorsVisual _visual;
    
    public ScissorsCyclicFacade(ScissorsConfig config)
    {
      _config = config;
    }
    
    public void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<ScissorsVisual>();
    }
    
    public void Initialize(EntityInitContext initContext)
    {
      RunCycle(_visual.destroyCancellationToken).Forget();
    }
    
    private async UniTask RunCycle(CancellationToken token)
    {
      while (!token.IsCancellationRequested)
      {
        _visual.PlayOpenAnimation();
        await UniTask.WaitForSeconds(
            _config.OpenAnimationDuration * _config.AnimationSpeed + _config.OpenedStateDuration,
            cancellationToken: token)
          .SuppressCancellationThrow();
        
        if (token.IsCancellationRequested)
          return;
        
        _visual.PlayCloseAnimation();
        await UniTask.WaitForSeconds(
            _config.OpenAnimationDuration * _config.AnimationSpeed + _config.ClosedStateDuration,
            cancellationToken: token)
          .SuppressCancellationThrow();
      }
    }
  }
}