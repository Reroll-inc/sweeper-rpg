using System;
using PlantUmlClassDiagramGenerator.Attributes;

namespace EngineGDI.Src.Events
{
    public abstract class Event
    {
        [PlantUmlIgnoreAssociation]
        public DateTime Timestamp { get; } = DateTime.Now;
    }
}
