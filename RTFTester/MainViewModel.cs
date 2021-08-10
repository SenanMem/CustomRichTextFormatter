using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomRichTextFormatter.Common;

namespace RTFTester
{
    public class MainViewModel : ViewModelBase
    {
        private string _uidString;
        private string _tagString;

        public string UidString
        {
            get => _uidString;

            set => base.SetProperty(ref _uidString, value, "UidString");
        }
        public string TagString
        {
            get => _tagString;

            set => SetProperty(ref _tagString, value, "TagString");
        }
    }
}
