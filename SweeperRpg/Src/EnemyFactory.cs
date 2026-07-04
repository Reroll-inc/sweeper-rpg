using System;
using System.Collections.Generic;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    public class EnemyFactory
    {
        [PlantUmlIgnoreAssociation]
        private readonly Dictionary<EnemyKind, Enemy> EnemyPrototypes = [];

        private static EnemyFactory Instance { get; } = new();

        private EnemyFactory()
        {
            RecreatePrototypes();
        }

        public static Enemy Create(int x, int y, EnemyKind kind)
        {
            _ = Instance.EnemyPrototypes.TryGetValue(kind, out Enemy enemy);

            return enemy.Clone(x: x, y: y);
        }

        private void RecreatePrototypes()
        {
            foreach (EnemyKind kind in Enum.GetValues<EnemyKind>())
            {
                EnemyPrototypes.Add(kind, new Enemy(0, 0, kind));
            }
        }
    }
}
