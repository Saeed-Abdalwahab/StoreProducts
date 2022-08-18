using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store_TechniaclTask.Services.Resources
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly IDistributedCache _cache;
        private readonly IStringLocalizer stringLocalizer;

        public JsonStringLocalizerFactory(IDistributedCache cache, IStringLocalizer stringLocalizer)
        {
            _cache = cache;
            this.stringLocalizer = stringLocalizer;
        }
        public IStringLocalizer Create(Type resourceSource) => stringLocalizer;
        public IStringLocalizer Create(string baseName, string location) =>
            stringLocalizer;
    }
}
