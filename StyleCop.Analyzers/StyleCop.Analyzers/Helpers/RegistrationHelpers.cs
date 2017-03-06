// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;

    /// <summary>
    /// Helper class used to determine if files are excluded from analysis
    /// </summary>
    internal static class RegistrationHelpers
    {
        internal static void Register(AnalysisContext analysisContext, Action<SyntaxTreeAnalysisContext> syntaxTreeAction)
        {
            analysisContext.RegisterSyntaxTreeActionWithExclusionsVerification((context) =>
            {
                if (FileExclusionHelpers.IsFileExcludedFromAnalysis(context))
                {
                    return;
                }
                syntaxTreeAction.Invoke(context);
            });
        }
    }
}
