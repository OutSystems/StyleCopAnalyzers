// Copyright (c) Tunnel Vision Laboratories, LLC. All Rights Reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace StyleCop.Analyzers.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Settings.ObjectModel;
    using StyleCop.Analyzers;

    /// <summary>
    /// Helper class used to determine if files are excluded from analysis
    /// </summary>
    internal static class FileExclusionHelpers
    {
        internal static bool IsFileExcludedFromAnalysis(SyntaxTreeAnalysisContext context)
        {
            return IsFileExcludedFromAnalysis(context.GetStyleCopSettings(context.CancellationToken), context.Tree);
        }

        internal static bool IsFileExcludedFromAnalysis(SyntaxTreeAnalysisContext context, StyleCopSettings settings)
        {
            return IsFileExcludedFromAnalysis(settings, context.Tree);
        }

        internal static bool IsFileExcludedFromAnalysis(SyntaxNodeAnalysisContext context)
        {
            return IsFileExcludedFromAnalysis(context.Options.GetStyleCopSettings(context.CancellationToken), context.Node.SyntaxTree);
        }

        internal static bool IsFileExcludedFromAnalysis(SyntaxNodeAnalysisContext context, StyleCopSettings settings)
        {
            return IsFileExcludedFromAnalysis(settings, context.Node.SyntaxTree);
        }

        internal static bool IsFileExcludedFromAnalysis(SymbolAnalysisContext context)
        {
            if (!context.Symbol.Locations.Any())
            {
                return false;
            }

            return IsFileExcludedFromAnalysis(context.Options.GetStyleCopSettings(context.CancellationToken), context.Symbol.Locations[0].SourceTree);
        }

        private static bool IsFileExcludedFromAnalysis(StyleCopSettings settings, Microsoft.CodeAnalysis.SyntaxTree tree)
        {
            return (settings?.ExcludedFiles != null && settings.ExcludedFiles.Any(file => tree.FilePath.Equals(file, StringComparison.OrdinalIgnoreCase))) ||
                (settings?.ExcludedFileFilters != null && settings.ExcludedFileFilters.Any(fileFilter => Regex.IsMatch(tree.FilePath, fileFilter, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture)));
        }
    }
}
