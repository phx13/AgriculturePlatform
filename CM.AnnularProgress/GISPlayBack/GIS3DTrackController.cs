using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using Digihail.AVE.Controls.GIS3D.OSG.Engine;
using Digihail.AVE.Controls.GIS3D.OSG.Engine.Models;
using Digihail.AVE.Launcher.Infrastructure.Communiction;
using Digihail.AVE.Playback;
using Digihail.AVE.Playback.Models;
using Digihail.AVECLI.Controls.GIS3D.Core;
using Digihail.AVECLI.Media3D.EntityFramework;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.BillboardStyles;
using Digihail.AVECLI.Media3D.EntityFramework.EntityComponent.Visual.LineStyles;
using Digihail.DAD3.Charts.Base;
using Digihail.DAD3.Charts.GIS3D.Controllers;
using Digihail.DAD3.Charts.GIS3D.Models;
using Digihail.DAD3.Charts.GIS3D.Utils;
using Digihail.DAD3.Charts.Message;
using Digihail.DAD3.Charts.Models;
using Digihail.DAD3.Charts.Utils;
using Digihail.DAD3.Models;
using Digihail.DAD3.Models.DataAdapter;
using Digihail.DAD3.Models.Interfaces;
using OpenTK;
using Digihail.DAD3.Models.DataViewModels;

namespace CM.AnnularProgress.GISPlayBack
{
	/// <summary>
	/// 3D节点轨迹图控制器
	/// </summary>
	// Token: 0x02000019 RID: 25
	public class GIS3DTrackController : GIS3DControllerBase
	{
		/// <summary>
		/// 当前控制器的dvm
		/// </summary>
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600016D RID: 365 RVA: 0x0000DC70 File Offset: 0x0000BE70
		public GIS3DTrackDataViewModel DVM
		{
			get
			{
				return this.m_DVM;
			}
		}

		/// <summary>
		/// 要素点图层
		/// </summary>
		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600016E RID: 366 RVA: 0x0000DC88 File Offset: 0x0000BE88
		public LayerModel Layer
		{
			get
			{
				return this.m_Layer;
			}
		}

		/// <summary>
		/// 当前管理的全部对象
		/// </summary>
		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000DCA0 File Offset: 0x0000BEA0
		public ConcurrentDictionary<string, GIS3DPlaybackModel> CurrentModelDictionary
		{
			get
			{
				return this.m_CurrentModelDictionary;
			}
		}

		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="dvm"></param>
		/// <param name="dataProxy"></param>
		/// <param name="player"></param>
		// Token: 0x06000170 RID: 368 RVA: 0x0000DCB8 File Offset: 0x0000BEB8
        public GIS3DTrackController(ChartDataViewModel dvm, IDataProxy dataProxy, IPlayable player)
            : base(dvm, dataProxy, player)
		{
			this.m_DVM = (base.DataViewModel as GIS3DTrackDataViewModel);
			this.SetBaseValue();
			this.ReceiveMessages();
		}

		/// <summary>
		/// 接收数据
		/// </summary>
		/// <param name="adt"></param>
		// Token: 0x06000171 RID: 369 RVA: 0x0000DD8E File Offset: 0x0000BF8E
		public override void ReceiveData(AdapterDataTable adt)
		{
			this.InitGlobe();
			this.InitOtherStyle();
			this.UpdateByDataTable(adt);
		}

		/// <summary>
		/// 刷新图表
		/// </summary>
		/// <param name="dvm"></param>
		// Token: 0x06000172 RID: 370 RVA: 0x0000DDA8 File Offset: 0x0000BFA8
		public override void RefreshChart(ChartDataViewModel dvm)
		{
			List<string> toRemoveModelKeyCollection = this.m_CurrentModelDictionary.Keys.ToList<string>();
			this.RemoveModelByKeys(toRemoveModelKeyCollection);
			this.m_CurrentModelDictionary.Clear();
			this.SetBaseValue();
		}

		/// <summary>
		/// 清空图表
		/// </summary>
		/// <param name="dvm"></param>
		// Token: 0x06000173 RID: 371 RVA: 0x0000DDE2 File Offset: 0x0000BFE2
		public override void ClearChart(ChartDataViewModel dvm)
		{
			this.RemoveAll(false);
		}

		/// <summary>
		/// 属性变更设置
		/// </summary>
		// Token: 0x06000174 RID: 372 RVA: 0x0000DDF0 File Offset: 0x0000BFF0
		public override void RefreshStyle()
		{
			base.SetGlobeStyleConfig(this.m_DVM);
			base.SetGlobeStyle(this.m_DVM);
			this.UpdateTextStyle();
			this.UpdateIconStyle();
			foreach (GIS3DPlaybackModel model in this.m_CurrentModelDictionary.Values)
			{
				model.PostProcessThreshold = (float)this.m_DVM.PostProcessThreshold;
				model.UpdateHeightMap();
				model.UpdateGeographicCoordinateTransform();
				model.ImageWidth = this.m_IconNameToIconStyle[model.IconName].ImageWidth;
				model.ImageHeight = this.m_IconNameToIconStyle[model.IconName].ImageHeight;
				model.SelectedImageWidth = this.m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
				model.SelectedImageHeight = this.m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;
				model.IsVisible = this.m_DVM.ShowLayer;
				model.ShowLabel = this.m_DVM.ShowLabel;
				model.UpdateBillboard();
				model.UpdateTrailComponent();
			}
		}

		/// <summary>
		/// 更新样式
		/// </summary>
		/// <param name="propertyDescription"></param>
		// Token: 0x06000175 RID: 373 RVA: 0x0000DF40 File Offset: 0x0000C140
		public override void RefreshStyle(PropertyDescription propertyDescription)
		{
			if (propertyDescription.Category == "数据设置" && propertyDescription.SubCategory == "其他" && propertyDescription.DisplayName == "启用贴地")
			{
				foreach (GIS3DPlaybackModel model in this.m_CurrentModelDictionary.Values)
				{
					model.UpdateHeightMap();
				}
			}
			if (propertyDescription.SubCategory == "地图样式")
			{
				base.SetGlobeStyleConfig(this.m_DVM);
				base.SetGlobeStyle(this.m_DVM);
			}
			else if (propertyDescription.SubCategory == "位置偏移")
			{
				foreach (GIS3DPlaybackModel model in this.m_CurrentModelDictionary.Values)
				{
					model.UpdateGeographicCoordinateTransform();
				}
			}
			else
			{
				if (propertyDescription.SubCategory == "标识")
				{
					if (propertyDescription.DisplayName == "类别图标")
					{
						this.UpdateIconStyle();
						foreach (GIS3DPlaybackModel model in this.m_CurrentModelDictionary.Values)
						{
							model.ImageWidth = this.m_IconNameToIconStyle[model.IconName].ImageWidth;
							model.ImageHeight = this.m_IconNameToIconStyle[model.IconName].ImageHeight;
							model.SelectedImageWidth = this.m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
							model.SelectedImageHeight = this.m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;
							model.UpdateBillboard();
						}
					}
					else if (propertyDescription.DisplayName == "后期特效过曝光度")
					{
						foreach (GIS3DPlaybackModel model in this.m_CurrentModelDictionary.Values)
						{
							model.PostProcessThreshold = (float)this.m_DVM.PostProcessThreshold;
						}
					}
				}
				if (propertyDescription.SubCategory == "放缩显示")
				{
					this.UpdateIconStyle();
					this.UpdateTextStyle();
				}
				bool isVisible = false;
				foreach (GIS3DPlaybackModel model in this.m_CurrentModelDictionary.Values)
				{
					if (this.m_DVM.ShowLayer && model.LegendItemChecked)
					{
						isVisible = true;
					}
					model.IsVisible = isVisible;
					model.ShowLabel = this.m_DVM.ShowLabel;
					model.UpdateBillboard();
					model.UpdateTrailComponent();
				}
			}
		}

