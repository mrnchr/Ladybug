﻿using System.Collections.Generic;
using CollectiveMind.Ladybug.Runtime;

namespace CollectiveMind.Ladybug.Editor.General
{
  public class EditorInitializer : IEditorInitializer
  {
    private readonly List<IEditorInitializable> _initializables = new List<IEditorInitializable>();

    private bool _initialized;

    public EditorInitializer(List<IEditorInitializable> initializables, EditorContext context)
    {
      _initializables.AddRange(initializables);
      context.AddListener(Initialize);
    }

    public void Add(IEditorInitializable initializable)
    {
      _initializables.Add(initializable);
      
      if(_initialized)
        initializable.Initialize();
    }

    private void Initialize()
    {
      foreach (IEditorInitializable initializable in _initializables)
      {
        initializable.Initialize();
      }
      
      _initialized = true;
    }
  }
}