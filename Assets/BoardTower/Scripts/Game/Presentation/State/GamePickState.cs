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
        private readonly PickRelicUseCase _pickRelicUseCase;

        public GamePickState(LotRelicUseCase lotRelicUseCase, PickRelicUseCase pickRelicUseCase)
        {
            _lotRelicUseCase = lotRelicUseCase;
            _pickRelicUseCase = pickRelicUseCase;
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
            await _pickRelicUseCase.PickAsync(token);
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