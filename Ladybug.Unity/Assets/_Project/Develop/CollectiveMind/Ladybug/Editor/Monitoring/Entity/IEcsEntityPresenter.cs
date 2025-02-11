using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;

namespace CollectiveMind.Ladybug.Editor.Monitoring.Entity
{
  public interface IEcsEntityPresenter
  {
    int Entity { get; }
    EcsEntityView View { get; }

    void Tick();
    void Initialize();
    void SetActive(bool value);
    void UpdateView();
    void AddComponents();
    void RemoveComponents();
    void ChangeComponent(IEcsComponent component);
  }
}