		/// <summary>
		/// 时间轴停止时
		/// </summary>
		// Token: 0x06000176 RID: 374 RVA: 0x0000E2C0 File Offset: 0x0000C4C0
		public override void OnAVEPlayerStoped()
		{
			this.RemoveAll(true);
		}

		/// <summary>
		/// 设置搜索结果
		/// </summary>
		/// <param name="adt"></param>
		// Token: 0x06000177 RID: 375 RVA: 0x0000E2CC File Offset: 0x0000C4CC
		public override void SetSearchResult(AdapterDataTable adt)
		{
			if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
			{
				foreach (GIS3DPlaybackModel model in this.m_CurrentModelDictionary.Values)
				{
					model.IsShinning = false;
				}
			}
			else
			{
				foreach (AdapterDataRow row in adt.Rows)
				{
					string key = this.GetAdapterDataRowKey(row);
					if (this.m_CurrentModelDictionary.ContainsKey(key))
					{
						this.m_CurrentModelDictionary[key].IsShinning = true;
					}
				}
			}
		}

		/// <summary>
		/// 更新dvm
		/// </summary>
		/// <param name="dvm"></param>
		// Token: 0x06000178 RID: 376 RVA: 0x0000E3D4 File Offset: 0x0000C5D4
		public override void UpdateDataViewModel(ChartDataViewModel dvm)
		{
			this.m_DVM = (dvm as GIS3DTrackDataViewModel);
			this.SetBaseValue();
			this.RefreshStyle();
		}

		/// <summary>
		/// 图表被联动时进行选中
		/// </summary>
		/// <param name="adt"></param>
		// Token: 0x06000179 RID: 377 RVA: 0x0000E3F4 File Offset: 0x0000C5F4
		public override void SetSelectedItem(AdapterDataTable adt)
		{
			if (this.m_EngineContainer != null && this.m_DVM != null && adt != null && adt.Rows != null && adt.Rows.Count > 0)
			{
				string key = adt.Rows[0][this.m_DVM.GroupKeyField.AsName].ToString();
				if (this.m_CurrentModelDictionary.ContainsKey(key))
				{
					this.m_CurrentModelDictionary[key].IsSelected = true;
					this.m_EngineContainer.CurrentSelectedModel = this.m_CurrentModelDictionary[key];
					this.m_EngineContainer.OnPanToCommand(this.m_EngineContainer.CurrentSelectedModel);
				}
			}
		}

		/// <summary>
		/// 图表取消联动时取消选中
		/// </summary>
		// Token: 0x0600017A RID: 378 RVA: 0x0000E4B8 File Offset: 0x0000C6B8
		public override void ClearSelectedItem()
		{
			if (this.m_DVM != null && this.m_EngineContainer != null && this.m_EngineContainer.CurrentSelectedModel != null && this.m_DVM.IsLinkage)
			{
				this.m_EngineContainer.CurrentSelectedModel.IsSelected = false;
				this.m_EngineContainer.CurrentSelectedModel = null;
			}
		}

