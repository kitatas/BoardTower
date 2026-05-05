using System.Threading;
using BoardTower.Common.Application;
using BoardTower.Game.Application;
using BoardTower.Game.Domain.UseCase;
using Cysharp.Threading.Tasks;

namespace BoardTower.Game.Presentation.State
{
    public sealed class GamePickState : BaseGameState
    {
        private readonly LotRelicUseCase _lotRelicUseCase;

        public GamePickState(LotRelicUseCase lotRelicUseCase)
        {
            _lotRelicUseCase = lotRelicUseCase;
        }

        public override GameState state => GameState.Pick;

        public override async UniTask InitAsync(CancellationToken token)
        {
            await (
                _lotRelicUseCase.InitAsync(token)
            );
        }

        public override async UniTask<GameState> TickAsync(CancellationToken token)
        {
            await _lotRelicUseCase.FadeAsync(Fade.In, token);
            // TODO: 選択待ち
            await _lotRelicUseCase.FadeAsync(Fade.Out, token);

            return GameState.SetUp;
        }

        public override async UniTask ForceExitAsync(CancellationToken token)
        {
            await (
                _lotRelicUseCase.FadeAsync(Fade.Out, token)
            );
        }
    }
}