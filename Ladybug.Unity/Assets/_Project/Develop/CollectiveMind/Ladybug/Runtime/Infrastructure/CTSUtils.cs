using System.Threading;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure
{
  public static class CTSUtils
  {
    public static CancellationTokenSource CancelDisposeAndForget(this CancellationTokenSource source)
    {
      source?.Cancel();
      source?.Dispose();
      return null;
    }
  }
}