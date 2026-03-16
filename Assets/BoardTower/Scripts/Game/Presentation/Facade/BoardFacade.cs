using System.Linq;
using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Presentation.View;
using BoardTower.Game.Utility;
using Cysharp.Threading.Tasks;
using R3;

namespace BoardTower.Game.Presentation.Facade
{
    public sealed class BoardFacade
    {
        private readonly BoardView _boardView;

        public BoardFacade(BoardView boardView)
        {
            _boardView = boardView;
        }

        public UniTask FadeAsync(BoardTransitionVO transition, CancellationToken token)
        {
            var tween = transition.fade switch
            {
                Fade.In => _boardView.FadeInSquareAll(transition.duration),
                Fade.Out => _boardView.FadeOutSquareAll(transition.duration),
                _ => throw new QuitExceptionVO(ExceptionConfig.INVALID_FADE),
            };

            return tween
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public UniTask RenderEventAsync(EventSquareVO[] squares, CancellationToken token)
        {
            var events = squares
                .Select(x => new EventVO(BoardHelper.ToIndex(x.square.file, x.square.rank), x.squareEvent.eventObject))
                .Distinct()
                .ToArray();

            return _boardView.RenderEventSquareAll(events, BoardConfig.EVENT_DURATION)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public UniTask ShowHighlightAsync(HighlightSquareVO[] squares, CancellationToken token)
        {
            var highlights = squares
                .Select(x => new HighlightVO(BoardHelper.ToIndex(x.square.file, x.square.rank), x.highlight))
                .Distinct()
                .ToArray();

            return _boardView.ShowHighlightSquare(highlights, BoardConfig.HIGHLIGHT_DURATION)
                .ToUniTask(TweenCancelBehaviour.KillAndCancelAwait, token);
        }

        public Observable<ClickSquareVO> OnClickAnySquareAsObservable()
        {
            return _boardView.OnClickAnySquareAsObservable()
                .Select(x =>
                {
                    var (file, rank) = BoardHelper.ToFileRank(x);
                    return new ClickSquareVO(file, rank);
                });
        }
    }
}