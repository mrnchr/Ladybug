using System.Threading;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure
{
  public static class CTSUtils
  {
    public static void CancelDisposeAndClear(ref CancellationTokenSource source)
    {
      source = source?.CancelDisposeAndForget();
    }
    
    public static CancellationTokenSource CancelDisposeAndForget(this CancellationTokenSource source)
    {
      source?.Cancel();
      source?.Dispose();
      return null;
    }
  }
}