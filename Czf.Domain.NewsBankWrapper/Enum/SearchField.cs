using System.ComponentModel;

namespace Czf.Domain.NewsBankWrapper.Enum
{
    //<select class="field-select form-select" id="edit-fld-base-0" name="fld-base-0"><option value = "alltext" > Select a Field(optional)</option><option value = "alltext" > All Text</option><option value = "Lead" > Lead / First Paragraph</option><option value = "Title" > Headline </ option >< option value="Author">Author/Byline</option><option value = "Section" > Section </ option >< option value="CapGraph">Caption</option><option value = "Page" > Page </ option >< option value="source">Source</option><option value = "YMD_date" > Date(s) </ option >< option value="wct">Word Count</option><option value = "lexs" > Readability / Lexile </ option >< option value= "dti" > Added within</option><option value = "papervariant" > Title as Published</option> < option value= "PTY" > Article Type</option></select>
    public enum SearchField
    {
        [Description("alltext")]
        AllText,
        [Description("Author")]
        Author,
        [Description("YMD_date")]
        Date,
        [Description("Title")]
        Headline
    }
}