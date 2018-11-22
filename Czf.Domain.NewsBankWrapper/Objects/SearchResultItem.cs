using System;

namespace Czf.Domain.NewsBankWrapper.Objects
{
    public class SearchResultItem
    {
        /// <summary>absolute URL to the result</summary>
        public Uri ResultItemUri { get; set; }
        
        /// <summary>
        /// Text used for the item uri
        /// </summary>
        public string ItemText { get; set; }
    }
}