		/// <summary>
		/// 界面进行更新
		/// </summary>
		/// <param name="adt"></param>
		// Token: 0x0600017B RID: 379 RVA: 0x0000E518 File Offset: 0x0000C718
		private void UpdateByDataTable(AdapterDataTable adt)
		{
			List<string> toRemoveModelKeyCollection = this.m_CurrentModelDictionary.Keys.ToList<string>();
			if (adt == null || adt.Rows == null || adt.Rows.Count == 0)
			{
				this.RemoveModelByKeys(toRemoveModelKeyCollection);
			}
			else
			{
				if (base.IsPlayerJump)
				{
					this.RemoveModelByKeys(toRemoveModelKeyCollection);
					base.IsPlayerJump = false;
				}
				Dictionary<string, AdapterDataRow> datas = new Dictionary<string, AdapterDataRow>();
				foreach (AdapterDataRow row in adt.Rows)
				{
					string key = this.GetAdapterDataRowKey(row);
					if (!string.IsNullOrEmpty(key))
					{
						if (datas.ContainsKey(key))
						{
							if (this.m_DVM.DataTimeColumn != null)
							{
								DateTime dt = Convert.ToDateTime(datas[key][this.m_DVM.DataTimeColumn.AsName]);
								DateTime dt2 = Convert.ToDateTime(row[this.m_DVM.DataTimeColumn.AsName]);
								long de = (long)(dt2 - dt).TotalSeconds;
								if (de > 0L)
								{
									datas[key] = row;
								}
							}
							else
							{
								datas[key] = row;
							}
						}
						else
						{
							datas.Add(key, row);
						}
					}
				}
				this.m_PresentPlayStep = base.GetPlayStep();
				foreach (KeyValuePair<string, AdapterDataRow> pair in datas)
				{
					if (this.m_CurrentModelDictionary.ContainsKey(pair.Key))
					{
						GIS3DPlaybackModel model = this.m_CurrentModelDictionary[pair.Key];
						if (this.m_DVM.DataTimeColumn != null && Math.Abs((base.Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds) > this.m_PresentPlayStep * 100.0)
						{
							ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "实时模式下超时重新添加点", string.Format("时间轴步长 {0} ， 时间轴时间与数据时间戳的差值为{1} , 发现时间为{2}", this.m_PresentPlayStep, Math.Abs((base.Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds), model.OccurDateTime.ToShortTimeString()));
							toRemoveModelKeyCollection.Remove(pair.Key);
							this.RemoveModel(pair.Key, model);
							this.AddModel(pair.Key, pair.Value);
						}
						else
						{
							toRemoveModelKeyCollection.Remove(pair.Key);
							this.SetPropertyValue(pair.Value, this.m_CurrentModelDictionary[pair.Key], true);
						}
					}
					else
					{
						this.AddModel(pair.Key, pair.Value);
					}
				}
				this.RemoveModelByKeys(toRemoveModelKeyCollection);
				if (base.IsPlayerJump)
				{
					base.IsPlayerJump = false;
				}
				this.m_ClearObject = false;
			}
		}

		/// <summary>
		/// 获取数据唯一键值
		/// </summary>
		/// <param name="dataRow"></param>
		/// <returns></returns>
		// Token: 0x0600017C RID: 380 RVA: 0x0000E8B0 File Offset: 0x0000CAB0
		private string GetAdapterDataRowKey(AdapterDataRow dataRow)
		{
			string result;
			if (dataRow == null)
			{
				result = "";
			}
			else
			{
				object groupKeyValue = dataRow[this.m_GroupKeyName];
				if (groupKeyValue == null)
				{
					result = "";
				}
				else
				{
					result = groupKeyValue.ToString();
				}
			}
			return result;
		}

		/// <summary>
		/// 添加散点
		/// </summary>
		/// <param name="dataKey"></param>
		/// <param name="row"></param>
		// Token: 0x0600017D RID: 381 RVA: 0x0000E8FC File Offset: 0x0000CAFC
		private void AddModel(string dataKey, AdapterDataRow row)
		{
			GIS3DPlaybackModel model = this.m_EngineContainer.CreateObjectModel(this.m_ModelDefinition) as GIS3DPlaybackModel;
			model.DataViewModel = this.m_DVM;
			if (!string.IsNullOrEmpty(this.m_LabelName))
			{
				model.DisplayName = row[this.m_LabelName].ToString();
			}
			this.SetPropertyValue(row, model, false);
			//this.m_Layer.AddObject(model);
            try
            {
                Layer.AddObject(model);
            }
            catch (Exception ex)
            {

            }
			this.m_CurrentModelDictionary.AddOrUpdate(dataKey, model, (string key, GIS3DPlaybackModel value) => value);
		}

		/// <summary>
		/// 设置图例图标
		/// </summary>
		/// <param name="row"></param>
		/// <param name="model"></param>
		// Token: 0x0600017E RID: 382 RVA: 0x0000E99C File Offset: 0x0000CB9C
		private void SetLegendIcon(AdapterDataRow row, GIS3DPlaybackModel model)
		{
			string legendName = "";
			string iconName = "";
			if (!string.IsNullOrEmpty(this.m_LegendName))
			{
				if (row[this.m_LegendName] != null)
				{
					legendName = row[this.m_LegendName].ToString().Trim();
					iconName = legendName;
				}
			}
			else
			{
				legendName = this.m_DVM.GroupKeyField.ToString();
				iconName = "Default3DIcon";
			}
			model.LegendName = legendName;
			model.LegendIcon = this.GetIconUri(iconName);
			model.IconName = iconName;
		}

		/// <summary>
		/// 获取图标路径
		/// </summary>
		/// <param name="legendValue"></param>
		/// <returns></returns>
		// Token: 0x0600017F RID: 383 RVA: 0x0000EA30 File Offset: 0x0000CC30
		private string GetIconPath(string legendValue)
		{
			return this.GetIconUri(legendValue).LocalPath;
		}

		/// <summary>
		/// 获取图标路径
		/// </summary>
		/// <param name="legendValue"></param>
		/// <returns></returns>
		// Token: 0x06000180 RID: 384 RVA: 0x0000EA50 File Offset: 0x0000CC50
		private Uri GetIconUri(string legendValue)
		{
			try
			{
				if (!string.IsNullOrEmpty(legendValue) && this.m_DVM.Icon != null && this.m_DVM.Icon.IconList != null)
				{
					IconModel itemModel = this.m_DVM.Icon.IconList.FirstOrDefault((IconModel item) => item.IconName.ToLower() == legendValue.ToLower());
					if (itemModel != null)
					{
						Uri fileUri = this.m_DataProxy.GetResourceFileUri(base.DataViewModel.ID, itemModel.IconPath.Substring(2));
						return new Uri(Path.Combine(fileUri.AbsoluteUri, itemModel.IconPath.Substring(2)), UriKind.Absolute);
					}
				}
			}
			catch (Exception ex)
			{
				ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "GetIconUri", string.Format("获取 {0} 图标路径出现问题", legendValue));
				ChartLogManager.WriteDadChartsError(ex);
				return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DIcon.png"));
			}
			return new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DIcon.png"));
		}

		/// <summary>
		/// 删除
		/// </summary>
		/// <param name="toRemoveModelKeyCollection"></param>
		// Token: 0x06000181 RID: 385 RVA: 0x0000EBB4 File Offset: 0x0000CDB4
		private void RemoveModelByKeys(List<string> toRemoveModelKeyCollection)
		{
			foreach (string key in toRemoveModelKeyCollection)
			{
				if (this.m_CurrentModelDictionary.ContainsKey(key))
				{
					GIS3DPlaybackModel model = this.m_CurrentModelDictionary[key];
					if (model != null)
					{
						if (base.DataViewModel.DataTimeColumn != null)
						{
							if (this.m_DVM.EnableAutoRemoveTrack && (base.Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds > (double)this.m_DVM.AutoRemoveTrackDeferred)
							{
								this.RemoveModel(key, model);
							}
						}
						else
						{
							this.RemoveModel(key, model);
						}
					}
				}
			}
			if (base.IsPlayerJump)
			{
				base.IsPlayerJump = false;
			}
		}

		/// <summary>
		/// 删除散点
		/// </summary>
		/// <param name="dataKey"></param>
		/// <param name="model"></param>
		// Token: 0x06000182 RID: 386 RVA: 0x0000ECC8 File Offset: 0x0000CEC8
		private void RemoveModel(string dataKey, GIS3DPlaybackModel model)
		{
			this.m_CurrentModelDictionary.TryRemove(dataKey, out model);
			if (this.m_EngineContainer.SurfaceView.Globe3DControler.World.CameraMode == GlobeWorld.eCameraMode.Follow && this.m_EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity == model.Entity3D)
			{
				this.m_EngineContainer.GlobeWorld.FollowGlobeCameraController.TargetEntity = null;
				this.m_EngineContainer.GlobeWorld.CameraMode = GlobeWorld.eCameraMode.Free;
			}
			this.m_Layer.RemoveObject(model);
		}

		/// <summary>
		/// 设置轨迹点的属性
		/// </summary>
		/// <param name="row"></param>
		/// <param name="model"></param>
		// Token: 0x06000183 RID: 387 RVA: 0x0000ED64 File Offset: 0x0000CF64
		private void SetPropertyValue(AdapterDataRow row, GIS3DPlaybackModel model, bool isUpdate)
		{
			model.Row = row;
			model.X = Convert.ToDouble(row[this.m_LongitudeName]);
			model.Y = Convert.ToDouble(row[this.m_LatitudeName]);
			if (string.IsNullOrEmpty(this.m_AltName))
			{
				if (this.m_DVM.IsOverlapAltitude)
				{
					model.Z = this.m_DVM.AltitudeOffset;
				}
				else
				{
				    model.Z = 0; //GIS3DConfigurationValue.DefaultHeight;
				}
			}
			else
			{
				model.Z = Convert.ToDouble(row[this.m_AltName]);
			}
			this.SetLegendIcon(row, model);
			this.InitIconStyle(model.IconName);
			if (!this.m_IsInit)
			{
				this.InitTextStyle();
				this.m_IsInit = true;
			}
			this.SetMaterialStyle(model);
			model.TrailColor = GetBrushHelper.GetColorString(this.m_DVM.TrailColor, model.LegendName);
			model.TextKey = base.DataViewModel.Name;
			if (base.Player != null && base.Player.State != Enums.PlayState.Stopped)
			{
				if ((model.OccurDateTime - DateTime.MinValue).TotalSeconds > 0.0)
				{
					model.Duration = (base.Player.CurrentAbsoluteTime - model.OccurDateTime).TotalSeconds;
				}
				else
				{
					model.Duration = base.Player.Interval / 1000.0;
				}
			}
			model.PostProcessThreshold = (float)this.m_DVM.PostProcessThreshold;
			if (!this.m_ClearObject)
			{
				model.UpdateTail();
			}
			else
			{
				model.ClearTail();
			}
			if (this.m_DVM.DataTimeColumn != null)
			{
				ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "m_DVM.DataTimeColumn不为空 ", string.Format("散点时间列的AsName为 {0} ， 值为{1} , 转换成时间为{2}", this.m_DVM.DataTimeColumn.AsName, row[this.m_DVM.DataTimeColumn.AsName], Convert.ToDateTime(row[this.m_DVM.DataTimeColumn.AsName])));
				model.OccurDateTime = Convert.ToDateTime(row[this.m_DVM.DataTimeColumn.AsName]);
			}
			else
			{
				ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "m_DVM.DataTimeColumn为空 ", "m_DVM.DataTimeColumn为空");
				if (base.Player != null)
				{
					ChartLogManager.PrintDebugMesage("GIS3DPlaybackController", "m_DVM.DataTimeColumn为空 ", "model.OccurDateTime使用了时间轴当前时间");
					model.OccurDateTime = base.Player.CurrentAbsoluteTime;
				}
			}
			if (base.Player != null)
			{
				model.SpeedRatio = base.Player.SpeedRatio;
				model.PlayStep = base.GetPlayStep();
			}
			model.IsVisible = this.m_DVM.ShowLayer;
			model.ShowLabel = this.m_DVM.ShowLabel;
			bool isChecked = base.GetLegendItemModelIsChecked(model.LegendName);
			this.SetObjectVisibility(this.m_DVM.ShowLayer, isChecked, new List<GIS3DPlaybackModel>
			{
				model
			});
		}

