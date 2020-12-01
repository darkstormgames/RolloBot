using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace RolloBot.ViewModel
{
    public abstract class ModuleViewModelBase : INotifyPropertyChanged
    {
        public string URL { get; set; }
        public string Title { get; set; }
        public virtual string Tooltip
        {
            get
            {
                return URL;
            }
        }

        public virtual bool CanClose
        {
            get
            {
                return true;
            }
        }

        public virtual bool HasChanged
        {
            get
            {
                return false;
            }
        }

        public virtual void Save()
        {
            // Do nowt!
        }

        public virtual void Close()
        {
            // Do nowt!
        }


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged
    }
}
