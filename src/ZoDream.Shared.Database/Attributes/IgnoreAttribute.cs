﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Database
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class IgnoreAttribute : Attribute
    {
    }
}
