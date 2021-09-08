using System;
using System.Collections.Generic;

namespace Search_Fight
{
    class Program
    {
        static void Main(string[] args)
        {
            GoogleSearcher googleSearcher = new GoogleSearcher();
            BingSearcher bingSearcher = new BingSearcher();

            List<SearchResult> googleSearchResults = googleSearcher.SearchMultiple(args);
            List<SearchResult> bingearchResults = bingSearcher.SearchMultiple(args);

            Console.WriteLine(args[0] + " " + args[1]);
        }

        
    }

    class SearchResult
    {
        public string SearchEngine { get; private set; }
        public string SearchWord { get; private set; }
        public int SearchHits { get; private set; }
        public SearchResult(string searchEngine, string searchWord, int searchHits)
        {
            SearchEngine = searchEngine;
            SearchWord = searchWord;
            SearchHits = searchHits;
        }
    }

    class MockCreator
    {
        static public Random r = new Random();
    }


    abstract class EngineSercher
    {
        public SearchResult Search(string searchWord)
        {
            searchWord = searchWord == null ? "" : searchWord.Trim();

            if (searchWord.Equals(""))
            {
                throw new InvalidOperationException("Search word empty or null");
            }

            return ExecuteSearch(searchWord);
        }

        public List<SearchResult> SearchMultiple(string[] searchWords)
        {
            var result = new List<SearchResult>();

            if (searchWords == null)
            {
                return result;
            }

            foreach (string el in searchWords)
            {
                var newElement = Search(el);
                result.Add(newElement);
            }

            return result;
        }

        protected abstract SearchResult ExecuteSearch(string searchWord);
    }

    class GoogleSearcher : EngineSercher
    {
        public GoogleSearcher()
        {
            // Code for establishing connection with Google
        }

        protected override SearchResult ExecuteSearch(string searchWord)
        {
            // Code for performing Google search. But right now just creating Mock data. 
            return new SearchResult("Google", searchWord, MockCreator.r.Next());
        }
    }

    class BingSearcher : EngineSercher
    {
        public BingSearcher()
        {
            // Code for establishing connection with Bing
        }

        protected override SearchResult ExecuteSearch(string searchWord)
        {
            // Code for performing Bing search. But right now just creating Mock data. 
            return new SearchResult("Bing", searchWord, MockCreator.r.Next());
        }
    }
}
