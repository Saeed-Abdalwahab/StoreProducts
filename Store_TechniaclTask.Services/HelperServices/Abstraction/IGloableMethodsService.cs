using Store_TechniaclTask.DAL.Enums;
using Store_TechniaclTask.Services.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.Services.HelperServices.Abstraction
{
    public interface IGloableMethodsService
    {
        string RemoveErrorMessage<Tentity>(Tentity entity, string Name = null);
        bool IsRangesOverlap(DateTime StartA, DateTime EndA, DateTime StartB, DateTime EndB);
        bool IsRangesOverlap(TimeSpan StartA, TimeSpan EndA, TimeSpan StartB, TimeSpan EndB);
        Language CurrentUserLanguage();
        string CurrentUserID();
        string CurrentUserEmail();
    }
}
