using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.DAL.Helper
{
    public partial interface ISoftDeletedEntity
    {
        bool IsDeleted { get; set; }
    }
}
