using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public class PauseSwitcher : IPauseSwitcher
  {
    public void PauseGame()
    {
      Time.timeScale = 0;
    }

    public void ResumeGame()
    {
      Time.timeScale = 1;
    }
  }
}