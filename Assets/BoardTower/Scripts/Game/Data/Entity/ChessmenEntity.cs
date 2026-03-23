using BoardTower.Game.Application;

namespace BoardTower.Game.Data.Entity
{
    public sealed class ChessmenEntity
    {
        private ChessmenVO _chessmen;

        public ChessmenEntity()
        {
            _chessmen = Create();
        }

        public ChessmenType chessmenType => _chessmen.type;
        public SquareVO square => _chessmen.square;

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
            Set(new SquareVO(square.file + offset.dx, square.rank + offset.dy));
        }

        public ChessmenMovementVO movement => new(square);
    }
}