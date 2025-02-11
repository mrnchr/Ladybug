using System;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.LifeCycle
{
  public class Starter : MonoBehaviour
  {
    public event Action OnStarted;

    private void Start()
    {
      OnStarted?.Invoke();
    }
  }
}