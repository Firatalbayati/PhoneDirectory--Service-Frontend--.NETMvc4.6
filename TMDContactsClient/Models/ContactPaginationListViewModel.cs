using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TMDContactsClient.Models
{
    public class ContactPaginationListViewModel
    {
        public List<Contacts> Data { get; set; }
        public int FirstPage { get; set; }
        public int PreviousPage { get; set; }
        public int CurrentPage { get; set; }
        public int NextPage { get; set; }
        public int LastPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

    }
}