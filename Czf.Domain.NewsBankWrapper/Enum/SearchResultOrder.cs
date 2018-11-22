using System.ComponentModel;

namespace Czf.Domain.NewsBankWrapper.Enum
{
    public enum SearchResultOrder
    {
        [Description("YMD_date:A")]
        AscendingDate,
        [Description("YMD_date:D")]
        DecendingDate,
        [Description("_rank_:D")]
        Relevance
    }
}