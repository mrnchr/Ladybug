namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  public interface IEcsConverter
  {
    public void ConvertTo(EcsEntity entity);
    public void ConvertBack(EcsEntity entity);
  }
}