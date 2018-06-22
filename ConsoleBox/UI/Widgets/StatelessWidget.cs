using ConsoleBox.UI.Elements;

namespace ConsoleBox.UI.Widgets
{
    /// <summary>
    ///  Simple wrapper for end user stateless widgets so not to require any interaction with 'elements'
    /// </summary>
    public abstract class StatelessWidget : IWidget
    {
        public abstract IWidget Create();

        public virtual IElement Build(BuildContext context)
        {
            return Create().Build(context);
        }
    }
}
