using System.Windows;

namespace CM_Flash.Controls
{
    public class AnimationCompletedEventArgs : RoutedEventArgs
    {
        #region Constructors

        /// <summary>
        ///     默认构造函数。
        /// </summary>
        public AnimationCompletedEventArgs(RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
        }

        #endregion
    }
}