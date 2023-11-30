using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using ZoDream.Shared.Form;
using ZoDream.Shared.Interfaces;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoDream.Reader.Dialogs
{
    public sealed partial class FormDialog : ContentDialog
    {
        public FormDialog()
        {
            this.InitializeComponent();
        }

        public Dictionary<string, object> FormData { get; private set; } = new();

        public void RenderForm(IEnumerable<IFormInput> inputItems, Dictionary<string, object>? data)
        {
            FormData.Clear();
            if (data is not null)
            {
                foreach (var item in data)
                {
                    FormData.Add(item.Key, item.Value);
                }
            }
            UpdateView(inputItems);
        }

        private void UpdateView(IEnumerable<IFormInput> inputItems)
        {
            var i = -1;
            foreach (var item in inputItems)
            {
                i++;
                var j = 2 * i;
                AddLabel(FormPanel.Children, item, j);
                AddInput(FormPanel.Children, item, j + 1);
            }
            i++;
            RemoveRange(2 * i, FormPanel.Children.Count - 2 * i);
        }

        private void RemoveRange(int begin, int count)
        {
            for (int i = count - 1; i >= 0; i--)
            {
                FormPanel.Children.RemoveAt(i + begin);
            }
        }

        private void AddInput(UIElementCollection children, IFormInput item, int i)
        {
            if (item is TextFormInput)
            {
                AddTextInput(children, item, i);
                return;
            }
            if (item is FileFormInput)
            {
                AddFileInput(children, item, i);
                return;
            }
            if (item is NumericFormInput)
            {
                AddNumericInput(children, item, i);
                return;
            }
            if (item is SwitchFormInput)
            {
                AddSwitchInput(children, item, i);
                return;
            }
            if (item is SelectFormInput o)
            {
                AddSelectInput(children, o, i);
                return;
            }
        }

        private void AddTextInput(UIElementCollection children, IFormInput item, int i)
        {
            var ctl = new TextBox()
            {
                Text = GetValue<string>(item.Name) ?? string.Empty,
                VerticalContentAlignment = VerticalAlignment.Center,
                Height = 32,
            };
            ctl.TextChanged += (s, o) => {
                UpdateValue(item.Name, ctl.Text.Trim());
            };
            if (children.Count <= i)
            {
                children.Add(ctl);
                return;
            }
            children.RemoveAt(i);
            children.Insert(i, ctl);
        }

        private void AddFileInput(UIElementCollection children, IFormInput item, int i)
        {
            var ctl = new TextBox()
            {
                Text = GetValue<string>(item.Name) ?? string.Empty,
                Height = 32,
            };
            ctl.TextChanged += (s, o) => {
                UpdateValue(item.Name, ctl.Text.Trim());
            };
            if (children.Count <= i)
            {
                children.Add(ctl);
                return;
            }
            children.RemoveAt(i);
            children.Insert(i, ctl);
        }

        private void AddNumericInput(UIElementCollection children, IFormInput item, int i)
        {
            var ctl = new NumberBox()
            {
                Value = GetValue<double>(item.Name),
                Height = 32,
                SpinButtonPlacementMode = NumberBoxSpinButtonPlacementMode.Compact
            };
            ctl.ValueChanged += (s, o) => {
                UpdateValue(item.Name, ctl.Value);
            };
            if (children.Count <= i)
            {
                children.Add(ctl);
                return;
            }
            children.RemoveAt(i);
            children.Insert(i, ctl);
        }

        private void AddSwitchInput(UIElementCollection children, IFormInput item, int i)
        {
            var ctl = new ToggleSwitch()
            {
                IsOn = GetValue<bool>(item.Name),
                Height = 32,
            };
            ctl.Toggled += (s, o) => {
                UpdateValue(item.Name, ctl.IsOn);
            };
            if (children.Count <= i)
            {
                children.Add(ctl);
                return;
            }
            children.RemoveAt(i);
            children.Insert(i, ctl);
        }

        private void AddSelectInput(UIElementCollection children, SelectFormInput item, int i)
        {
            var val = GetValue<object>(item.Name);
            var ctl = new ComboBox()
            {
                ItemsSource = item.Items,
                DisplayMemberPath = "Name",
                Height = 32,
            };
            for (int j = 0; j < item.Items.Length; j++)
            {
                if (item.Items[j].Value == val)
                {
                    ctl.SelectedIndex = j;
                }
            }
            ctl.SelectionChanged += (s, o) => {
                UpdateValue(item.Name, item.Items[ctl.SelectedIndex].Value);
            };
            if (children.Count <= i)
            {
                children.Add(ctl);
                return;
            }
            children.RemoveAt(i);
            children.Insert(i, ctl);
        }

        private void AddLabel(UIElementCollection children, IFormInput item, int i)
        {
            if (children.Count > i)
            {
                (children[i] as TextBlock)!.Text = item.Label;
            }
            else
            {
                children.Add(new TextBlock()
                {
                    Text = item.Label,
                    Margin = new Thickness(0, 10, 0, 0)
                });
            }
        }

        private T? GetValue<T>(string key)
        {
            if (!FormData.TryGetValue(key, out var val))
            {
                return default;
            }
            return (T)val;
        }

        private void UpdateValue(string key, object value)
        {
            if (FormData.ContainsKey(key))
            {
                FormData[key] = value;
            } else
            {
                FormData.Add(key, value);
            }
        }
    }
}
