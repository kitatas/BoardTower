namespace BoardTower.Common.Application
{
    public sealed class TransitionVO
    {
        public readonly Fade fade;
        public readonly float duration;

        public TransitionVO(Fade fade, float duration = 0.0f)
        {
            this.fade = fade;
            this.duration = duration;
        }
    }
}