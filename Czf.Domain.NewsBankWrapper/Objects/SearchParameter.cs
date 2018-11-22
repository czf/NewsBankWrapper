using Czf.Domain.NewsBankWrapper.Enum;

namespace Czf.Domain.NewsBankWrapper.Objects
{
    public class SearchParameter
    {
        public string Value { get; set; }
        public SearchField Field { get; set; }
        public CompoundOperator ParameterCompoundOperator { get; set;  }
    }
}