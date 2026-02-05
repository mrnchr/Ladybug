using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugSkin : MonoBehaviour
  {
    private static readonly int _walkSpeed = Animator.StringToHash("WalkSpeed");
    
    [SerializeField]
    private List<Renderer> _renderers;
    
    [field: SerializeField]
    public CapsuleCollider CapsuleCollider { get; private set; }

    [SerializeField]
    private Animator _animator;

    private readonly List<MaterialSlot> _slots = new List<MaterialSlot>();
    private readonly Dictionary<Material, Material> _swapMap = new Dictionary<Material, Material>();
    private readonly List<Material> _liveMaterials = new List<Material>();
    private LadybugConfig _ladybugConfig;

    [Inject]
    public void Construct(LadybugConfig ladybugConfig)
    {
      _ladybugConfig = ladybugConfig;
    }

    public void Initialize()
    {
      foreach (MaterialPair pair in _ladybugConfig.InterchangeableMaterials)
      {
        _swapMap[pair.Opaque] = pair.Transparent;
        _swapMap[pair.Transparent] = pair.Opaque;
      }

      for (int r = 0; r < _renderers.Count; r++)
      {
        Renderer renderer = _renderers[r];
        Material[] sharedAssets = renderer.sharedMaterials;
        Material[] instances = renderer.materials;
        int length = Mathf.Min(sharedAssets.Length, instances.Length);

        for (int m = 0; m < length; m++)
        {
          Material sharedAsset = sharedAssets[m];
          Material instance = instances[m];
          Color c = instance.color;

          _slots.Add(new MaterialSlot
          {
            RendererIndex = r,
            MaterialIndex = m,
            SharedMaterial = sharedAsset,
            InstanceMaterial = instance,
            OriginalAlpha = c.a
          });
        }
      }
    }

    public void ApplyOpacity(float minAlpha, float opacity)
    {
      for (int s = 0; s < _slots.Count; s++)
      {
        MaterialSlot slot = _slots[s];

        Renderer renderer = _renderers[slot.RendererIndex];
        _liveMaterials.Clear();
        renderer.GetMaterials(_liveMaterials);
        Material live = _liveMaterials[slot.MaterialIndex];

        if (!ReferenceEquals(live, slot.InstanceMaterial))
        {
          slot.InstanceMaterial = live;
        }

        Material currentInstance = slot.InstanceMaterial;

        float targetAlpha = Mathf.Lerp(minAlpha, slot.OriginalAlpha, opacity);
        float currentAlpha = currentInstance.color.a;

        bool crossToOpaque = currentAlpha < 1f && targetAlpha >= 1f;
        bool crossToTransparent = currentAlpha >= 1f && targetAlpha < 1f;

        if (crossToOpaque || crossToTransparent)
        {
          Material otherShared = _swapMap[slot.SharedMaterial];
          Material newInstance = SwapSlotMaterial(renderer, slot.MaterialIndex, otherShared);

          slot.SharedMaterial = otherShared;
          slot.InstanceMaterial = newInstance;
          currentInstance = newInstance;
        }

        Color color = currentInstance.color;
        color.a = targetAlpha;
        currentInstance.color = color;

        _slots[s] = slot;
      }
    }

    public void SetWalkAnimationSpeed(float speed)
    {
      _animator.SetFloat(_walkSpeed, speed);
    }

    private Material SwapSlotMaterial(Renderer renderer, int materialIndex, Material newSharedAsset)
    {
      Material[] materials = renderer.materials;
      Material newInstance = new Material(newSharedAsset);
      materials[materialIndex] = newInstance;
      renderer.materials = materials;
      return newInstance;
    }

    [Serializable]
    private struct MaterialSlot
    {
      public int RendererIndex;
      public int MaterialIndex;
      public Material SharedMaterial;
      public Material InstanceMaterial;
      public float OriginalAlpha;
    }
  }
}