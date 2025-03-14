using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.Advertisement
{
  public class AdService : IAdService
  {
    public async UniTask ShowAd()
    {
      await UniTask.WaitForSeconds(3, true);
    }
  }
}