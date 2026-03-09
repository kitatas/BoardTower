using System.Collections.Generic;
using System.IO;
using BoardTower.Game.Application;
using BoardTower.Game.Data.DataStore;
using FastEnumUtility;
using MessagePack;
using MessagePack.Resolvers;
using UnityEditor;

namespace BoardTower.Editor
{
    public static class BinaryGenerator
    {
        [MenuItem("Tools/MasterMemory/" + nameof(BuildBinary))]
        private static void BuildBinary()
        {
            StaticCompositeResolver.Instance.Register(
                MasterMemoryResolver.Instance,
                GeneratedResolver.Instance,
                StandardResolver.Instance
            );

            var options = MessagePackSerializerOptions.Standard.WithResolver(StaticCompositeResolver.Instance);
            MessagePackSerializer.DefaultOptions = options;

            var databaseBuilder = new DatabaseBuilder();
            databaseBuilder.Append(GetBoardPatternMaster());
            databaseBuilder.Append(GetChessmenMovementRuleMaster());

            var path = "Assets/Externals/Binary/MasterMemory.bytes";
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllBytes(path, databaseBuilder.Build());
            AssetDatabase.Refresh();
        }

        private static IEnumerable<BoardPatternMaster> GetBoardPatternMaster()
        {
            return new[]
            {
                new BoardPatternMaster(1, new[]
                {
                    SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                }),
                new BoardPatternMaster(2,new[]
                {
                    SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(),
                    SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(),
                }),
                new BoardPatternMaster(3, new[]
                {
                    SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                }),
                new BoardPatternMaster(4, new[]
                {
                    SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                    SquareEventType.Empty.ToInt32(), SquareEventType.Block.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                }),
                // new BoardPatternMaster(5, new[]
                // {
                //     SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                //     SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                //     SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                //     SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(), SquareEventType.Empty.ToInt32(),
                // }),
            };
        }

        private static IEnumerable<ChessmenMovementRuleMaster> GetChessmenMovementRuleMaster()
        {
            return new[]
            {
                new ChessmenMovementRuleMaster(ChessmenType.King.ToInt32(), ChessmenMovementType.Leaper.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 0),
                        (dx: -1, dy: 1),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 0),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Queen.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 0),
                        (dx: -1, dy: 1),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 0),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Rook.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: 0),
                        (dx: 1, dy: 0),
                        (dx: 0, dy: -1),
                        (dx: 0, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Bishop.ToInt32(), ChessmenMovementType.Slider.ToInt32(),
                    new[]
                    {
                        (dx: -1, dy: -1),
                        (dx: -1, dy: 1),
                        (dx: 1, dy: -1),
                        (dx: 1, dy: 1),
                    }),
                new ChessmenMovementRuleMaster(ChessmenType.Knight.ToInt32(), ChessmenMovementType.Leaper.ToInt32(),
                    new[]
                    {
                        (dx: -2, dy: -1),
                        (dx: -2, dy: 1),
                        (dx: -1, dy: -2),
                        (dx: -1, dy: 2),
                        (dx: 1, dy: -2),
                        (dx: 1, dy: 2),
                        (dx: 2, dy: -1),
                        (dx: 2, dy: 1),
                    }),
            };
        }
    }
}