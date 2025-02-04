using CollectiveMind.Ladybug.Runtime;

namespace CollectiveMind.Ladybug.Editor.Boot
{
  public class EditorContainer : IEditorContainer
  {
    public IProfilerService ProfilerService { get; set; }
  }
}