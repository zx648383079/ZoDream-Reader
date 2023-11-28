using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Form
{
    public class Select(string name, string label, DataItem[] items) : IFormInput
    {
        public string Name { get; private set; } = name;

        public string Label { get; private set; } = label;

        public DataItem[] Items { get; private set; } = items;

        public string Tip { get; private set; } = string.Empty;

        public bool TryParse(ref object input)
        {
            foreach (var item in Items)
            {
                if (input == item)
                {
                    return true;
                }
            }
            input = Items[0].Value;
            return true;
        }
    }
}
