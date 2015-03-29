﻿namespace StyleCop.Analyzers.LayoutRules
{
    using System;
    using System.Collections.Immutable;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Diagnostics;
    using Microsoft.CodeAnalysis.Text;
    using System.Collections.Generic;


    /// <summary>
    /// The C# code contains multiple blank lines in a row.
    /// </summary>
    /// <remarks>
    /// <para>To improve the readability of the code, StyleCop requires blank lines in certain situations, and prohibits
    /// blank lines in other situations. This results in a consistent visual pattern across the code, which can improve
    /// recognition and readability of unfamiliar code.</para>
    ///
    /// <para>A violation of this rule occurs when the code contains more than one blank line in a row. For
    /// example:</para>
    ///
    /// <code language="csharp">
    /// public bool Enabled
    /// {
    ///     get 
    ///     { 
    ///         Console.WriteLine("Getting the enabled flag.");
    /// 
    /// 
    ///         return this.enabled; 
    ///     }
    /// }
    /// </code>
    ///
    /// <para>The code above would generate an instance of this violation, since it contains blank multiple lines in a
    /// row.</para>
    /// </remarks>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SA1507CodeMustNotContainMultipleBlankLinesInARow : DiagnosticAnalyzer
    {
        /// <summary>
        /// The ID for diagnostics produced by the <see cref="SA1507CodeMustNotContainMultipleBlankLinesInARow"/>
        /// analyzer.
        /// </summary>
        public const string DiagnosticId = "SA1507";
        private const string Title = "Code must not contain multiple blank lines in a row";
        private const string MessageFormat = "Code must not contain multiple blank lines in a row";
        private const string Category = "StyleCop.CSharp.LayoutRules";
        private const string Description = "The C# code contains multiple blank lines in a row.";
        private const string HelpLink = "http://www.stylecop.com/docs/SA1507.html";

        private static readonly DiagnosticDescriptor Descriptor =
            new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, true, Description, HelpLink);

        private static readonly ImmutableArray<DiagnosticDescriptor> SupportedDiagnosticsValue =
            ImmutableArray.Create(Descriptor);

        /// <inheritdoc/>
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        {
            get
            {
                return SupportedDiagnosticsValue;
            }
        }

        /// <inheritdoc/>
        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxTreeAction(this.HandleSyntaxTreeAnalysis);
        }

        private void HandleSyntaxTreeAnalysis(SyntaxTreeAnalysisContext context)
        {
            SyntaxNode root = context.Tree.GetRoot(context.CancellationToken);
            foreach (var token in root.DescendantTokens(descendIntoTrivia: false))
            {
                if (token.IsKind(SyntaxKind.EndOfFileToken))
                {
                    // If the file ends with blanks lines, ignore them, they will be handled by SA1518.
                    continue;
                }

                int blankLineIndex = 0;
                int blankLineEndIndex = -1;
                int blankLineCount = 0;
                SyntaxTriviaList leadingTrivia = token.LeadingTrivia;
                for (int i = 0; i < leadingTrivia.Count; i++)
                {
                    switch (leadingTrivia[i].Kind())
                    {
                        case SyntaxKind.WhitespaceTrivia:
                            break;

                        case SyntaxKind.EndOfLineTrivia:
                            blankLineEndIndex = i;
                            blankLineCount++;
                            break;

                        default:
                            ReportDiagnosticIfNecessary(context, leadingTrivia, blankLineIndex, blankLineEndIndex, blankLineCount);
                            blankLineIndex = i + 1;
                            blankLineCount = 0;
                            break;
                    }
                }

                ReportDiagnosticIfNecessary(context, leadingTrivia, blankLineIndex, blankLineEndIndex, blankLineCount);
            }
        }

        private static void ReportDiagnosticIfNecessary(SyntaxTreeAnalysisContext context, SyntaxTriviaList leadingTrivia, int blankLineIndex, int blankLineEndIndex, int blankLineCount)
        {
            if (blankLineIndex < 0 || blankLineEndIndex <= blankLineIndex)
            {
                // nothing to report
                return;
            }

            if (blankLineCount < 2)
            {
                // only care about multiple blank lines in a row
                return;
            }

            if (leadingTrivia[blankLineIndex].SpanStart == 0)
            {
                // blank lines at the beginning are reported by SA1517
                return;
            }

            TextSpan span = TextSpan.FromBounds(leadingTrivia[blankLineIndex].SpanStart, leadingTrivia[blankLineEndIndex].Span.End);
            Location location = Location.Create(context.Tree, span);
            context.ReportDiagnostic(Diagnostic.Create(Descriptor, location));
        }
    }
}
