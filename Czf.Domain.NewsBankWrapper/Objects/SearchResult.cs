using System;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using System.Web;
using System.Linq;
using System.Collections.Specialized;

namespace Czf.Domain.NewsBankWrapper.Objects
{
    public class SearchResult
    {
        /// <summary>
        /// First result of the search response
        /// </summary>
        public SearchResultItem  FirstSearchResultItem{ get; protected set; }
        //TODO: allow returning all results instead of just first
        //TODO: public IReadOnlyList<Uri> PaginationLinks {  get; protected set; }

        public SearchResult(HttpResponseMessage searchResultMessage)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            string content = searchResultMessage.Content.ReadAsStringAsync().Result;
            htmlDocument.LoadHtml(content);

            HtmlNode firstArticle = htmlDocument.GetElementbyId("search-hits__hit--1");
            HtmlNode itemNode = firstArticle.SelectSingleNode("div[2]/div/div[1]/h3/a");
            

            FirstSearchResultItem = new SearchResultItem()
            {
                ItemText = itemNode.SelectSingleNode("text()").InnerText,
                ResultItemUri = BuildArticleUri(searchResultMessage, itemNode).Uri
            };

           
        }

        private UriBuilder BuildArticleUri(HttpResponseMessage searchResultMessage, HtmlNode itemNode)
        {
            Uri baseUri = new Uri(searchResultMessage.RequestMessage.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped));
            Uri fullSearchResultUri = new Uri(baseUri, itemNode.GetAttributeValue("href", null));
            NameValueCollection queryCollection = HttpUtility.ParseQueryString(HttpUtility.HtmlDecode(fullSearchResultUri.Query));
            UriBuilder builder = new UriBuilder(fullSearchResultUri.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped));
            builder.Query = $"p={queryCollection["p"]}&docref={queryCollection["docref"]}";
            return builder;
        }
    }
}