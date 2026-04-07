using BoardTower.Game.Application;
using BoardTower.Game.Utility;

namespace BoardTower.Game.Data.Entity
{
    public sealed class ChessmenEntity
    {
        private ChessmenVO _chessmen;

        public ChessmenEntity()
        {
            Reset();
        }

        public ChessmenType chessmenType => _chessmen.type;
        public SquareVO square => _chessmen.square;

        public void Reset()
        {
            _chessmen = Create();
        }

        private static ChessmenVO Create()
        {
            return new ChessmenVO(
                ChessmenConfig.DEFAULT_TYPE,
                new SquareVO(BoardConfig.MIN_FILE, BoardConfig.MIN_RANK)
            );
        }

        public void Set(SquareVO vo)
        {
            _chessmen = new ChessmenVO(_chessmen, vo);
        }

        public void MoveBy(ChessmenMovementOffsetVO offset)
        {
            Set(CalcMovementOffset(offset));
        }

        public SquareVO CalcMovementOffset(ChessmenMovementOffsetVO offset)
        {
            return ChessmenHelper.CalcSquare(square, offset);
        }

        public ChessmenMovementVO movement => new(square);
    }
}