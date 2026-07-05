using System;
using System.Collections.Generic;
using EngineGDI.Src.Events;
using PlantUmlClassDiagramGenerator.Attributes;

namespace SweeperRpg.Src
{
    public class EnemyFactory
    {
        [PlantUmlIgnoreAssociation]
        private readonly Dictionary<EnemyKind, Enemy> EnemyPrototypes = [];

        public EnemyFactory(EventBus bus)
        {
            RecreatePrototypes(bus: bus);
        }

        public Enemy Create(int x, int y, EnemyKind kind)
        {
            _ = EnemyPrototypes.TryGetValue(kind, out Enemy enemy);

            return enemy.Clone(x: x, y: y);
        }

        private void RecreatePrototypes(EventBus bus)
        {
            foreach (EnemyKind kind in Enum.GetValues<EnemyKind>())
            {
                EnemyPrototypes.Add(key: kind, value: new(x: 0, y: 0, kind: kind, bus: bus));
            }
        }
    }
}
