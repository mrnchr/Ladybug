namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public interface IGameStep : IGameCycle
  {
    public void Step();
  }
}