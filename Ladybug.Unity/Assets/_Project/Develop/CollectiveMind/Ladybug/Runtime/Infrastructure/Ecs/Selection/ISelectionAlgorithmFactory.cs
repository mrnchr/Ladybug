namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface ISelectionAlgorithmFactory
  {
    TAlgorithm Create<TAlgorithm>(params object[] parameters) where TAlgorithm : ISelectionAlgorithm;
  }
}