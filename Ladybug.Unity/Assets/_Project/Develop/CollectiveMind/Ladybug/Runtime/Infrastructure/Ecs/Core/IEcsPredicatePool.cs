﻿namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsPredicatePool
  {
    EcsPredicate<TComponent> PopPredicate<TComponent>() where TComponent : struct, IEcsComponent;
    void PushPredicate<TComponent>(IEcsPredicate predicate) where TComponent : struct, IEcsComponent;
  }
}