using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Form
{
    public static class Input
    {
        public static TextFormInput Text(string name, string label, bool required = false)
        {
            return new TextFormInput(name, label, required);
        }

        public static TextFormInput Url(string name, string label, bool required = false)
        {
            return new TextFormInput(name, label, required);
        }

        public static SwitchFormInput Switch(string name, string label)
        {
            return new SwitchFormInput(name, label);
        }

        public static NumericFormInput Numeric(string name, string label)
        {
            return new NumericFormInput(name, label);
        }

        public static FileFormInput File(string name, string label, 
            bool required = false, 
            bool isSave = false, bool isFolder = false)
        {
            return new FileFormInput(name, label, required, isSave, isFolder);
        }

        public static SelectFormInput Select(string name, string label, DataItem[] items)
        {
            return new SelectFormInput(name, label, items);
        }
    }
}
