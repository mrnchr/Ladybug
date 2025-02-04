namespace CollectiveMind.Ladybug.Runtime
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
    }
  }
}