using System.Collections.Generic;

namespace ZoDream.Shared.Text.Models
{
    public class CharTrieNode : Dictionary<char, CharTrieNode>
    {
        public bool IsEndOfWord { get; set; }
        public string Word { get; set; } = string.Empty;
    }
}
