using System.Collections.Generic;

namespace SiteAtt.List.Models ;

    public class CreateSitesRequest
    {
        public IEnumerable<string> Sites { get; set; }
    }