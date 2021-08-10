using System.Collections.ObjectModel;
using System.Windows.Media;

namespace CustomRichTextFormatter.Common
{
    public class FontFamilyList : ObservableCollection<string>
    {
        public FontFamilyList()
        {
            foreach (var f in Fonts.SystemFontFamilies)
            {
                Add(f.ToString());
            }
        }
    }
    public class FontHeightList : ObservableCollection<string>
    {
        public FontHeightList()
        {
            Add("8");
            Add("9");
            Add("10");
            Add("11");
            Add("12");
            Add("14");
            Add("16");
            Add("18");
            Add("20");
            Add("22");
            Add("24");
            Add("26");
            Add("28");
            Add("36");
            Add("48");
            Add("72");
        }
    }
    
}
