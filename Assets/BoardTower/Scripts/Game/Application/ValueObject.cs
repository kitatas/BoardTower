using BoardTower.Common.Application;

namespace BoardTower.Game.Application
{
    public sealed class BoardTransitionVO : TransitionVO
    {
        public BoardTransitionVO(Fade fade, float duration = 0) : base(fade, duration)
        {
        }
    }
}