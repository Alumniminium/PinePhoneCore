using System;

namespace wifimon
{
    public partial class OUI
    {
        public string Oui { get; set; }
        public bool IsPrivate { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CountryCode { get; set; }
        public string AssignmentBlockSize { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public DateTimeOffset DateUpdated { get; set; }
    }
}