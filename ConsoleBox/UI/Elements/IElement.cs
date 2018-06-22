using System.Drawing;

namespace ConsoleBox.UI.Elements
{
    /// <summary>
    /// A renderable component
    /// </summary>
    public interface IElement
    {
        /// <summary>
        /// Calculates the smallest area the component will take up given the available space.
        /// May still return _larger_ than the available given
        /// </summary>
        Size Measure(BoxConstraint constraint);

        /// <summary>
        /// Renders the component given the final size area.
        /// </summary>
        Size Draw(Point offset, BoxConstraint constraint, UIContext context);
        
    }
}
