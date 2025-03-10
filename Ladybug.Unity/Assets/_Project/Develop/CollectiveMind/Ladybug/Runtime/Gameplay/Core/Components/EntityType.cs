namespace CollectiveMind.Ladybug.Runtime.Gameplay
{
  public enum EntityType 
  {
    None = 0,
    Ladybug = 1,
    
    Canvas = 1000,
    Blob = Canvas + 1,
    Eraser = Blob + 1,
    PaperClip = Eraser + 1,
    Pencil = PaperClip + 1,
    PushPin1 = Pencil + 1,
    PushPin2 = PushPin1 + 1,
    Marker = PushPin2 + 1,
    
    SpawnPoint = 10000
  }
}