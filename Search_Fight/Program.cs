using System;
using System.Collections.Generic;
using System.Text;

namespace Search_Fight
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No arguments given");
                Console.ReadKey();
                return;
            }

            GoogleSearcher googleSearcher = new GoogleSearcher();
            BingSearcher bingSearcher = new BingSearcher();

            List<EngineSercher> allSearchers = new List<EngineSercher> 
            { 
                googleSearcher,
                bingSearcher
            };

            List<SearchResult> winners = new List<SearchResult>();

            foreach (EngineSercher el in allSearchers)
            {
                List<SearchResult> searchResults = el.SearchMultiple(args);
                Console.WriteLine(FormatSearchResult(searchResults, el.EngineName));
                SearchResult engineWinner = GetWinner(searchResults);
                winners.Add(engineWinner);
                Console.WriteLine($"{el.EngineName} winner: " + engineWinner.SearchWord + "\n");
            }

            var winner = GetWinner(winners);

            Console.WriteLine($"Total Winner: {winner.SearchWord}");
            Console.ReadKey();

        }

        /// <summary>
        /// Formats the search result so that it can be displayed as a string. 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string FormatSearchResult(List<SearchResult> result, string engine)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(engine + ": ");

            foreach (SearchResult el in result)
            {
                builder.Append($"\n {el.SearchWord}: {el.SearchHits}");
            }
            builder.Append("\n");

            return builder.ToString();
        }

        public static SearchResult GetWinner(List<SearchResult> result)
        {
            SearchResult currentLeader = result[0];

            foreach (SearchResult el in result)
            {
                if (el.SearchHits > currentLeader.SearchHits)
                {
                    currentLeader = el;
                }
            }

            return currentLeader;
        }
    }

    class SearchResult
    {
        public string SearchWord { get; private set; }
        public int SearchHits { get; private set; }
        public SearchResult(string searchWord, int searchHits)
        {
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
        public string EngineName { get; protected set; }

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
            EngineName = "Google";
        }

        protected override SearchResult ExecuteSearch(string searchWord)
        {
            // Code for performing Google search. But right now just creating Mock data. 
            return new SearchResult(searchWord, MockCreator.r.Next());
        }
    }

    class BingSearcher : EngineSercher
    {
        public BingSearcher()
        {
            // Code for establishing connection with Bing
            EngineName = "Bing";
        }

        protected override SearchResult ExecuteSearch(string searchWord)
        {
            // Code for performing Bing search. But right now just creating Mock data. 
            return new SearchResult(searchWord, MockCreator.r.Next());
        }
    }
}
