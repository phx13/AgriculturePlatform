using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.ViewModel;

namespace CM.SwitchLayerButton
{
    public class DataModel : NotificationObject
    {
        private string m_ButtonName;

        private Dictionary<Guid, string> m_LayerInfo = new Dictionary<Guid, string>();

        public string ButtonName
        {
            get { return m_ButtonName; }
            set
            {
                m_ButtonName = value;
                RaisePropertyChanged(() => ButtonName);
            }
        }

        public Dictionary<Guid, string> LayerInfo
        {
            get { return m_LayerInfo; }
            set
            {
                m_LayerInfo = value;
                RaisePropertyChanged(() => LayerInfo);
            }
        }
    }
}