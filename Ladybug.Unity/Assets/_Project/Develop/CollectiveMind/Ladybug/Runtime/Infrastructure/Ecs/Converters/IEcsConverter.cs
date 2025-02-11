namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsConverter
  {
    public void ConvertTo(EcsEntityWrapper entity);
    public void ConvertBack(EcsEntityWrapper entity);
  }
}