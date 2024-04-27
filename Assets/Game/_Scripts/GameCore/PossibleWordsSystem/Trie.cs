using System;
using System.Collections.Generic;

namespace GameCore.PossibleWordsSystem
{
    public class Trie
    {
        private readonly TrieNode root;

        public Trie()
        {
            root = new TrieNode();
        }

        public void Insert(string word)
        {
            TrieNode node = root;
            foreach (var ch in word)
            {
                if (!node.Children.ContainsKey(ch))
                {
                    node.Children[ch] = new TrieNode();
                }
                node = node.Children[ch];
            }
            node.IsEndOfWord = true;
        }

        public bool Search(string word)
        {
            TrieNode node = root;
            foreach (var ch in word)
            {
                if (!node.Children.ContainsKey(ch))
                {
                    return false;
                }
                node = node.Children[ch];
            }
            return node.IsEndOfWord;
        }

        public bool IsEmpty()
        {
            return root.Children.Count == 0;
        }

        // Toplam kelime sayısını döndüren Count fonksiyonu
        public int Count()
        {
            return CountWords(root);
        }

        // Özyinelemeli olarak kelime sayısını hesaplayan yardımcı fonksiyon
        private int CountWords(TrieNode node)
        {
            int count = 0;

            // Eğer geçerli düğüm bir kelimenin sonuysa, sayacı artır
            if (node.IsEndOfWord)
            {
                count++;
            }

            // Her bir çocuk için özyinelemeli olarak fonksiyonu çağır ve sonuçları topla
            foreach (var child in node.Children.Values)
            {
                count += CountWords(child);
            }

            return count;
        }
    }
}
