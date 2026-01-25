using System.Collections;
using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime.Configuration;
using CollectiveMind.Ladybug.Runtime.Gameplay.Environment.Canvas;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Worlds;
using FluentAssertions;
using NSubstitute;
using UnityEngine;
using UnityEngine.TestTools;

namespace CollectiveMind.Ladybug.Testing.IntegrationTests
{
  public class CanvasTests
  {
    [UnityTest]
    public IEnumerator WhenFindCanvasesCrossedBySegmentAndSegmentCrossesThreeCanvasesThenThreeCanvasesShouldBeFound()
    {
      // Arrange.
      var game = new GameWorldWrapper();
      var message = new MessageWorldWrapper();
      List<IEcsWorldWrapper> worldWrappers = new List<IEcsWorldWrapper> { game, message };
      IEcsUniverse universe = new EcsUniverse(worldWrappers);
      CanvasConfig canvasConfig = ScriptableObject.CreateInstance<CanvasConfig>();
      // configProvider.Get<CanvasConfig>().Returns(canvasConfig);
      canvasConfig.CanvasScale = 1;
      CanvasService canvasSvc = new CanvasService(universe, canvasConfig);

      CreateCanvas(Vector3.zero);
      CreateCanvas(Vector3.one * 10);
      CreateCanvas(Vector3.forward * 10);
      CreateCanvas(Vector3.right * 10);

      Vector2 start = new Vector2(12, 12);
      Vector2 end = new Vector2(0, 12);

      // yield return new WaitForSeconds(5);

      // Act.
      EcsRawEntities entities = canvasSvc.FindCanvasesCrossedBySegment(start, end);

      // Assert.
      entities.Entities.Count.Should().Be(3);

      Object.DestroyImmediate(canvasConfig);
      foreach (Transform transform in Object.FindObjectsByType<Transform>(FindObjectsInactive.Include,
        FindObjectsSortMode.None))
        Object.DestroyImmediate(transform.gameObject);

      game.World.Destroy();
      message.World.Destroy();
      yield break;

      void CreateCanvas(Vector3 position)
      {
        game.World.CreateEntity()
          .Add<CanvasTag>()
          .Add<ConverterRef>()
          .Add((ref TransformRef transformRef) =>
          {
            transformRef.Transform = new GameObject().transform;
            transformRef.Transform.position = position;
          });
      }
    }
  }
}