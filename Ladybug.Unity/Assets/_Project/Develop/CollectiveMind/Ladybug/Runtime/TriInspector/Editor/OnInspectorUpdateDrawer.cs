using System;
using System.Reflection;
using CollectiveMind.Ladybug.Runtime.TriInspector.Editor;
using TriInspector;
using TriInspector.Resolvers;
using UnityEngine;

[assembly: RegisterTriAttributeDrawer(typeof(OnInspectorUpdateDrawer), TriDrawerOrder.System)]

namespace CollectiveMind.Ladybug.Runtime.TriInspector.Editor
{
  public class OnInspectorUpdateDrawer : TriAttributeDrawer<OnInspectorUpdate>
  {
    private ActionResolver _actionResolver;
    private MethodInfo _methodInfo;

    public override TriExtensionInitializationResult Initialize(TriPropertyDefinition propertyDefinition)
    {
      base.Initialize(propertyDefinition);

      _actionResolver = ActionResolver.Resolve(propertyDefinition, Attribute.Method);
      bool isMethodInfo = false;

      if (propertyDefinition.TryGetMemberInfo(out MemberInfo memberInfo) && memberInfo is MethodInfo methodInfo)
      {
        isMethodInfo = true;
        _methodInfo = methodInfo;
      }
      
      if (_actionResolver.TryGetErrorString(out string error) && !isMethodInfo)
      {
        return error;
      }

      return TriExtensionInitializationResult.Ok;
    }

    public override TriElement CreateElement(TriProperty property, TriElement next)
    {
      return new OnInspectorUpdateListenerElement(property, next, _actionResolver, _methodInfo);
    }

    private class OnInspectorUpdateListenerElement : TriElement
    {
      private readonly TriProperty _property;
      private readonly ActionResolver _actionResolver;
      private readonly MethodInfo _methodInfo;
      private readonly Action _invokeAction;

      public OnInspectorUpdateListenerElement(TriProperty property,
        TriElement next,
        ActionResolver actionResolver,
        MethodInfo methodInfo)
      {
        _property = property;
        _actionResolver = actionResolver;
        _methodInfo = methodInfo;

        if (!_actionResolver.TryGetErrorString(out _))
        {
          _invokeAction = InvokeAction;
        }
        else if (_methodInfo != null)
        {
          _invokeAction = InvokeMethod;
        }
        
        AddChild(next);
      }

      public override bool Update()
      {
        _invokeAction?.Invoke();
        return base.Update();
      }
      
      private void InvokeAction()
      {
        _property.PropertyTree.ApplyChanges();
        _actionResolver.InvokeForAllTargets(_property);
        _property.PropertyTree.Update(forceUpdate: true);
        _property.NotifyValueChanged();
        _property.PropertyTree.RequestValidation();
        _property.PropertyTree.RequestRepaint();
      }

      private void InvokeMethod()
      {
        if (_methodInfo != null)
        {
          _property.PropertyTree.ApplyChanges();

          for (var targetIndex = 0; targetIndex < _property.PropertyTree.TargetsCount; targetIndex++)
          {
            try
            {
              object parentValue = _property.Parent.GetValue(targetIndex);
              _methodInfo.Invoke(parentValue, null);
            }
            catch (Exception e)
            {
              Debug.LogException(e);
            }
          }

          _property.PropertyTree.Update(forceUpdate: true);
          _property.NotifyValueChanged();
          _property.PropertyTree.RequestValidation();
          _property.PropertyTree.RequestRepaint();
        }
      }
    }
  }
}