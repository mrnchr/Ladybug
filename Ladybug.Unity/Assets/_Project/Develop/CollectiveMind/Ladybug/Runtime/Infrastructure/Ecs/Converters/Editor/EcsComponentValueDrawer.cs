using System.Linq;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Converters.Editor;
using TriInspector;
using UnityEditor;

[assembly: RegisterTriValueDrawer(typeof(EcsComponentValueDrawer), TriDrawerOrder.System)]

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs.Converters.Editor
{
  public class EcsComponentValueDrawer : TriValueDrawer<EcsComponentValue>
  {
    public override TriElement CreateElement(TriValue<EcsComponentValue> propertyValue, TriElement next)
    {
      return new EcsComponentValueElement(propertyValue, next);
    }

    private class EcsComponentValueElement : TriElement
    {
      private readonly TriValue<EcsComponentValue> _propertyValue;

      public EcsComponentValueElement(TriValue<EcsComponentValue> propertyValue, TriElement next)
      {
        _propertyValue = propertyValue;
        AddChild(next);
      }

      public override bool Update()
      {
        if (_propertyValue.Property.TryGetSerializedProperty(out SerializedProperty serializedProperty) 
          && serializedProperty.serializedObject.targetObject is EcsConverterAsset asset)
        {
          EcsComponentValue value = _propertyValue.SmartValue;
          value.IsOverridenComponent = asset.ComponentTypes.Count(x => x == value.Type) > 1;
        }

        return base.Update();
      }
    }
  }
}