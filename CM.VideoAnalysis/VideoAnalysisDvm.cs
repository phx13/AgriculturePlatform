using System;
using System.Collections.Generic;
using Digihail.AVE.Launcher.Infrastructure.ObjectSynchronization;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.VideoAnalysis
{
    [Serializable]
    public class VideoAnalysisDvm : ChartDataViewModel
    {
        //private string m_FifthVideoPath;

        //private string m_FirstVideoPath;

        //private string m_FourthVideoPath;

        //private string m_SecondVideoPath;

        //private string m_SixthVideoPath;

        //private string m_ThirdVideoPath;

        ///// <summary>
        /////     样式设置 - 基本文字 - 文字大小
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("视频一地址", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        //public virtual string FirstVideoPath
        //{
        //    get { return m_FirstVideoPath; }
        //    set
        //    {
        //        m_FirstVideoPath = value;
        //        RaisePropertyChanged(() => FirstVideoPath);
        //    }
        //}

        ///// <summary>
        /////     样式设置 - 基本文字 - 文字大小
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("视频二地址", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        //public virtual string SecondVideoPath
        //{
        //    get { return m_SecondVideoPath; }
        //    set
        //    {
        //        m_SecondVideoPath = value;
        //        RaisePropertyChanged(() => SecondVideoPath);
        //    }
        //}

        ///// <summary>
        /////     样式设置 - 基本文字 - 文字大小
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("视频三地址", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        //public virtual string ThirdVideoPath
        //{
        //    get { return m_ThirdVideoPath; }
        //    set
        //    {
        //        m_ThirdVideoPath = value;
        //        RaisePropertyChanged(() => ThirdVideoPath);
        //    }
        //}

        ///// <summary>
        /////     样式设置 - 基本文字 - 文字大小
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("视频四地址", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        //public virtual string FourthVideoPath
        //{
        //    get { return m_FourthVideoPath; }
        //    set
        //    {
        //        m_FourthVideoPath = value;
        //        RaisePropertyChanged(() => FourthVideoPath);
        //    }
        //}

        ///// <summary>
        /////     样式设置 - 基本文字 - 文字大小
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("视频五地址", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        //public virtual string FifthVideoPath
        //{
        //    get { return m_FifthVideoPath; }
        //    set
        //    {
        //        m_FifthVideoPath = value;
        //        RaisePropertyChanged(() => FifthVideoPath);
        //    }
        //}

        ///// <summary>
        /////     样式设置 - 基本文字 - 文字大小
        ///// </summary>
        //[Synchronous]
        //[PropertyDescription("视频六地址", Category = DescriptionEnum.样式设置, SubCategory = "基本样式")]
        //public virtual string SixthVideoPath
        //{
        //    get { return m_SixthVideoPath; }
        //    set
        //    {
        //        m_SixthVideoPath = value;
        //        RaisePropertyChanged(() => SixthVideoPath);
        //    }
        //}

        public override List<DataColumnModel> GetColumns()
        {
            var columns = new List<DataColumnModel>();
            return columns;
        }
    }
}