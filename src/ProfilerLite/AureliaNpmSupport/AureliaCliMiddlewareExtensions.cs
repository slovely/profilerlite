using System;
using Microsoft.AspNetCore.SpaServices;

namespace ProfilerLite.AureliaNpmSupport
{
    public static class AureliaCliMiddlewareExtensions
    {
        public static void UseAureliaCliServer(this ISpaBuilder spaBuilder, string npmScript)
        {
            if (spaBuilder == null)
                throw new ArgumentNullException(nameof(spaBuilder));
            if (string.IsNullOrEmpty(spaBuilder.Options.SourcePath))
                throw new InvalidOperationException("To use UseAureliaCliServer, you must supply a non-empty value for the SourcePath property of SpaOptions when calling UseSpa.");
            AureliaCliMiddleware.Attach(spaBuilder, npmScript);
        }
    }
}