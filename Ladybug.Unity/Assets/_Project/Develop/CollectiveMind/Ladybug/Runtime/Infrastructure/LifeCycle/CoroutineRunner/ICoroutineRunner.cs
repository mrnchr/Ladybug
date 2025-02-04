using System.Collections;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle.CoroutineRunner
{
  public interface ICoroutineRunner
  {
    Coroutine Run(IEnumerator coroutine);
    void Abort(IEnumerator coroutine);
    void Abort(Coroutine coroutine);
  }
}