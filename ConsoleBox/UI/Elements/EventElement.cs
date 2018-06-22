using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    public class EventElement : IElement
    {
        public AreaEventHandler EventHandler { get; set; } = new AreaEventHandler();

        public IElement Child { get; set; }

        public EventElement(IElement child)
        {
            Child = child;
        }

        public Size Draw(Point offset, BoxConstraint constraint, UIContext context)
        {
            var size = Child.Draw(offset, constraint, context);

            if (EventHandler != null)
            {
                // simply wraps the child bounds in an area event handler
                context.AddEventHandler(new UIContext.AreaEventHandlerRegistration()
                {
                    Area = new Rectangle
                    {
                        Location = offset,
                        Size = size,
                    },
                    Handler = EventHandler,
                });
            }

            return size;
        }

        public Size Measure(BoxConstraint constraint)
        {
            return Child.Measure(constraint);
        }
    }
}
