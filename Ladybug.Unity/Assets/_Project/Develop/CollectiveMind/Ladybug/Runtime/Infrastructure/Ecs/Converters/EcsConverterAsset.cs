﻿using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using TriInspector;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs
{
  [CreateAssetMenu(menuName = CAC.Names.ECS_CONVERTER_MENU, fileName = CAC.Names.ECS_CONVERTER_FILE)]
  [HideMonoScript]
  public class EcsConverterAsset : ScriptableObject, IEcsConverter
  {
    public List<EcsConverterValue> Converters;

    public void ConvertTo(EcsEntityWrapper entity)
    {
      foreach (EcsConverterValue converter in Converters)
      {
        converter.ConvertTo(entity);
      }
    }

    public void ConvertBack(EcsEntityWrapper entity)
    {
    }

#if UNITY_EDITOR
    [Button(ButtonSizes.Medium, "Validate Entity")]
    [PropertyOrder(0)]
    [PropertySpace(SpaceAfter = 10)]
    [GUIColor(0f, 1f, 1f)]
    private void ValidateEntity()
    {
      var world = new EcsWorld();
      EcsEntityWrapper entity = world.CreateEntity();
      try
      {
        ConvertTo(entity);
        Debug.Log("Entity was successfully created");
      }
      catch (Exception e)
      {
        Debug.LogException(e);
      }
    }
#endif
  }
}