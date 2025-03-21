﻿namespace CollectiveMind.Ladybug.Runtime
{
  /// <summary>
  /// Create Asset Menu constants
  /// </summary>
  public static class CAC
  {
    public const string PROJECT_MENU = "Ladybug/";

    public const string CONFIG_MENU = PROJECT_MENU + "Configs/";
    public const string ENTITY_MENU = CONFIG_MENU + "Entities/";

    public const string ECS_MENU = PROJECT_MENU + "Ecs/";


    public static class Names
    {
      public const string ECS_CONVERTER_MENU = ECS_MENU + "Converter";
      public const string ECS_CONVERTER_FILE = "NewEntityConverter";

      public const string LADYBUG_CONFIG_MENU = ENTITY_MENU + "Ladybug";
      public const string LADYBUG_CONFIG_FILE = "LadybugConfig";

      public const string SCRIPTABLE_CONFIG_PROVIDER_MENU = CONFIG_MENU + "ConfigProvider";
      public const string SCRIPTABLE_CONFIG_PROVIDER_FILE = "ConfigProvider";

      public const string DRAWING_CONFIG_MENU = ENTITY_MENU + "Drawing";
      public const string DRAWING_CONFIG_FILE = "DrawingConfig";

      public const string PREFAB_PROVIDER_MENU = CONFIG_MENU + "PrefabProvider";
      public const string PREFAB_PROVIDER_FILE = "PrefabProvider";

      public const string CANVAS_CONFIG_MENU = ENTITY_MENU + "Canvas";
      public const string CANVAS_CONFIG_FILE = "CanvasConfig";

      public const string SCENES_PROVIDER_MENU = CONFIG_MENU + "Scenes";
      public const string SCENES_PROVIDER_FILE = "ScenesProvider";

      public const string CURTAIN_CONFIG_MENU = CONFIG_MENU + "Curtain";
      public const string CURTAIN_CONFIG_FILE = "CurtainConfig";
      
      public const string OBSTACLE_SPAWN_CONFIG_MENU = ENTITY_MENU + "ObstacleSpawn";
      public const string OBSTACLE_SPAWN_CONFIG_FILE = "ObstacleSpawnConfig";
      
      public const string GAME_SESSION_CONFIG_MENU = CONFIG_MENU + "GameSession";
      public const string GAME_SESSION_CONFIG_FILE = "GameSessionConfig";
    }
  }
}