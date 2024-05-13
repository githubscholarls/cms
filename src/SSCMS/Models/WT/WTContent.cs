using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSCMS.Models.WT
{
    public class WTContent
    {
        public FirendLink FirendLink { get; set; }
    }

    public class FirendLink
    {
        public string FromPro { get; set; }
        public string FromCity { get; set; }
        public string FromArea { get; set; }
        public string ToPro { get; set; }
        public string ToCity { get; set; }
        public string ToArea { get; set; }
    }
}
