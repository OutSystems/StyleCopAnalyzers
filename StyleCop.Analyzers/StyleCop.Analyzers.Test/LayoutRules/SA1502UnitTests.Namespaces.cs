﻿namespace StyleCop.Analyzers.Test.LayoutRules
{
    using System.Threading;
    using System.Threading.Tasks;

    using StyleCop.Analyzers.LayoutRules;
    using TestHelper;
    using Xunit;

    /// <summary>
    /// Unit tests for the namespaces part of <see cref="SA1502ElementMustNotBeOnASingleLine"/>.
    /// </summary>
    public partial class SA1502UnitTests : CodeFixVerifier
    {
        /// <summary>
        /// Verifies that a correct empty namespace will pass without diagnostic.
        /// </summary>
        [Fact]
        public async Task TestValidEmptyNamespace()
        {
            var testCode = @"namespace Foo 
{
}";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }

        /// <summary>
        /// Verifies that an empty namespace defined on a single line will trigger a diagnostic.
        /// </summary>
        [Fact]
        public async Task TestEmptyNamespaceOnSingleLine()
        {
            var testCode = @"namespace Foo { }";

            var expected = this.CSharpDiagnostic().WithLocation(1, 15);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);
        }

        /// <summary>
        /// Verifies that a namespace defined on a single line will trigger a diagnostic.
        /// </summary>
        [Fact]
        public async Task TestNamespaceOnSingleLine()
        {
            var testCode = @"namespace Foo { using System; }";

            var expected = this.CSharpDiagnostic().WithLocation(1, 15);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);
        }

        /// <summary>
        /// Verifies that a namespace with its block defined on a single line will trigger a diagnostic.
        /// </summary>
        [Fact]
        public async Task TestNamespaceWithBlockOnSingleLine()
        {
            var testCode = @"namespace Foo 
{ using System; }";

            var expected = this.CSharpDiagnostic().WithLocation(2, 1);
            await this.VerifyCSharpDiagnosticAsync(testCode, expected, CancellationToken.None);
        }

        /// <summary>
        /// Verifies that a namespace with its block defined on a mutiple lines will pass without diagnostic.
        /// </summary>
        [Fact]
        public async Task TestNamespaceWithBlockStartkOnSameLine()
        {
            var testCode = @"namespace Foo {
    using System;
}";

            await this.VerifyCSharpDiagnosticAsync(testCode, EmptyDiagnosticResults, CancellationToken.None);
        }
    }
}
