using CollectiveMind.Ladybug.Runtime.Gameplay.Session;

namespace CollectiveMind.Ladybug.Runtime.UI.Defeat
{
  public class Reviver
  {
    private readonly SessionService _sessionSvc;
    private readonly GameSessionData _sessionData;

    public Reviver(SessionService sessionSvc)
    {
      _sessionSvc = sessionSvc;
    }
    
    public void Revive()
    {
      _sessionSvc.ResetHealth();
    }
  }
}