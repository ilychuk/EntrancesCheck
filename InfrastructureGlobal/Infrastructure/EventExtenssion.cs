using System;

namespace InfrastructureGlobal.Infrastructure
{
    public static class EventExtension
    {
        public static bool TryPerformAction(this EventHandler eventHandler, object sender)
        {
            bool isExistHandler = eventHandler != null;

            if (isExistHandler)
                eventHandler(sender, EventArgs.Empty);

            return isExistHandler;
        }
    }
}