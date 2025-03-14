using R3;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Session
{
  public class GameSessionData
  {
    public readonly ReactiveProperty<int> Health = new ReactiveProperty<int>(-5);
    public readonly ReactiveProperty<float> Score = new ReactiveProperty<float>();
    public readonly ReactiveProperty<int> RevivalCount = new ReactiveProperty<int>();
  }
}