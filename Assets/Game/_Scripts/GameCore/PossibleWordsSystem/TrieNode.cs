using System.Collections.Generic;

namespace GameCore.PossibleWordsSystem
{
    public class TrieNode
    {
        public Dictionary<char, TrieNode> Children;
        public bool IsEndOfWord;

        public TrieNode()
        {
            Children = new Dictionary<char, TrieNode>();
            IsEndOfWord = false;
        }
    }
}
