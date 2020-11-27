using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace RolloBot.Tools
{
    public class ToolConfigItem
    {
        private readonly ToolConfigBase config;

        public object Value { get; set; }
        public object DefaultValue { get; set; }
        public Type Type { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ToolTip { get; set; }
        public string ValidationRegex { get; set; }

        public ToolConfigItem(ToolConfigBase config)
        {
            this.config = config;
        }

        public void Int16Changed(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (short.TryParse(textBox.Text.Trim('_'), out short result))
                {
                    Value = result;
                    config.SaveItem(this);
                }
            }
        }
        public void UInt16Changed(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (ushort.TryParse(textBox.Text.Trim('_'), out ushort result))
                {
                    Value = result;
                    config.SaveItem(this);
                }
            }
        }
        public void Int32Changed(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (int.TryParse(textBox.Text.Trim('_'), out int result))
                {
                    Value = result;
                    config.SaveItem(this);
                }
            }
        }
        public void UInt32Changed(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (uint.TryParse(textBox.Text.Trim('_'), out uint result))
                {
                    Value = result;
                    config.SaveItem(this);
                }
            }
        }
        public void Int64Changed(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (long.TryParse(textBox.Text.Trim('_'), out long result))
                {
                    Value = result;
                    config.SaveItem(this);
                }
            }
        }
        public void UInt64Changed(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (ulong.TryParse(textBox.Text.Trim('_'), out ulong result))
                {
                    Value = result;
                    config.SaveItem(this);
                }
            }
        }

        public void TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                Value = textBox.Text;
                config.SaveItem(this);
            }
        }

        public void Checked(object sender, RoutedEventArgs e)
        {
            Value = true;
            config.SaveItem(this);
        }
        public void Unchecked(object sender, RoutedEventArgs e)
        {
            Value = false;
            config.SaveItem(this);
        }
    }
}
