using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CM.PopupPanels.OrderInfomation;
using Digihail.DAD3.Models.DataAdapter;

namespace CM.PopupPanels
{
    public partial class Window1 : Window
    {
        public Window1(AdapterDataRow row, EarthDataViewModel m_Dvm)
        {
            InitializeComponent();

            Image2.Source =
                new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\CM\\Background.png", UriKind.Absolute));

            var planting = row[m_Dvm.PlantingField.AsName].ToString();
            var purpose = row[m_Dvm.PurposeField.AsName].ToString();
            var area = row[m_Dvm.AreaField.AsName].ToString();
            var owner = row[m_Dvm.OwnerField.AsName].ToString();
            var productName = row[m_Dvm.ProductNameField.AsName].ToString();
            var count = row[m_Dvm.CountField.AsName].ToString();
            var unitPrice = row[m_Dvm.UnitPriceField.AsName].ToString();
            var totalPrice = row[m_Dvm.TotalPriceField.AsName].ToString();
            var subsidyAmount = row[m_Dvm.SubsidyAmountField.AsName].ToString();
            var paidAmount = row[m_Dvm.PaidAmountField.AsName].ToString();

            var pictureCatalog = row[m_Dvm.PictureCatalog.AsName].ToString();

            Text1.Text = planting;
            Text2.Text = purpose;
            Text3.Text = area + " 亩";
            Text4.Text = owner;
            Text5.Text = productName.Substring(0, 5);
            Text6.Text = count;
            Text7.Text = unitPrice + " 元";
            Text8.Text = totalPrice + " 元";
            Text9.Text = subsidyAmount + " 元";
            Text10.Text = paidAmount + " 元";

            var path = AppDomain.CurrentDomain.BaseDirectory + pictureCatalog;

            if (File.Exists(path))
            {
                Image1.Source = new BitmapImage(new Uri(path, UriKind.Absolute));
            }
        }

        /// <summary>
        ///     控件截图
        /// </summary>
        public string CreatePic()
        {
            FrameworkElement ui = PictureControl;
            FileStream ms = null;

            var fiels = Directory.GetFiles(@"./CM");

            foreach (var fiel in fiels)
            {
                if (fiel.Contains("基地信息"))
                {
                    File.Delete(fiel);
                }
            }

            var filePath = "./CM/基地信息" + Guid.NewGuid() + ".png";

            try
            {
                ms = new FileStream(filePath, FileMode.Create);
                var bmp = new RenderTargetBitmap((int) ui.ActualWidth,
                    (int) ui.ActualHeight, 96d, 96d, PixelFormats.Pbgra32);
                bmp.Render(ui);
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bmp));
                encoder.Save(ms);
                ms.Close();
            }
            catch (Exception ex)
            {
                if (ms != null)
                {
                    ms.Close();
                }
            }

            return filePath;
        }
    }
}