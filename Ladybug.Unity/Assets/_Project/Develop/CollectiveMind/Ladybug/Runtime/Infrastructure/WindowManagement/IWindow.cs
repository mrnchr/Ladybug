namespace CollectiveMind.Ladybug.Runtime.Infrastructure.WindowManagement
{
  public interface IWindow
  {
    public bool IsOpen { get; }
    
    public void Open();
    public void Close();
  }
}