using System;
using System.Linq;
using System.Threading;
using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras;
using CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using R3;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Obstacle.Enemy
{
  public class EnemyFacade : IFacade, IEntityInitializable, IBindable, IGameStep
  {
    private readonly EnemyConfig _config;
    private readonly IEcsUniverse _universe;
    private readonly CameraService _cameraService;
    private readonly SessionService _session;
    private readonly LadybugService _ladybugService;
    private readonly LadybugBooster _booster;
    protected readonly LadybugConfig _ladybugConfig;

    private EnemyStateMachine _stateMachine;
    private EnemyVisual _visual;
    private DisposableBag _disposableBag;

    public CancellationToken DestroyCancellationToken => _visual.destroyCancellationToken;

    protected EnemyConfig Config => _config;

    private EcsEntityWrapper _entity => _visual.Entity;

    public EnemyFacade(EnemyConfig config,
      IEcsUniverse universe,
      CameraService cameraService,
      SessionService session,
      LadybugService ladybugService,
      LadybugBooster booster,
      LadybugConfig ladybugConfig)
    {
      _universe = universe;
      _config = config;
      _cameraService = cameraService;
      _session = session;
      _booster = booster;

      _ladybugConfig = ladybugConfig;
      _ladybugService = ladybugService;
    }

    public virtual void Bind(EcsEntityWrapper entity)
    {
      _visual = entity.GetVisual<EnemyVisual>();
      _disposableBag.Add(_universe.Subscribe<OutCameraView>(_entity, OnCameraViewExit));
    }

    public void Initialize(EntityInitContext _)
    {
      _stateMachine = new EnemyStateMachine(this, Config.States);
      _stateMachine.Switch(Config.States.FirstOrDefault());
    }

    public void Step()
    {
      
    }

    private void OnCameraViewExit()
    {
      // destroy
    }

    public void StartMove()
    {
      
    }

    public void StopMove()
    {
      
    }

    public virtual float GetCurrentSpeed()
    {
      return _ladybugConfig.Speed * _booster.Multiplier;
    }
  }
}