#if DOTWEEN

using GameFlow;
using DG.Tweening;

public class DOTweenTask : Task
{
    private Tween tween;

    public DOTweenTask(Tween tween)
    {
        this.tween = tween;
        tween.Pause();
        tween.onComplete += Complete;
    }

    protected override void OnPlay()
    {
        base.OnPlay();
        tween.Play();
    }

    protected override void OnComplete()
    {
        base.OnComplete();
        tween.Complete();
    }

    protected override void OnKill()
    {
        base.OnKill();
        tween.Kill();
    }

    public static implicit operator DOTweenTask(Tween tween)
    {
        return new DOTweenTask(tween);
    }
}

public static class DOTweenTaskEx
{
    public static void Add(this TaskList list, Tween t)
    {
        list.Add(new DOTweenTask(t));
    }

    public static void Add(this TaskSet set, Tween t)
    {
        set.Add(new DOTweenTask(t));
    }
}

#endif