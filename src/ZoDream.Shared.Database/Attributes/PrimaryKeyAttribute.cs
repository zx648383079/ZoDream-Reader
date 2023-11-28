using System;

namespace ZoDream.Shared.Database
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyAttribute(string primaryKey) : Attribute
    {
        public PrimaryKeyAttribute(string[] primaryKey) : this(string.Join(",", primaryKey))
        {
        }

        public string Value { get; private set; } = primaryKey;
        private bool _autoIncrement = true;
        public bool AutoIncrement {
            get { return _autoIncrement; }
            set {
                _autoIncrement = value;
                if (value && Value.Contains(","))
                {
                    throw new InvalidOperationException("Cannot set AutoIncrement to true when the primary key is a Composite Key");
                }
            }
        }
    }
}
