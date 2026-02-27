using BoardTower.Game.Application;
using MessagePipe;

namespace BoardTower.Game.Domain.Ports
{
    public sealed class MovementPorts
    {
        public readonly IAsyncSubscriber<HighlightSquareVO[]> highlightsSubscriber;
        public readonly IAsyncPublisher<HighlightSquareVO[]> highlightsPublisher;

        public MovementPorts(IAsyncSubscriber<HighlightSquareVO[]> highlightsSubscriber,
            IAsyncPublisher<HighlightSquareVO[]> highlightsPublisher)
        {
            this.highlightsSubscriber = highlightsSubscriber;
            this.highlightsPublisher = highlightsPublisher;
        }
    }
}