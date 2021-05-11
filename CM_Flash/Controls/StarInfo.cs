using System.Windows;

namespace CM_Flash.Controls
{
    public class StarInfo
    {
        /// <summary>
        ///     X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///     X轴速度(单位距离/帧)
        /// </summary>
        public double XV { get; set; }

        /// <summary>
        ///     X坐标以X轴速度运行的时间(帧)
        /// </summary>
        public int XT { get; set; }

        /// <summary>
        ///     Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        ///     Y轴速度(单位距离/帧)
        /// </summary>
        public double YV { get; set; }

        /// <summary>
        ///     Y坐标以Y轴速度运行的时间(帧)
        /// </summary>
        public int YT { get; set; }

        /// <summary>
        ///     对星星的引用
        /// </summary>
        //public Path StarRef { get; set; }
        ///// <summary>
        ///// 对圆的引用
        ///// </summary>
        public FrameworkElement StarRef { get; set; }
    }
}