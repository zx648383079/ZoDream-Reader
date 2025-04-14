using System.Collections.Generic;
using ZoDream.Shared.Text.Models;

namespace ZoDream.Shared.Text
{
    public class WordDictionary
    {
        private readonly CharTrieNode _root = [];

        /// <summary>
        /// 向 Trie 树中插入一个词语
        /// </summary>
        public void Insert(string word)
        {
            var currentNode = _root;
            foreach (char c in word)
            {
                if (!currentNode.TryGetValue(c, out var node))
                {
                    node = [];
                    currentNode.Add(c, node);
                }
                currentNode = node;
            }
            currentNode.IsEndOfWord = true;
            currentNode.Word = word;  // 存储完整词语
        }

        /// <summary>
        /// 检查词语是否存在于 Trie 中
        /// </summary>
        public bool Search(string word)
        {
            var node = SearchNode(word);
            return node != null && node.IsEndOfWord;
        }

        /// <summary>
        /// 检查是否存在以指定前缀开头的词语
        /// </summary>
        public bool StartsWith(string prefix)
        {
            return SearchNode(prefix) != null;
        }

        private CharTrieNode? SearchNode(string word)
        {
            var currentNode = _root;

            foreach (char c in word)
            {
                if (!currentNode.TryGetValue(c, out currentNode))
                {
                    return null;
                }
            }
            return currentNode;
        }

        /// <summary>
        /// 查找所有可能的正确词语（用于错误词语建议）
        /// </summary>
        public List<string> FindSimilarWords(string input, int maxDistance = 2)
        {
            var results = new List<string>();
            FindSimilarWords(_root, input, "", maxDistance, results);
            return results;
        }

        private void FindSimilarWords(CharTrieNode node, string remainingInput, string currentPath,
                                    int remainingDistance, List<string> results)
        {
            // 如果已经找到完整词语且剩余距离允许
            if (node.IsEndOfWord && remainingInput.Length <= remainingDistance)
            {
                results.Add(node.Word);
            }

            // 如果没有剩余输入字符，检查当前节点子节点
            if (remainingInput.Length == 0)
            {
                foreach (var child in node)
                {
                    if (remainingDistance > 0)
                    {
                        FindSimilarWords(child.Value, remainingInput, currentPath + child.Key,
                                       remainingDistance - 1, results);
                    }
                }
                return;
            }

            char nextChar = remainingInput[0];
            string newRemainingInput = remainingInput.Substring(1);

            // 1. 精确匹配情况
            if (node.TryGetValue(nextChar, out var next))
            {
                FindSimilarWords(next, newRemainingInput,
                               currentPath + nextChar, remainingDistance, results);
            }

            if (remainingDistance > 0)
            {
                // 2. 考虑插入、删除和替换操作
                foreach (var child in node)
                {
                    // 插入操作（跳过当前输入字符）
                    FindSimilarWords(child.Value, remainingInput, currentPath + child.Key,
                                     remainingDistance - 1, results);

                    // 替换操作（消耗一个距离）
                    if (child.Key != nextChar)
                    {
                        FindSimilarWords(child.Value, newRemainingInput, currentPath + child.Key,
                                       remainingDistance - 1, results);
                    }
                }
                // 删除操作（跳过当前节点，消耗一个距离）
                FindSimilarWords(node, newRemainingInput, currentPath, remainingDistance - 1, results);
            }
        }

        /// <summary>
        /// 查找所有以指定前缀开头的词语
        /// </summary>
        public List<string> GetWordsWithPrefix(string prefix)
        {
            var node = SearchNode(prefix);
            var words = new List<string>();
            if (node != null)
            {
                CollectWords(node, words);
            }
            return words;
        }

        private void CollectWords(CharTrieNode node, List<string> words)
        {
            if (node.IsEndOfWord)
            {
                words.Add(node.Word);
            }

            foreach (var child in node.Values)
            {
                CollectWords(child, words);
            }
        }
    }
}
