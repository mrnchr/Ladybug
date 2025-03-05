using Cysharp.Threading.Tasks;

namespace CollectiveMind.Ladybug.Runtime.SceneTransition
{
  public interface IGameSwitcher
  {
    UniTask SwitchToGame();
    UniTask SwitchToMenu();
  }
}