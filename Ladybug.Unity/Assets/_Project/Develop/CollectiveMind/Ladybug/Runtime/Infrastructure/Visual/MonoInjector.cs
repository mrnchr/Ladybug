using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Visual
{
  [DisallowMultipleComponent]
  public class MonoInjector : MonoBehaviour, IInjectable
  {
    public bool Injected { get; set; }

    [Inject]
    public void Construct()
    {
      Injected = true;
    }

    private void Awake()
    {
      if (!Injected)
      {
        Context ctx = GetComponentInParent<GameObjectContext>(true);
        var parentInjector = transform.parent.GetComponentInParent<MonoInjector>(true);

        switch ((bool)ctx, (bool)parentInjector)
        {
          case (false, false):
          case (false, true) when parentInjector.Injected:
          case (true, false):
          case (true, true) when parentInjector.transform.IsChildOf(ctx.transform) && parentInjector.Injected:
          case (true, true) when ctx.transform.IsChildOf(parentInjector.transform):
          case (true, true) when ctx.gameObject == parentInjector.gameObject && parentInjector.Injected:
            Inject(ctx);
            break;
        }
      }
    }

    private void Inject(Context ctx)
    {
      if (ctx == null)
        ctx = FindAnyObjectByType<SceneContext>();
      ctx.Container.InjectGameObject(gameObject);
    }
  }
}