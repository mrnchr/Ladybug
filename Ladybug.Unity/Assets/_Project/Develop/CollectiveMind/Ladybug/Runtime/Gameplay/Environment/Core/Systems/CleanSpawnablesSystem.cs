using CollectiveMind.Ladybug.Runtime.Gameplay.Cameras.PlayerCamera;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.Creation;
using Leopotam.EcsLite;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Systems
{
  public class CleanSpawnablesSystem : IEcsRunSystem
  {
    private readonly EntityFactory _entityFactory;
    private readonly EcsEntities _spawnables;
    private readonly EcsEntities _cameras;

    public CleanSpawnablesSystem(IEcsUniverse universe, EntityFactory entityFactory)
    {
      _entityFactory = entityFactory;
      _cameras = universe
        .FilterGame<CameraTag>()
        .Inc<GameObjectRef>()
        .Collect();

      _spawnables = universe
        .FilterGame<Cleanable>()
        .Inc<GameObjectRef>()
        .Collect();
    }

    public void Run(IEcsSystems systems)
    {
      foreach (EcsEntityWrapper camera in _cameras)
      foreach (EcsEntityWrapper spawnable in _spawnables)
      {
        Bounds cameraBounds = GetCameraBounds(camera.Get<CameraData>().WorldXZBounds);
        Bounds objectBounds = spawnable.Get<MeshRendererRef>().MeshRenderer.bounds;

        if (cameraBounds.min.z - objectBounds.max.z > 20)
        {
          _entityFactory.DestroyEntity(spawnable);
        }
      }
    }

    private Bounds GetCameraBounds(Rect bounds2D)
    {
      Vector3 minBound = new Vector3(bounds2D.min.x, 0, bounds2D.min.y);
      Vector3 maxBound = new Vector3(bounds2D.max.x, 0, bounds2D.max.y);
      var cameraBounds = new Bounds();
      cameraBounds.SetMinMax(minBound, maxBound);
      return cameraBounds;
    }
  }
}