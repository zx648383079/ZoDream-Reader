using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoDream.Reader.Events
{
    public delegate void ValueChangedEventHandler<T>(object sender, T value);
}