		/// <summary>
		/// 设置对象的可见性
		/// </summary>
		/// <param name="showLayer">当前图层是否可见</param>
		/// <param name="checkedLegend">图例项是否可见</param>
		/// <param name="models">当前图层的所有对象</param>
		// Token: 0x06000184 RID: 388 RVA: 0x0000F0A8 File Offset: 0x0000D2A8
		private void SetObjectVisibility(bool showLayer, bool checkedLegend, IEnumerable<GIS3DPlaybackModel> models)
		{
			bool isVisible = false;
			if (showLayer && checkedLegend)
			{
				isVisible = true;
			}
			foreach (GIS3DPlaybackModel pointModel in models)
			{
				pointModel.IsVisible = isVisible;
				pointModel.LegendItemChecked = checkedLegend;
			}
		}

		/// <summary>
		/// override 清除图层
		/// </summary>
		/// <param name="isPlayerStoped">true:点击时间轴停止</param>
		// Token: 0x06000185 RID: 389 RVA: 0x0000F11C File Offset: 0x0000D31C
		private void RemoveAll(bool isPlayerStoped)
		{
			if (isPlayerStoped)
			{
				base.OnAVEPlayerStoped();
				if (this.m_DVM.IsGetFirstDataImmediate)
				{
					if (this.m_DVM.EnableTrackingPoint)
					{
						foreach (string key in this.m_CurrentModelDictionary.Keys)
						{
							GIS3DPlaybackModel model = this.m_CurrentModelDictionary[key];
							model.ClearTail();
						}
					}
				}
				else
				{
					foreach (string key in this.m_CurrentModelDictionary.Keys)
					{
						if (this.m_CurrentModelDictionary.ContainsKey(key))
						{
							GIS3DPlaybackModel model = this.m_CurrentModelDictionary[key];
							this.RemoveModel(key, model);
						}
					}
					this.m_CurrentModelDictionary.Clear();
				}
			}
			else
			{
				List<string> toRemoveModelKeyCollection = this.m_CurrentModelDictionary.Keys.ToList<string>();
				this.RemoveModelByKeys(toRemoveModelKeyCollection);
			}
		}

