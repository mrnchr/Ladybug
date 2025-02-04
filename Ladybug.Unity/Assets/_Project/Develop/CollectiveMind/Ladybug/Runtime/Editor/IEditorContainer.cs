#if UNITY_EDITOR
namespace CollectiveMind.Ladybug.Runtime
{
  public interface IEditorContainer
  {
    IProfilerService ProfilerService { get; set; }
  }
}
#endif