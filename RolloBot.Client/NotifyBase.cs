using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace RolloBot.Client
{
    public abstract class NotifyBase : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        protected Dictionary<string, List<string>> ErrorList = new Dictionary<string, List<string>>();

        public bool HasErrors => ErrorList.Values.Count > 0;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentException(propertyName + " Bindung existiert nicht in " + GetType().Name + " ");
            }
            try
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            catch (TargetInvocationException)
            {
            }
            catch (Exception)
            {
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            CheckErrorCollectionForProperty(propertyName);
            return ErrorList[propertyName];
        }

        protected void CheckErrorCollectionForProperty(string propertyName)
        {
            if (!ErrorList.ContainsKey(propertyName))
            {
                ErrorList[propertyName] = new List<string>();
            }
        }

        public void AddError(string propertyName, string error)
        {
            CheckErrorCollectionForProperty(propertyName);
            ErrorList[propertyName].Add(error);
            RaiseErrorsChanged(propertyName);
        }

        public void RemoveError(string propertyName, string error)
        {
            if (ErrorList.Count > 0 && ErrorList[propertyName].Contains(error))
            {
                ErrorList[propertyName].Remove(error);
                RaiseErrorsChanged(propertyName);
            }
        }

        public bool ExistsError(string propertyName, string error)
        {
            if (ErrorList.Count > 0 && ErrorList.ContainsKey(propertyName))
            {
                return ErrorList[propertyName].Contains(error);
            }
            return false;
        }

        protected void RaiseErrorsChanged(string propertyName)
        {
            this.ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        protected void ClearErrors()
        {
            ErrorList.Clear();
        }
    }
}
