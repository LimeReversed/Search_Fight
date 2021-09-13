using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using HtmlAgilityPack;
using System.IO;
using System.Globalization;

namespace Search_Fight
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No search words given as arguments");
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

            //WebScrap("tiger");
            Console.ReadKey();

        }

        public static void WebScrap(string searchWord)
        {
            HtmlWeb web = new HtmlWeb(); 
            HtmlDocument doc = web.Load($"https://www.bing.com/search?q={searchWord}");
            var headerNames = doc.DocumentNode.SelectNodes("//span[@class='sb_count']") ?? doc.DocumentNode.SelectNodes("/ html[1] / body[1] / div[1] / div[1] / main[1] / ol[1] / li[1] / h1[1]");
            //var headerNames = doc.DocumentNode.SelectNodes("//main");

            foreach (var item in headerNames) { Console.WriteLine(item.InnerText); }
        }

        /// <summary>
        /// Formats the search result so that it can be displayed as a string. 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="engine"></param>
        /// <returns></returns>
        public static string FormatSearchResult(List<SearchResult> result, string engine)
        {
            if (result == null || result.Count < 1)
            {
                throw new NullReferenceException("List is null or empty");
            }

            StringBuilder builder = new StringBuilder();
            builder.Append(engine + ": ");

            foreach (SearchResult el in result)
            {
                builder.Append($"\n {el.SearchWord}: {el.SearchHits}");
            }
            builder.Append("\n");

            return builder.ToString();
        }

        public static SearchResult GetWinner(List<SearchResult> list)
        {
            if (list == null || list.Count < 1)
            {
                throw new NullReferenceException("List is null or empty");
            }

            SearchResult currentLeader = list[0];

            foreach (SearchResult el in list)
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
            EngineName = "Google";
            // Code for establishing connection with Google
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
            EngineName = "Bing";
            // Code for establishing connection with Bing
        }

        protected override SearchResult ExecuteSearch(string searchWord)
        {
            // Code for performing Bing search. But right now just creating Mock data. 
            return new SearchResult(searchWord, MockCreator.r.Next());
        }
    }
}
