using System;
using System.Collections.Generic;
using System.Linq;

namespace Verndale.ContentUsages.Models
{
    public class ContentTypeUsageViewModels
    {
        public ContentTypeUsageViewModels()
        {
            AllContentTypes = Enumerable.Empty<ContentItem>();
        }

        public IEnumerable<ContentItem> AllContentTypes { get; set; }

        public string ContentType { get; set; }

        public int ContentTypeId { get; set; }
    }

    public class ContentItem
    {
        public int ContentId { get; set; }
        public string ContentName { get; set; }
        public string ContentUrl { get; set; }
        public DateTime? PublicationDate { get; set; }
        public DateTime LastChanged { get; set; }
        public string ChangedBy { get; set; }
        public string Language { get; set; }
        public string ContentType { get; set; }
        public int ContentTypeId { get; set; }
    }
}