using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Form
{
    public class TextFormInput(string name, string label, bool required = false) : IFormInput
    {
        public string Name { get; private set; } = name;

        public string Label { get; private set; } = label;

        public bool Required { get; private set; } = required;

        public string Tip { get; private set; } = string.Empty;

        public bool TryParse(ref object input)
        {
            return true;
        }
    }
}
