using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

using RolloBot.Client.Configuration;

namespace RolloBot.Tools
{
    public class ToolConfigBase
    {
        protected IniFile iniFile;
        protected ToolWindowBase mainWindow;

        public ObservableCollection<ToolConfigItem> ConfigItems { get; protected set; }

        public ToolConfigBase(ToolWindowBase mainWindow)
        {
            this.mainWindow = mainWindow;
        }
        
        protected void loadOrDefault()
        {
            if (iniFile == null) return;

            foreach (ToolConfigItem item in ConfigItems)
            {
                if (!iniFile.KeyExists(item.Name))
                    item.Value = item.DefaultValue;
                else
                    LoadItem(item);

                SaveItem(item);
            }
        }

        public void SaveItem(ToolConfigItem item)
        {
            switch (item.Type.FullName)
            {
                #region Binary-Types (boolean, (s)byte)
                case "System.Boolean":
                    iniFile.Write(item.Name, ((bool)item.Value).ToString());
                    break;
                case "System.Byte":
                    iniFile.Write(item.Name, ((byte)item.Value).ToString());
                    break;
                case "System.SByte":
                    iniFile.Write(item.Name, Convert.ToSByte(item.Value).ToString());
                    break;
                #endregion

                #region Integer-Types ((u)short, (u)int, (u)long)
                case "System.Int16":
                    iniFile.Write(item.Name, ((short)item.Value).ToString());
                    break;
                case "System.UInt16":
                    iniFile.Write(item.Name, Convert.ToUInt16(item.Value).ToString());
                    break;
                case "System.Int32":
                    iniFile.Write(item.Name, ((int)item.Value).ToString());
                    break;
                case "System.UInt32":
                    iniFile.Write(item.Name, Convert.ToUInt32(item.Value).ToString());
                    break;
                case "System.Int64":
                    iniFile.Write(item.Name, ((long)item.Value).ToString());
                    break;
                case "System.UInt64":
                    iniFile.Write(item.Name, Convert.ToUInt64(item.Value).ToString());
                    break;
                #endregion

                case "System.String":
                    iniFile.Write(item.Name, item.Value.ToString());
                    break;
                
            }
        }
        public void LoadItem(ToolConfigItem item)
        {
            switch (item.Type.FullName)
            {
                #region Binary-Types (boolean, (s)byte)
                case "System.Boolean":
                    if (bool.TryParse(iniFile.Read(item.Name), out bool resultbool))
                        item.Value = resultbool;
                    else
                        item.Value = item.DefaultValue;
                    break;
                case "System.Byte":
                    if (byte.TryParse(iniFile.Read(item.Name), out byte resultbyte))
                        item.Value = resultbyte;
                    else
                        item.Value = item.DefaultValue;
                    break;
                case "System.SByte":
                    if (sbyte.TryParse(iniFile.Read(item.Name), out sbyte resultsbyte))
                        item.Value = resultsbyte;
                    else
                        item.Value = item.DefaultValue;
                    break;
                #endregion

                #region Integer-Types ((u)short, (u)int, (u)long)
                case "System.Int16":
                    if (short.TryParse(iniFile.Read(item.Name), out short resultshort))
                        item.Value = resultshort;
                    else
                        item.Value = item.DefaultValue;
                    break;
                case "System.UInt16":
                    if (ushort.TryParse(iniFile.Read(item.Name), out ushort resultushort))
                        item.Value = resultushort;
                    else
                        item.Value = item.DefaultValue;
                    break;
                case "System.Int32":
                    if (int.TryParse(iniFile.Read(item.Name), out int resultint))
                        item.Value = resultint;
                    else
                        item.Value = item.DefaultValue;
                    break;
                case "System.UInt32":
                    if (uint.TryParse(iniFile.Read(item.Name), out uint resultuint))
                        item.Value = resultuint;
                    else
                        item.Value = item.DefaultValue;
                    break;
                case "System.Int64":
                    if (long.TryParse(iniFile.Read(item.Name), out long resultlong))
                        item.Value = resultlong;
                    else
                        item.Value = item.DefaultValue;
                    break;
                case "System.UInt64":
                    if (ulong.TryParse(iniFile.Read(item.Name), out ulong resultulong))
                        item.Value = resultulong;
                    else
                        item.Value = item.DefaultValue;
                    break;
                #endregion

                case "System.String":
                    item.Value = iniFile.Read(item.Name);
                    break;
                
            }
        }
        private UIElement getConfigElement(ToolConfigItem item)
        {
            UIElement element = null;

            switch(item.Type.FullName)
            {
                #region Binary-Types (boolean, (s)byte)
                case "System.Boolean":
                    element = new CheckBox()
                    {
                        Name = item.Name,
                        IsChecked = (bool)item.Value,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        ToolTip = item.ToolTip
                    };
                    (element as CheckBox).Checked += item.Checked;
                    (element as CheckBox).Unchecked += item.Unchecked;
                    break;
                case "System.Byte":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = ((byte)item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "000",
                        CaretIndex = 3,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.Int16Changed;
                    break;
                case "System.SByte":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = Convert.ToSByte(item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "000",
                        CaretIndex = 3,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.Int16Changed;
                    break;
                #endregion

                #region Integer-Types ((u)short, (u)int, (u)long)
                case "System.Int16":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = ((short)item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "00000",
                        CaretIndex = 5,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.Int16Changed;
                    break;
                case "System.UInt16":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = Convert.ToUInt16(item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "00000",
                        CaretIndex = 5,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.UInt16Changed;
                    break;
                case "System.Int32":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = ((int)item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "0000000000",
                        CaretIndex = 10,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.Int32Changed;
                    break;
                case "System.UInt32":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = Convert.ToUInt32(item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "0000000000",
                        CaretIndex = 10,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.UInt32Changed;
                    break;
                case "System.Int64":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = ((long)item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "00000000000000000000",
                        CaretIndex = 20,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.Int64Changed;
                    break;
                case "System.UInt64":
                    element = new MaskedTextBox()
                    {
                        Name = item.Name,
                        Text = Convert.ToUInt64(item.Value).ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        ToolTip = item.ToolTip,
                        Mask = "00000000000000000000",
                        CaretIndex = 20,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.UInt64Changed;
                    break;
                #endregion

                //case "System.Decimal":

                //    break;
                //case "System.Double":

                //    break;
                case "System.String":
                    element = new TextBox()
                    {
                        Name = item.Name,
                        Text = (string)item.Value,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        MinWidth = 80,
                        TextWrapping = TextWrapping.Wrap,
                        ToolTip = item.ToolTip,
                        HorizontalContentAlignment = HorizontalAlignment.Center
                    };
                    (element as TextBox).TextChanged += item.TextChanged;
                    break;
                //case "System.Char":
                    
                //    break;
                //case "System.DateTime":
                    
                //    break;
                
                default:
                    element = new Label()
                    {
                        Content = "Couldn´t read config-item...",
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    break;
            }

            return element;
        }

        public void GetOptionsView()
        {
            mainWindow.OptionsPanel.Children.Clear();
            foreach (ToolConfigItem item in ConfigItems)
            {
                mainWindow.OptionsPanel.Children.Add(getLabel(item));
                mainWindow.OptionsPanel.Children.Add(getConfigElement(item));
            }
        }
        private Label getLabel(ToolConfigItem item)
        {
            return new Label()
            {
                Content = item.DisplayName,
                HorizontalAlignment = HorizontalAlignment.Center,
                ToolTip = item.ToolTip
            };
        }

        public ToolConfigItem this[string name]
        {
            get
            {
                return ConfigItems.FirstOrDefault(p => p.Name.Equals(name));
            }
        }
    }
}
