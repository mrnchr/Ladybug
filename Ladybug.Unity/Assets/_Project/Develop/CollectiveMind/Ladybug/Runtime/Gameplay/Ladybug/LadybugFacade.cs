using System.Threading;
using CollectiveMind.Ladybug.Runtime.Gameplay.Session;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Visual;
using CollectiveMind.Ladybug.Runtime.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using R3;
using UnityEngine;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugFacade : IFacade
  {
    public Transform Transform => _visual.transform;
    public Vector3 Velocity { get; private set; }
    public ReadOnlyReactiveProperty<bool> IsMoving => _isMoving;
    public ReadOnlyReactiveProperty<float> Opacity => _opacity;
    
    private readonly ReactiveProperty<float> _opacity = new ReactiveProperty<float>(1);
    private readonly ReactiveProperty<bool> _isMoving = new ReactiveProperty<bool>();
    private readonly LadybugConfig _config;
    private readonly ILadybugRotator _rotator;
    private readonly GameSessionData _sessionData;
    private readonly GameSwitcher _gameSwitcher;
    private readonly DOGetter<float> _transparencyGetter; 
    private readonly DOSetter<float> _transparencySetter;

    private LadybugVisual _visual;
    private EcsEntityWrapper Entity => _visual.Entity;
    private Sequence _blinkingTween;
    private CancellationTokenSource _invincibilityTimeResetCts;

    public LadybugFacade(LadybugConfig config,
      ILadybugRotator rotator,
      GameSessionData sessionData,
      GameSwitcher gameSwitcher)
    {
      _config = config;
      _rotator = rotator;
      _sessionData = sessionData;
      _gameSwitcher = gameSwitcher;

      _transparencyGetter = () => _opacity.Value;
      _transparencySetter = value => _opacity.Value = value;
    }

    public void Bind(LadybugVisual visual)
    {
      _visual = visual;
      _sessionData.Health
        .Pairwise()
        .Where(pair => pair.Current < pair.Previous)
        .Select(pair => pair.Current)
        .Subscribe(OnHealthChanged)
        .AddTo(_visual);
    }

    public void CheckBounds()
    {
      _rotator.CheckBounds();
    }

    public void UpdateVelocity(Vector3 direction)
    {
      Velocity = direction * (_config.Speed * _sessionData.SpeedRate.Value);
      _isMoving.Value = Velocity != Vector3.zero;
    }

    private void OnHealthChanged(int health)
    {
      if (health <= 0)
      {
        _gameSwitcher.Defeat().Forget();
        return;
      }
      
      if (Entity.Has<Invincible>())
      {
        _invincibilityTimeResetCts.Cancel();
      }
      else
      {
        RunInvincibilityAsync(_visual.GetCancellationTokenOnDestroy()).Forget();
      }
    }

    private async UniTask RunInvincibilityAsync(CancellationToken token = default(CancellationToken))
    {
      Entity.Add<Invincible>();
      StartBlinking();
      bool cancelled;
      
      do
      {
        _invincibilityTimeResetCts?.Dispose();
        _invincibilityTimeResetCts = new CancellationTokenSource();
        using var linkedTokenSource = CancellationTokenSource.CreateLinkedTokenSource(token, _invincibilityTimeResetCts.Token);
        cancelled = await UniTask
          .WaitForSeconds(_config.InvincibilityTime, cancellationToken: linkedTokenSource.Token)
          .SuppressCancellationThrow();
      } while (cancelled && _visual);

      _invincibilityTimeResetCts.Dispose();
      _invincibilityTimeResetCts = null;
      
      if(Entity.IsAlive())
      {
        Entity.Del<Invincible>();
      }

      if (!_visual)
      {
        return;
      }
      
      StopBlinking();
    }

    private void StartBlinking()
    {
      StopBlinking();

      float tweenTime = MathUtils.DivideOrZero(_config.InvincibilityTime, _config.BlinkCount);
      _blinkingTween = DOTween.Sequence()
        .Append(DOTween.To(_transparencyGetter, _transparencySetter, 0, tweenTime))
        .SetLoops(-1, LoopType.Yoyo)
        .SetLink(_visual.gameObject, LinkBehaviour.KillOnDestroy)
        .Play();
    }

    private void StopBlinking()
    {
      _blinkingTween?.Kill();
      _blinkingTween = null;
      _opacity.Value = 1;
    }
  }
}