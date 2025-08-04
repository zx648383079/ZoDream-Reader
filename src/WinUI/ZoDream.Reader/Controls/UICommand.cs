using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Windows.Input;
using Windows.System;

namespace ZoDream.Reader.Controls
{
    internal static class UICommand
    {
        public static ICommand Copy(Action execute) => Copy(new RelayCommand(execute));
        public static ICommand Copy(ICommand command) => new StandardUICommand(StandardUICommandKind.Copy)
        {
            Command = command
        };
        public static ICommand Delete(Action execute) => Delete(new RelayCommand(execute));
        public static ICommand Delete(ICommand command) => new StandardUICommand(StandardUICommandKind.Delete)
        {
            Command = command
        };
        public static ICommand Play(Action execute) => Play(new RelayCommand(execute));
        public static ICommand Play(ICommand command) => new StandardUICommand(StandardUICommandKind.Play)
        {
            Command = command
        };
        public static ICommand Pause(Action execute) => Pause(new RelayCommand(execute));
        public static ICommand Pause(ICommand command) => new StandardUICommand(StandardUICommandKind.Pause)
        {
            Command = command
        };
        public static ICommand Stop(Action execute) => Stop(new RelayCommand(execute));
        public static ICommand Stop(ICommand command) => new StandardUICommand(StandardUICommandKind.Stop)
        {
            Command = command
        };
        public static ICommand Resume(Action execute) => Resume(new RelayCommand(execute));
        public static ICommand Resume(ICommand command) => new StandardUICommand(StandardUICommandKind.Redo)
        {
            Command = command
        };
        public static ICommand Save(Action execute) => Save(new RelayCommand(execute));
        public static ICommand Save(ICommand command) => new StandardUICommand(StandardUICommandKind.Save)
        {
            Command = command
        };
        public static ICommand SelectAll(Action execute) => SelectAll(new RelayCommand(execute));
        public static ICommand SelectAll(ICommand command) => new StandardUICommand(StandardUICommandKind.SelectAll)
        {
            Command = command
        };
        public static ICommand Backward(Action execute) => Backward(new RelayCommand(execute));
        public static ICommand Backward(ICommand command) => new StandardUICommand(StandardUICommandKind.Backward)
        {
            Command = command
        };
        public static ICommand Info(Action execute) => Info(new RelayCommand(execute));
        public static ICommand Info(ICommand command) => new XamlUICommand()
        {
            Label = "Info",
            Description = "View information",
            IconSource = new FontIconSource()
            {
                Glyph = "\uE946"
            },
            KeyboardAccelerators =
            {
                new KeyboardAccelerator()
                {
                    Key = VirtualKey.I,
                    Modifiers = VirtualKeyModifiers.Control
                }
            },
            Command = command
        };

        public static ICommand View(Action execute) => View(new RelayCommand(execute));
        public static ICommand View<T>(Action<T?> execute) => View(new RelayCommand<T>(execute));
        public static ICommand View(ICommand command) => new XamlUICommand()
        {
            Label = "View",
            Description = "View",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.View
            },
            Command = command
        };
        public static ICommand Add(Action execute) => Add(new RelayCommand(execute));
        public static ICommand Add(ICommand command) => new XamlUICommand()
        {
            Label = "Add",
            Description = "Add files",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.Add
            },
            KeyboardAccelerators =
            {
                new KeyboardAccelerator()
                {
                    Key = VirtualKey.A,
                    Modifiers = VirtualKeyModifiers.Control
                }
            },
            Command = command
        };
        public static ICommand AddFolder(Action execute) => AddFolder(new RelayCommand(execute));
        public static ICommand AddFolder(ICommand command) => new XamlUICommand()
        {
            Label = "Add",
            Description = "Add folder",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.OpenLocal
            },
            Command = command
        };
        public static ICommand Find(Action execute) => Find(new RelayCommand(execute));
        public static ICommand Find(ICommand command) => new XamlUICommand()
        {
            Label = "Find",
            Description = "Find",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.Find
            },
            KeyboardAccelerators =
            {
                new KeyboardAccelerator()
                {
                    Key = VirtualKey.F,
                    Modifiers = VirtualKeyModifiers.Control
                }
            },
            Command = command
        };

        public static ICommand Setting(Action execute) => Setting(new RelayCommand(execute));
        public static ICommand Setting(ICommand command) => new XamlUICommand()
        {
            Label = "Setting",
            Description = "Setting",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.Setting
            },
            KeyboardAccelerators =
            {
                new KeyboardAccelerator()
                {
                    Key = VirtualKey.O,
                    Modifiers = VirtualKeyModifiers.Control
                }
            },
            Command = command
        };
        public static ICommand MultipleSelect(Action execute) => MultipleSelect(new RelayCommand(execute));
        public static ICommand MultipleSelect(ICommand command) => new XamlUICommand()
        {
            Label = "管理",
            Description = "批量多选管理",
            IconSource = new FontIconSource()
            {
                Glyph = "\uE762"
            },
            Command = command
        };
        public static ICommand Edit(Action execute) => Edit(new RelayCommand(execute));
        public static ICommand Edit(ICommand command) => new XamlUICommand()
        {
            Label = "编辑",
            Description = "编辑选中",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.Edit
            },
            Command = command
        };

        public static ICommand Group(Action execute) => Group(new RelayCommand(execute));
        public static ICommand Group(ICommand command) => new XamlUICommand()
        {
            Label = "分组",
            Description = "管理分组",
            IconSource = new FontIconSource()
            {
                Glyph = "\uEC26"
            },
            Command = command
        };

        public static ICommand Create(Action execute) => Create(new RelayCommand(execute));
        public static ICommand Create(ICommand command) => new XamlUICommand()
        {
            Label = "创建",
            Description = "创建",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.NewFolder
            },
            Command = command
        };

        public static ICommand Sync(Action execute) => Sync(new RelayCommand(execute));
        public static ICommand Sync(ICommand command) => new XamlUICommand()
        {
            Label = "同步",
            Description = "同步",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.Sync
            },
            Command = command
        };

        public static ICommand Import(Action execute) => Import(new RelayCommand(execute));
        public static ICommand Import(ICommand command) => new XamlUICommand()
        {
            Label = "导入",
            Description = "导入",
            IconSource = new SymbolIconSource()
            {
                Symbol = Symbol.Import
            },
            Command = command
        };

        public static ICommand Export(Action execute) => Export(new RelayCommand(execute));
        public static ICommand Export(ICommand command) => new XamlUICommand()
        {
            Label = "导出",
            Description = "导出",
            IconSource = new FontIconSource()
            {
                Glyph = "\uEDE1"
            },
            Command = command
        };
    }
}
