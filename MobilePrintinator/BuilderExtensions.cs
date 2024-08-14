using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobilePrintinator
{
    public static class BuilderExtensions
    {
        public static MauiAppBuilder UseMobilePrintinator(this MauiAppBuilder builder)
        {
            builder.Services.AddSingleton<IMobilePrintinatorService, MobilePrintinatorService>();
            return builder;
        }
    }
}
