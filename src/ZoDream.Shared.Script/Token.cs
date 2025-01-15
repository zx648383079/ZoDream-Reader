namespace ZoDream.Shared.Script
{
    public class Token
    {
        public Token(TokenType type, string value)
            : this(type)
        {
            Value = value;
        }

        public Token(TokenType type)
        {
            Type = type;
        }


        public TokenType Type { get; private set; }

        public string Value { get; private set; } = string.Empty;

        public override string ToString()
            => string.Format("{0}='{1}'", Type, Value);
    }
}
