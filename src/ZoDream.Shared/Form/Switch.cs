using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Form
{
    public class SwitchFormInput(string name, string label) : IFormInput
    {
        public string Name { get; private set; } = name;

        public string Label { get; private set; } = label;

        public string Tip { get; private set; } = string.Empty;

        public bool TryParse(ref object input)
        {
            if (input is bool)
            {
            } else if (input is null)
            {
                input = false;
            } else
            {
                var val = input.ToString().ToUpper();
                input = val == "1" || val == "Y" || val == "TRUE";
            }
            return true;
        }
    }
}
