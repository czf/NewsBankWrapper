using System.Collections.Generic;
using Czf.Domain.NewsBankWrapper.Enum;

namespace Czf.Domain.NewsBankWrapper.Objects
{
    public class SearchRequest
    {
        public List<Publication> Publications {get; set;}
        public SearchParameter SearchParameter0 { get; set; }
        public SearchParameter SearchParameter1 { get; set; }
        public SearchParameter SearchParameter2 { get; set; }
        public SearchParameter SearchParameter3 { get; set; }
        public SearchParameter SearchParameter4 { get; set; }
        public SearchParameter SearchParameter5 { get; set; }
        public SearchParameter SearchParameter6 { get; set; }
        public SearchParameter SearchParameter7 { get; set; }
        public SearchParameter SearchParameter8 { get; set; }
        public SearchParameter SearchParameter9 { get; set; }
        
        public SearchResultOrder SortOrder { get; set; }
        public Product Product { get; set; }
        //TODO missing params
    }
}