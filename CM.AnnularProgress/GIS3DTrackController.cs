using Digihail.AVE.Playback;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.DataViewModels;
using Digihail.DAD3.Models.Interfaces;

namespace CM.AnnularProgress.GISPlayBack
{
    /// <summary>
    ///     3D节点轨迹图控制器
    /// </summary>
    // Token: 0x02000019 RID: 25
    public class GIS3DTrackController : GIS3DPlaybackController // GIS3DControllerBase
    {
        /// <summary>
        ///     构造函数
        /// </summary>
        /// <param name="dvm"></param>
        /// <param name="dataProxy"></param>
        /// <param name="player"></param>
        // Token: 0x06000170 RID: 368 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
        public GIS3DTrackController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
        {
            //this.m_DVM = (base.DataViewModel as GIS3DTrackDataViewModel);
            //this.SetBaseValue();
            //this.ReceiveMessages();
        }

        /// <summary>
        ///     接收数据
        /// </summary>
        /// <param name="adt"></param>
        // Token: 0x06000171 RID: 369 RVA: 0x0000DD8E File Offset: 0x0000BF8E
        public override void ReceiveData(AdapterDataTable adt)
        {
            //this.InitGlobe();
            //this.InitOtherStyle();
            //this.UpdateByDataTable(adt);
        }
    }
}