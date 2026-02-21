using CollectiveMind.Ladybug.Editor;
using CollectiveMind.Ladybug.Editor.Monitoring.Entity;
using CollectiveMind.Ladybug.Editor.Monitoring.Universe;
using CollectiveMind.Ladybug.Editor.Monitoring.World;
using CollectiveMind.Ladybug.Runtime;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using Leopotam.EcsLite;
using TriInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using Zenject;

[assembly: RegisterTriValueDrawer(typeof(EcsPackedEntityDrawer), TriDrawerOrder.Fallback)]

namespace CollectiveMind.Ladybug.Editor
{
  public class EcsPackedEntityDrawer : TriValueDrawer<EcsPackedEntity>
  {
    public override TriElement CreateElement(TriValue<EcsPackedEntity> propertyValue, TriElement next)
    {
      return new EcsPackedEntityDrawerElement(propertyValue);
    }

    private class EcsPackedEntityDrawerElement : TriElement
    {
      private readonly bool _isSceneObject;
      private readonly TriValue<EcsPackedEntity> _propertyValue;

      private EcsEntityView _obj;

      public EcsPackedEntityDrawerElement(TriValue<EcsPackedEntity> propertyValue)
      {
        _propertyValue = propertyValue;
        _isSceneObject = IsPropertyOfSceneObject(_propertyValue.Property);
      }

      public override bool Update()
      {       
        EcsEntityView value = _obj;
        SceneContext ctx;
        IEcsUniversePresenter universe;
        if (_isSceneObject && EditorApplication.isPlaying
          && (ctx = Object.FindAnyObjectByType<SceneContext>()) != null
          && (universe = ctx.Container.TryResolve<IEcsUniversePresenter>()) != null
          && universe.WasInitialized)
        {
          string worldName = _propertyValue.Property.TryGetAttribute(out ExistInWorldAttribute worldAttribute)
            ? worldAttribute.Name
            : EcsConstants.Worlds.GAME;

          IEcsWorldPresenter worldPresenter = universe.Children.Find(x => x.Wrapper.Name == worldName);
          EcsWorld world = worldPresenter.Wrapper.World;
          _obj = _propertyValue.SmartValue.TryUnpackEntity(world, out EcsEntityWrapper entity)
            ? worldPresenter.Children.Find(x => x.Entity == entity.Entity).View
            : null;
        }
        else
        {
          _obj = null;
        }

        return base.Update() || value != _obj;
      }

      public override float GetHeight(float width)
      {
        return EditorGUIUtility.singleLineHeight;
      }

      public override void OnGUI(Rect position)
      {
        GUIContent content = _propertyValue.Property.DisplayNameContent;
        if (!_isSceneObject)
        {
          GUIContent rightContent = new GUIContent("Show only in scene objects", content.image);
          EditorGUI.LabelField(position, content, rightContent);
        }
        else if (!EditorApplication.isPlaying)
        {
          GUIContent rightContent = new GUIContent("Show only in play mode", content.image);
          EditorGUI.LabelField(position, content, rightContent);
        }
        else
        {
          EditorGUI.ObjectField(position, content, _obj, typeof(EcsEntityView),
            true);
        }
      }

      private static bool IsPropertyOfSceneObject(TriProperty property)
      {
        string scene = property?.Owner?.Value is MonoBehaviour monoBehaviour ? monoBehaviour.gameObject.scene.name : null;
        bool isSceneObject = scene != null && scene != PrefabStageUtility.GetCurrentPrefabStage()?.scene.name; 
        return isSceneObject;
      }
    }
  }
}