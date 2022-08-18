﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Helper
{
    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; }

        int PageSize { get; }
        int TotalCount { get; }

        int TotalPages { get; }

        bool HasPreviousPage { get; }

        bool HasNextPage { get; }
        object MetaData();
    }
}
