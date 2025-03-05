using CollectiveMind.Ladybug.Runtime.Infrastructure.SceneLoading;
using CollectiveMind.Ladybug.Runtime.UI;
using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.SceneTransition
{
  public class GameSwitcher : IGameSwitcher
  {
    private readonly ISceneLoader _sceneLoader;
    private readonly CurtainFacade _curtainFacade;

    public GameSwitcher(ISceneLoader sceneLoader, CurtainFacade curtainFacade)
    {
      _sceneLoader = sceneLoader;
      _curtainFacade = curtainFacade;
    }
    
    public async UniTask SwitchToGame()
    {
      await _curtainFacade.ShowCurtain();
      await _sceneLoader.LoadAsync(SceneType.Game);
      await _curtainFacade.HideCurtain();
    }

    public async UniTask SwitchToMenu()
    {
      await _curtainFacade.ShowCurtain();
      await _sceneLoader.LoadAsync(SceneType.Menu);
      await _curtainFacade.HideCurtain();
    }
  }
}