		/// <summary>
		/// 创建图例
		/// </summary>
		// Token: 0x06000186 RID: 390 RVA: 0x0000F26C File Offset: 0x0000D46C
		private void CreateLegend()
		{
			if (base.ChartLegendModel == null)
			{
				ChartLegendModel chartLegendModel = new ChartLegendModel();
				chartLegendModel.LegendTitle = base.DataViewModel.Name;
				IEnumerable<IGrouping<string, GIS3DPlaybackModel>> groupModels = from GIS3DPlaybackModel point in this.m_Layer.ObjectModels
				group point by point.LegendName;
				foreach (IGrouping<string, GIS3DPlaybackModel> pointModels2 in groupModels)
				{
					ChartLegendItemModel legendItemModel = this.CreateLegendItemModel(pointModels2.Key, pointModels2.ToList<GIS3DPlaybackModel>()[0].LegendIcon);
					legendItemModel.PropertyChanged += this.legendItemModel_PropertyChanged;
					chartLegendModel.LegendItemModels.Add(legendItemModel);
				}
				base.ChartLegendModel = chartLegendModel;
			}
			else
			{
				IEnumerable<IGrouping<string, GIS3DPlaybackModel>> groupModels = from GIS3DPlaybackModel layerModel in this.m_Layer.ObjectModels
				group layerModel by layerModel.LegendName;
				using (IEnumerator<IGrouping<string, GIS3DPlaybackModel>> enumerator = groupModels.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IGrouping<string, GIS3DPlaybackModel> pointModels = enumerator.Current;
						if (base.ChartLegendModel.LegendItemModels.FirstOrDefault((ChartLegendItemModel item) => item.LegendValue == pointModels.Key) == null)
						{
							ChartLegendItemModel legendItemModel = this.CreateLegendItemModel(pointModels.Key, pointModels.ToList<GIS3DPlaybackModel>()[0].LegendIcon);
							legendItemModel.PropertyChanged += this.legendItemModel_PropertyChanged;
							base.ChartLegendModel.LegendItemModels.Add(legendItemModel);
						}
					}
				}
			}
		}

		/// <summary>
		/// 创建图例控件项
		/// </summary>
		/// <param name="legendValue"></param>
		/// <param name="iconPath"></param>
		/// <returns></returns>
		// Token: 0x06000187 RID: 391 RVA: 0x0000F488 File Offset: 0x0000D688
		private ChartLegendItemModel CreateLegendItemModel(string legendValue, Uri iconPath)
		{
			return new ChartLegendItemModel
			{
				LegendValue = legendValue,
				LegendIconPath = iconPath,
				UseIcon = true,
				IsMultiple = true
			};
		}

		/// <summary>
		/// 属性改变事件
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		// Token: 0x06000188 RID: 392 RVA: 0x0000F4C4 File Offset: 0x0000D6C4
		private void legendItemModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "IsChecked")
			{
				ChartLegendItemModel legendItemModel = sender as ChartLegendItemModel;
				IEnumerable<GIS3DPlaybackModel> models = from GIS3DPlaybackModel item in this.m_Layer.ObjectModels
				where item.LegendName == legendItemModel.LegendValue
				select item;
				this.SetObjectVisibility(this.m_DVM.ShowLayer, legendItemModel.IsChecked, models);
			}
		}

		/// <summary>
		/// 设置基础值
		/// </summary>
		// Token: 0x06000189 RID: 393 RVA: 0x0000F540 File Offset: 0x0000D740
		private void SetBaseValue()
		{
			if (this.m_DVM == null || this.m_DVM.LonField == null || this.m_DVM.LatField == null || this.m_DVM.GroupKeyField == null)
			{
				if (this.m_Layer != null)
				{
					this.m_Layer.RemoveAll();
				}
			}
			else
			{
				this.m_LongitudeName = this.m_DVM.LonField.AsName;
				this.m_LatitudeName = this.m_DVM.LatField.AsName;
				this.m_GroupKeyName = this.m_DVM.GroupKeyField.AsName;
				if (this.m_DVM.AltField != null)
				{
					this.m_AltName = this.m_DVM.AltField.AsName;
				}
				else
				{
					this.m_AltName = string.Empty;
				}
				if (this.m_DVM.LegendField != null)
				{
					this.m_LegendName = this.m_DVM.LegendField.AsName;
				}
				else
				{
					this.m_LegendName = string.Empty;
				}
				if (this.m_DVM.LabelField != null)
				{
					this.m_LabelName = this.m_DVM.LabelField.AsName;
				}
				else
				{
					this.m_LabelName = string.Empty;
				}
			}
		}

		/// <summary>
		/// 初始化地图控件相关内容 
		/// </summary>
		// Token: 0x0600018A RID: 394 RVA: 0x0000F698 File Offset: 0x0000D898
		private void InitGlobe()
		{
			if (base.EngineContainer != null)
			{
				if (this.m_EngineContainer == null)
				{
					this.m_EngineContainer = (EngineContainer)base.EngineContainer;
				}
				if (this.m_EngineContainer.ModelDefinitionList != null)
				{
					this.m_ModelDefinition = (from item in this.m_EngineContainer.ModelDefinitionList
					where item.DisplayName == "节点轨迹模型"
					select item).FirstOrDefault<GIS3DModelDefinition>();
				}
				if (this.m_Layer == null)
				{
					this.m_Layer = new LayerModel();
					this.m_Layer.Name = base.DataViewModel.Name;
					this.m_EngineContainer.SurfaceModel.Layers.Add(this.m_Layer);
					base.SetGlobeStyleConfig(this.m_DVM);
					base.SetGlobeStyle(this.m_DVM);
				}
			}
		}

		/// <summary>
		/// 初始化Other材质
		/// </summary>
		// Token: 0x0600018B RID: 395 RVA: 0x0000F798 File Offset: 0x0000D998
		private void InitOtherStyle()
		{
			if (this.m_EngineContainer != null && this.m_EngineContainer.SurfaceView != null && this.m_EngineContainer.SurfaceView.Globe3DControler != null)
			{
				if (null == this.m_TrailStyleEntity3D)
				{
					this.m_TrailStyle = new TrailMaterialStyle(this.m_EngineContainer.GlobeWorld.World.ContentManager);
					this.m_TrailStyleEntity3D = this.m_EngineContainer.GlobeWorld.World.AddEntity("m_TrailStyleEntity3D" + base.DataViewModel.Name);
					this.m_TrailStyleEntity3D.Visible = false;
					TrailComponent trailComponent = new TrailComponent();
					trailComponent.Style = this.m_TrailStyle;
					this.m_TrailStyleEntity3D.AddComponent(trailComponent);
				}
				if (null == this.m_TextBackgroundStyleEntity3D)
				{
					this.m_TextBackgroundStyle = new DefaultBillboardMaterialStyle(this.m_EngineContainer.GlobeWorld.World.ContentManager);
					this.m_TextBackgroundStyle.InitializeWithMaterialFile(this.m_EngineContainer.GlobeWorld.World.ContentManager, ".\\Resources\\System\\9GridsBillboardMaterial.xml");
					this.m_TextBackgroundStyle.ClipRange = (float)this.m_DVM.MaxVisibleDistance;
					this.m_TextBackgroundStyle.NearFactor = new Vector2((float)this.m_DVM.NearFactor);
					this.m_TextBackgroundStyle.FarFactor = new Vector2((float)this.m_DVM.FarFactor);
					this.m_TextBackgroundStyle.IsPerspective = false;
					this.m_TextBackgroundStyle.TexturePath = ".\\Resources\\System\\WhiteTextBG.dds";
					this.m_TextBackgroundStyleEntity3D = this.m_EngineContainer.GlobeWorld.World.AddEntity("m_TextBackgroundStyleEntity3D" + base.DataViewModel.Name);
					this.m_TextBackgroundStyleEntity3D.Visible = false;
					BillboardComponent billboardComponent = new BillboardComponent();
					billboardComponent.MaterialStyle = this.m_TextBackgroundStyle;
					this.m_TextBackgroundStyleEntity3D.AddComponent(billboardComponent);
				}
				if (null == this.m_PointStyleEntity3D)
				{
					this.m_PointStyle = new DefaultBillboardMaterialStyle(this.m_EngineContainer.GlobeWorld.World.ContentManager);
					this.m_PointStyle.ClipRange = 5000f;
					this.m_PointStyle.NearFactor = new Vector2(1f, 1f);
					this.m_PointStyle.FarFactor = new Vector2(0.2f, 0.2f);
					this.m_PointStyle.IsPerspective = false;
					this.m_PointStyle.Texture = this.m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(".\\Resources\\Textures\\Point.png");
					this.m_PointStyleEntity3D = this.m_EngineContainer.GlobeWorld.World.AddEntity("m_PointStyleEntity3D" + base.DataViewModel.Name);
					this.m_PointStyleEntity3D.Visible = false;
					BillboardComponent billboardComponent = new BillboardComponent();
					billboardComponent.MaterialStyle = this.m_PointStyle;
					this.m_PointStyleEntity3D.AddComponent(billboardComponent);
				}
			}
		}

		/// <summary>
		/// 初始化Icon材质
		/// </summary>
		// Token: 0x0600018C RID: 396 RVA: 0x0000FAAC File Offset: 0x0000DCAC
		private void InitIconStyle(string iconName)
		{
			if (!this.m_IconNameToIconStyle.ContainsKey(iconName))
			{
				string iconPath = this.GetIconPath(iconName);
				string meterialPath = string.Empty;
				if (this.m_DVM.IsOverlapShine)
				{
					meterialPath = ".\\Resources\\System\\TrailHeadMaterial.xml";
				}
				else
				{
					meterialPath = ".\\Resources\\System\\DefaultBillboardMaterial.xml";
				}
				DefaultStyleClass defaultStyleClass = this.InitIconDefaultStyleClass(iconPath, meterialPath);
				this.m_IconNameToIconStyle.Add(iconName, defaultStyleClass);
				this.CreateIconEntity(this.m_IconStyleEntity3D, "m_IconStyleEntity3D_" + iconName + base.DataViewModel.Name, defaultStyleClass);
			}
			if (!this.m_IconNameToSelectedIconStyle.ContainsKey(iconName))
			{
				string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DBg.png");
				string meterialPath = ".\\Resources\\System\\TrailHeadMaterialWhite.xml";
				DefaultStyleClass defaultStyleClass = this.InitIconDefaultStyleClass(iconPath, meterialPath);
				this.m_IconNameToSelectedIconStyle.Add(iconName, defaultStyleClass);
				this.CreateIconEntity(this.m_SelectedIconStyleEntity3D, "m_SelectedIconStyleEntity3D_" + iconName + base.DataViewModel.Name, defaultStyleClass);
			}
			if (!this.m_IconNameToShinningIconStyle.ContainsKey(iconName))
			{
				string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DShinningBg.png");
				string meterialPath = ".\\Resources\\System\\TrailHeadMaterialWhite.xml";
				DefaultStyleClass defaultStyleClass = this.InitIconDefaultStyleClass(iconPath, meterialPath);
				this.m_IconNameToShinningIconStyle.Add(iconName, defaultStyleClass);
				this.CreateIconEntity(this.m_ShinningIconStyleEntity3D, "m_ShinningIconStyleEntity3D_" + iconName + base.DataViewModel.Name, defaultStyleClass);
			}
		}

		/// <summary>
		/// 初始化IconStyle
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="meterialPath"></param>
		/// <returns></returns>
		// Token: 0x0600018D RID: 397 RVA: 0x0000FC14 File Offset: 0x0000DE14
		private DefaultStyleClass InitIconDefaultStyleClass(string iconPath, string meterialPath)
		{
			DefaultStyleClass defaultStyleClass = new DefaultStyleClass();
			defaultStyleClass.BillboardStyle = new DefaultBillboardMaterialStyle();
			defaultStyleClass.BillboardStyle.InitializeWithMaterialFile(this.m_EngineContainer.GlobeWorld.World.ContentManager, meterialPath);
			this.UpdateDefaultStyleClass(iconPath, defaultStyleClass);
			return defaultStyleClass;
		}

		/// <summary>
		/// 更新IconStyle
		/// </summary>
		/// <param name="iconPath"></param>
		/// <param name="defaultStyleClass"></param>
		// Token: 0x0600018E RID: 398 RVA: 0x0000FC64 File Offset: 0x0000DE64
		private void UpdateDefaultStyleClass(string iconPath, DefaultStyleClass defaultStyleClass)
		{
			defaultStyleClass.BillboardStyle.Texture = this.m_EngineContainer.GlobeWorld.World.ContentManager.LoadTexture(iconPath);
			defaultStyleClass.BillboardStyle.ClipRange = (float)this.m_DVM.MaxVisibleDistance;
			defaultStyleClass.BillboardStyle.NearFactor = new Vector2((float)this.m_DVM.NearFactor);
			defaultStyleClass.BillboardStyle.FarFactor = new Vector2((float)this.m_DVM.FarFactor);
			defaultStyleClass.BillboardStyle.IsPerspective = false;
			this.InitIconWidthHeight(iconPath, defaultStyleClass);
		}

		/// <summary>
		/// 初始化IconEntity
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="entityName"></param>
		/// <param name="defaultStyleClass"></param>
		// Token: 0x0600018F RID: 399 RVA: 0x0000FD04 File Offset: 0x0000DF04
		private void CreateIconEntity(Entity3D entity, string entityName, DefaultStyleClass defaultStyleClass)
		{
			entity = this.m_EngineContainer.GlobeWorld.World.AddEntity(entityName);
			entity.Visible = false;
			entity.AddComponent(new BillboardComponent
			{
				MaterialStyle = defaultStyleClass.BillboardStyle
			});
		}

		/// <summary>
		/// 更新Icon材质
		/// </summary>
		// Token: 0x06000190 RID: 400 RVA: 0x0000FD50 File Offset: 0x0000DF50
		private void UpdateIconStyle()
		{
			foreach (KeyValuePair<string, DefaultStyleClass> item in this.m_IconNameToIconStyle)
			{
				string iconPath = this.GetIconPath(item.Key);
				this.UpdateDefaultStyleClass(iconPath, item.Value);
			}
			foreach (KeyValuePair<string, DefaultStyleClass> item in this.m_IconNameToSelectedIconStyle)
			{
				string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DBg.png");
				this.UpdateDefaultStyleClass(iconPath, item.Value);
			}
			foreach (KeyValuePair<string, DefaultStyleClass> item in this.m_IconNameToShinningIconStyle)
			{
				string iconPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Icons", "Default3DShinningBg.png");
				this.UpdateDefaultStyleClass(iconPath, item.Value);
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0000FE98 File Offset: 0x0000E098
		private void InitTextStyle()
		{
			if (this.m_EngineContainer != null)
			{
				this.m_EngineContainer.GlobeWorld.RegisterTextSystem(base.DataViewModel.Name, ".\\\\Resources\\\\Fonts\\\\MSYaHei_GBK.fnt", GlobeWorld.SceneGroupNearMiddle, new Vector2((float)this.m_DVM.NearFactor), new Vector2((float)this.m_DVM.FarFactor), (float)this.m_DVM.MaxVisibleDistance, false, true, -200f);
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000FF10 File Offset: 0x0000E110
		private void UpdateTextStyle()
		{
			if (this.m_EngineContainer != null)
			{
				this.m_EngineContainer.GlobeWorld.ModifyTextSystem(base.DataViewModel.Name, new Vector2((float)this.m_DVM.NearFactor), new Vector2((float)this.m_DVM.FarFactor), (float)this.m_DVM.MaxVisibleDistance);
			}
		}

		/// <summary>
		/// 初始化图标大小
		/// </summary>
		// Token: 0x06000193 RID: 403 RVA: 0x0000FF78 File Offset: 0x0000E178
		private void InitIconWidthHeight(string iconPath, DefaultStyleClass defaultStyleClass)
		{
			if (iconPath.StartsWith("."))
			{
				iconPath = AppDomain.CurrentDomain.BaseDirectory + iconPath.Substring(2, iconPath.Length - 2);
			}
			if (!File.Exists(iconPath))
			{
				//iconPath = AppDomain.CurrentDomain.BaseDirectory + GIS3DConfigurationValue.PointPath.Substring(2, GIS3DConfigurationValue.PointPath.Length - 2);
			}
			Image image = Image.FromFile(iconPath);
			defaultStyleClass.ImageWidth = (double)image.Width;
			defaultStyleClass.ImageHeight = (double)image.Height;
		}

		/// <summary>
		/// 设置材质
		/// </summary>
		// Token: 0x06000194 RID: 404 RVA: 0x00010010 File Offset: 0x0000E210
		private void SetMaterialStyle(GIS3DPlaybackModel model)
		{
			model.TrailStyle = this.m_TrailStyle;
			model.TextBackgroundStyle = this.m_TextBackgroundStyle;
			model.PointStyle = this.m_PointStyle;
			model.IconStyle = this.m_IconNameToIconStyle[model.IconName].BillboardStyle;
			model.ImageWidth = this.m_IconNameToIconStyle[model.IconName].ImageWidth;
			model.ImageHeight = this.m_IconNameToIconStyle[model.IconName].ImageHeight;
			model.SelectedIconStyle = this.m_IconNameToSelectedIconStyle[model.IconName].BillboardStyle;
			model.SelectedImageWidth = this.m_IconNameToSelectedIconStyle[model.IconName].ImageWidth;
			model.SelectedImageHeight = this.m_IconNameToSelectedIconStyle[model.IconName].ImageHeight;
			model.ShinningIconStyle = this.m_IconNameToShinningIconStyle[model.IconName].BillboardStyle;
			model.ShinningImageWidth = this.m_IconNameToShinningIconStyle[model.IconName].ImageWidth;
			model.ShinningImageHeight = this.m_IconNameToShinningIconStyle[model.IconName].ImageHeight;
			model.UpdateBillboard();
		}

		/// <summary>
		/// 地球范围改变事件
		/// </summary>
		/// <param name="minLon"></param>
		/// <param name="maxLon"></param>
		/// <param name="minLat"></param>
		/// <param name="maxLat"></param>
		// Token: 0x06000195 RID: 405 RVA: 0x00010154 File Offset: 0x0000E354
		public override void MapExtentChanged(double minLon, double maxLon, double minLat, double maxLat)
		{
			if (this.m_DVM != null && this.m_DVM.EnableSpatialQuery)
			{
				base.SetAdditionCondition(minLon, maxLon, minLat, maxLat);
				base.ReloadByAdditionCondition();
			}
		}

		/// <summary>
		/// 创建附加筛选条件
		/// </summary>
		/// <param name="minLon"></param>
		/// <param name="maxLon"></param>
		/// <param name="minLat"></param>
		/// <param name="maxLat"></param>
		/// <returns></returns>
		// Token: 0x06000196 RID: 406 RVA: 0x00010198 File Offset: 0x0000E398
		public override AdapterConditionModel CreateAdditionConditions(double minLon, double maxLon, double minLat, double maxLat)
		{
			AdapterConditionModel conditions = new AdapterConditionModel();
			AdapterConditionModel lonGreater = AdapterConditionModelHelper.GetCondition(this.m_DVM.LonField, minLon, ConditionJudgmentTypes.GreaterThaOrEqual);
			AdapterConditionModel lonLess = AdapterConditionModelHelper.GetCondition(this.m_DVM.LonField, maxLon, ConditionJudgmentTypes.LessThaOrEqual);
			AdapterConditionModel latGreater = AdapterConditionModelHelper.GetCondition(this.m_DVM.LatField, minLat, ConditionJudgmentTypes.GreaterThaOrEqual);
			AdapterConditionModel latLess = AdapterConditionModelHelper.GetCondition(this.m_DVM.LatField, maxLat, ConditionJudgmentTypes.LessThaOrEqual);
			conditions.CompoundConditions.Add(lonGreater);
			conditions.CompoundConditions.Add(lonLess);
			conditions.CompoundConditions.Add(latGreater);
			conditions.CompoundConditions.Add(latLess);
			return conditions;
		}

		/// <summary>
		/// 设置后期特效曝光亮度门限 
		/// </summary>
		/// <param name="value"></param>
		// Token: 0x06000197 RID: 407 RVA: 0x0001024A File Offset: 0x0000E44A
		private void SetProcessThreshold(double value)
		{
			this.m_EngineContainer.PostProcessThreshold = value;
		}

		/// <summary>
		/// 图层显隐控制
		/// </summary>
		/// <param name="showLayer"></param>
		// Token: 0x06000198 RID: 408 RVA: 0x0001025C File Offset: 0x0000E45C
		public override void SetShowLayer(bool showLayer)
		{
			if (this.m_Layer != null)
			{
				this.m_Layer.IsVisible = showLayer;
				foreach (OsgObjectModel osgObjectModel in this.m_Layer.ObjectModels)
				{
					GIS3DPlaybackModel pointModel = (GIS3DPlaybackModel)osgObjectModel;
					pointModel.IsVisible = showLayer;
				}
			}
		}

		/// <summary>
		/// 接收消息
		/// </summary>
		// Token: 0x06000199 RID: 409 RVA: 0x000102E0 File Offset: 0x0000E4E0
		private void ReceiveMessages()
		{
			this.m_MessageAggregator.GetMessage<ClearObjectMessage>().Subscribe(new Action<ClearObjectModel>(this.ReceivedClearObjectMessage));
		}

		/// <summary>
		/// 接收清空对象消息
		/// </summary>
		/// <param name="clearModel"></param>
		// Token: 0x0600019A RID: 410 RVA: 0x00010300 File Offset: 0x0000E500
		private void ReceivedClearObjectMessage(ClearObjectModel clearModel)
		{
			if (this.m_DVM != null && !string.IsNullOrEmpty(this.m_DVM.Name))
			{
				if (clearModel.DVMIdList.Contains(this.m_DVM.ID.ToString()))
				{
					this.m_ClearObject = true;
					if (this.m_DVM.EnableTrackingPoint)
					{
						if (clearModel.ClearTrail)
						{
							foreach (string key in this.m_CurrentModelDictionary.Keys)
							{
								GIS3DPlaybackModel model = this.m_CurrentModelDictionary[key];
								model.ClearTail();
							}
						}
						if (clearModel.ClearModel)
						{
							List<string> toRemoveModelKeyCollection = this.m_CurrentModelDictionary.Keys.ToList<string>();
							this.RemoveModelByKeys(toRemoveModelKeyCollection);
						}
					}
				}
			}
		}

		/// <summary>
		/// 销毁
		/// </summary>
		// Token: 0x0600019B RID: 411 RVA: 0x0001041C File Offset: 0x0000E61C
		public override void Dispose()
		{
			if (this.m_EngineContainer != null && this.m_EngineContainer.SurfaceView != null && this.m_EngineContainer.SurfaceView.Globe3DControler != null)
			{
				if (this.m_Layer != null)
				{
					this.m_EngineContainer.SurfaceModel.Layers.Remove(this.m_Layer);
				}
				if (this.m_PointStyleEntity3D != null)
				{
					this.m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(this.m_PointStyleEntity3D);
					this.m_PointStyleEntity3D = null;
				}
				if (this.m_TrailStyleEntity3D != null)
				{
					this.m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(this.m_TrailStyleEntity3D);
					this.m_TrailStyleEntity3D = null;
				}
				if (this.m_TextBackgroundStyleEntity3D != null)
				{
					this.m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(this.m_TextBackgroundStyleEntity3D);
					this.m_TextBackgroundStyleEntity3D = null;
				}
				if (this.m_IconStyleEntity3D != null)
				{
					this.m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(this.m_IconStyleEntity3D);
					this.m_IconStyleEntity3D = null;
				}
				if (this.m_SelectedIconStyleEntity3D != null)
				{
					this.m_EngineContainer.SurfaceView.Globe3DControler.RemoveEntity(this.m_SelectedIconStyleEntity3D);
					this.m_SelectedIconStyleEntity3D = null;
				}
			}
			if (this.m_CurrentModelDictionary != null)
			{
				this.m_CurrentModelDictionary.Clear();
			}
			base.Dispose();
		}

		/// <summary>
		/// 引擎容器
		/// </summary>
		// Token: 0x040000C3 RID: 195
		protected EngineContainer m_EngineContainer;

		/// <summary>
		/// 模型定义
		/// </summary>
		// Token: 0x040000C4 RID: 196
		private GIS3DModelDefinition m_ModelDefinition;

		/// <summary>
		/// 经度列名称
		/// </summary>
		// Token: 0x040000C5 RID: 197
		private string m_LongitudeName;

		/// <summary>
		/// 纬度列名称
		/// </summary>
		// Token: 0x040000C6 RID: 198
		private string m_LatitudeName;

		/// <summary>
		/// 高度列名称
		/// </summary>
		// Token: 0x040000C7 RID: 199
		private string m_AltName = string.Empty;

		/// <summary>
		/// 批号列名称
		/// </summary>
		// Token: 0x040000C8 RID: 200
		private string m_GroupKeyName;

		/// <summary>
		/// 显示名称
		/// </summary>
		// Token: 0x040000C9 RID: 201
		private string m_LabelName;

		/// <summary>
		/// 图例列名称
		/// </summary>
		// Token: 0x040000CA RID: 202
		private string m_LegendName;

		// Token: 0x040000CB RID: 203
		private double m_PresentPlayStep = 0.0;

		// Token: 0x040000CC RID: 204
		private bool m_IsInit = false;

		// Token: 0x040000CD RID: 205
		private GIS3DTrackDataViewModel m_DVM;

		// Token: 0x040000CE RID: 206
		private LayerModel m_Layer;

		// Token: 0x040000CF RID: 207
		private ConcurrentDictionary<string, GIS3DPlaybackModel> m_CurrentModelDictionary = new ConcurrentDictionary<string, GIS3DPlaybackModel>();

		/// <summary>
		/// 图标材质
		/// </summary>
		// Token: 0x040000D0 RID: 208
		private Dictionary<string, DefaultStyleClass> m_IconNameToIconStyle = new Dictionary<string, DefaultStyleClass>();

		/// <summary>
		/// 图标材质（选中）
		/// </summary>
		// Token: 0x040000D1 RID: 209
		private Dictionary<string, DefaultStyleClass> m_IconNameToSelectedIconStyle = new Dictionary<string, DefaultStyleClass>();

		/// <summary>
		/// 图标材质（shinning）
		/// </summary>
		// Token: 0x040000D2 RID: 210
		private Dictionary<string, DefaultStyleClass> m_IconNameToShinningIconStyle = new Dictionary<string, DefaultStyleClass>();

		/// <summary>
		/// 点材质
		/// </summary>
		// Token: 0x040000D3 RID: 211
		private DefaultBillboardMaterialStyle m_PointStyle = null;

		/// <summary>
		/// 点材质存储实体
		/// </summary>
		// Token: 0x040000D4 RID: 212
		private Entity3D m_PointStyleEntity3D = null;

		/// <summary>
		/// 尾迹材质
		/// </summary>
		// Token: 0x040000D5 RID: 213
		private TrailMaterialStyle m_TrailStyle = null;

		/// <summary>
		/// 尾迹材质存储实体
		/// </summary>
		// Token: 0x040000D6 RID: 214
		private Entity3D m_TrailStyleEntity3D = null;

		/// <summary>
		/// 文字背景材质
		/// </summary>
		// Token: 0x040000D7 RID: 215
		private DefaultBillboardMaterialStyle m_TextBackgroundStyle = null;

		/// <summary>
		/// 文字背景材质存储实体
		/// </summary>
		// Token: 0x040000D8 RID: 216
		private Entity3D m_TextBackgroundStyleEntity3D = null;

		/// <summary>
		/// 材质的实体
		/// </summary>
		// Token: 0x040000D9 RID: 217
		private Entity3D m_IconStyleEntity3D = null;

		/// <summary>
		/// 材质的实体（选中）
		/// </summary>
		// Token: 0x040000DA RID: 218
		private Entity3D m_SelectedIconStyleEntity3D = null;

		/// <summary>
		/// 材质的实体（shinning)
		/// </summary>
		// Token: 0x040000DB RID: 219
		private Entity3D m_ShinningIconStyleEntity3D = null;

		// Token: 0x040000DC RID: 220
		private DateTime m_PresentDateTime;

		/// <summary>
		/// 进程间通信的消息聚合器对象
		/// </summary>
		// Token: 0x040000DD RID: 221
		private IMessageAggregator m_MessageAggregator = new MessageAggregator();

		/// <summary>
		/// 是否进行了清空对象操作
		/// </summary>
		// Token: 0x040000DE RID: 222
		private bool m_ClearObject = false;
	}


    /// <summary>
    /// 默认材质结构（包含贴图大小）
    /// </summary>
    public class DefaultStyleClass
    {
        /// <summary>
        /// 图片宽
        /// </summary>
        public double ImageWidth;
        /// <summary>
        /// 图片高
        /// </summary>
        public double ImageHeight;
        /// <summary>
        /// 标牌样式
        /// </summary>
        public DefaultBillboardMaterialStyle BillboardStyle;
        /// <summary>
        /// 应用标牌的实例
        /// </summary>
        public Entity3D Entity3D { get; set; }
    }

}
