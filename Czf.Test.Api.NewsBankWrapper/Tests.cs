﻿using System;
using System.Collections.Generic;
using Czf.Api.NewsBankWrapper;
using Czf.Domain.NewsBankWrapper.Enum;
using Czf.Domain.NewsBankWrapper.Interfaces;
using Czf.Domain.NewsBankWrapper.Objects;
using Czf.Test.Api.NewsBankWrapper.IgnoredInterfaceImplementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Czf.Test.Api.NewsBankWrapper
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestSignIn()
        {
            NewsBankClient client = new NewsBankClient(new urlprovider(), new credProvider(), new baseurlprovider(), new log());
            client.SignIn().Wait();
        }
        [TestMethod]
        public void TestMethodSeach()
        {
            NewsBankClient client = new NewsBankClient(new urlprovider(), new credProvider(), new baseurlprovider(), new log());
            SearchRequest request = new SearchRequest()
            {
                Publications = new List<Publication>() { Publication.SeattleTimesWebEditionArticles },
                SearchParameter0  = new SearchParameter() { Field = SearchField.Author, Value = "Agueda Pacheco-Flores" },
                SearchParameter1 = new SearchParameter() { Field = SearchField.Headline, Value = "More stores closing on Thanksgiving as online shopping booms; here’s what’s open, closed in the Seattle area" },
                SearchParameter2 = new SearchParameter() { Field = SearchField.Date, Value = "11/19/2018" }
                
            };
            SearchResult searchResult = client.Search(request).Result;
            string expectedText = "More stores closing on Thanksgiving as online shopping booms; here's what's open, closed in the Seattle area - American consumers are expected to spend about $720 billion on holiday shopping this year, an annual increase of more than 4 percent, according to the National Retail Federation.";
            Uri expectedUri = new Uri("https://infoweb-newsbank-com.ezproxy.spl.org/apps/news/document-view?p=WORLDNEWS&docref=news/16FCFBD6F00BE188");
            Assert.IsNotNull(searchResult);
            Assert.IsNotNull(searchResult.FirstSearchResultItem);
            Assert.IsNotNull(searchResult.FirstSearchResultItem.ItemText);
            Assert.AreEqual(expectedText, searchResult.FirstSearchResultItem.ItemText);
            Assert.AreEqual(expectedUri, searchResult.FirstSearchResultItem.ResultItemUri);

        }
    }
    
    public class log : ICanLog
    {
        public void Error(string message)
        {
            
        }

        public void Info(string message)
        {
            
        }
    }


}
