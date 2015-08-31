using Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Plugin
{
    public class PluginBase
    {
        public PluginBase()
        {
            Background = new SolidColorBrush(Color.FromRgb(194, 194, 194));
        }
        public string Name { get; set; }

        public IExecutor Executor { get; protected set; }

        protected bool _isUsed;
        public bool IsUsed
        {
            get
            {
                return _isUsed;
            }
            set
            {
                if(IExecutorUsedChangedEvent!=null&&Executor!=null)
                {
                    IExecutorUsedChangedEvent(Executor,value);
                }
                _isUsed = value;
                if (value)
                {
                    ((SolidColorBrush)Background).Color = Color.FromRgb(72, 72, 72);
                }
                else
                {
                    ((SolidColorBrush)Background).Color = Color.FromRgb(194, 194, 194);
                }
            }
        }
        public string Description { set; get; }

        public Brush Background { get;protected set; }

        public virtual UserControl GetControl()
        {
            return null;
        }

        public delegate void IExecutorUsedChanged(IExecutor ie, bool value);

        public event IExecutorUsedChanged IExecutorUsedChangedEvent;
    }
}
