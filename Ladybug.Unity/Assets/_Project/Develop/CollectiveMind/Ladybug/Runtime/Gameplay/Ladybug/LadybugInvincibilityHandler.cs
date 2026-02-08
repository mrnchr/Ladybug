using System.Threading;
using CollectiveMind.Ladybug.Runtime.Infrastructure.Ecs;
using CollectiveMind.Ladybug.Runtime.Utils;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using R3;

namespace CollectiveMind.Ladybug.Runtime.Gameplay.Ladybug
{
  public class LadybugInvincibilityHandler
  {
    public ReadOnlyReactiveProperty<float> Opacity => _opacity;

    private readonly LadybugContext _context;
    private readonly LadybugConfig _config;
    private readonly DOGetter<float> _transparencyGetter;
    private readonly DOSetter<float> _transparencySetter;
    private readonly ReactiveProperty<float> _opacity = new ReactiveProperty<float>(1);

    private LadybugVisual _visual => _context.Visual;
    private EcsEntityWrapper _entity => _visual.Entity;
    private Sequence _blinkingTween;
    private CancellationTokenSource _invincibilityTimeResetCts;

    public LadybugInvincibilityHandler(LadybugContext context, LadybugConfig config)
    {
      _context = context;
      _config = config;

      _transparencyGetter = () => _opacity.Value;
      _transparencySetter = value => _opacity.Value = value;
    }

    public void HandleInvincibility()
    {
      if (_entity.Has<Invincible>())
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
      _entity.Add<Invincible>();
      bool cancelled;

      do
      {
        StartBlinking();
        
        _invincibilityTimeResetCts?.Dispose();
        _invincibilityTimeResetCts = new CancellationTokenSource();
        using var linkedTokenSource =
          CancellationTokenSource.CreateLinkedTokenSource(token, _invincibilityTimeResetCts.Token);
        cancelled = await UniTask
          .WaitForSeconds(_config.InvincibilityTime, cancellationToken: linkedTokenSource.Token)
          .SuppressCancellationThrow();
      } while (cancelled && _visual);

      _invincibilityTimeResetCts.Dispose();
      _invincibilityTimeResetCts = null;

      if (_entity.IsAlive())
      {
        _entity.Del<Invincible>();
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