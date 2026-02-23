//
//  SPDX-FileName: EnumerableExtensions.cs
//  SPDX-FileCopyrightText: Copyright (c) Jarl Gullberg
//  SPDX-License-Identifier: AGPL-3.0-or-later
//

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Puzzle.Extensions;

/// <summary>
/// Holds extension methods for the IEnumerable interface.
/// </summary>
internal static class EnumerableExtensions
{
    /// <summary>
    /// Calculates the median of a set of values. Mutates the existing span.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <returns>The median value.</returns>
    [Pure]
    internal static double Median(this Span<double> source)
    {
        source.Sort();

        if (source.Length == 1)
        {
            return source[0];
        }

        var halfwayIndex = source.Length / 2;

        if (source.Length % 2 == 0)
        {
            return source[halfwayIndex];
        }

        return (source[halfwayIndex] + source[halfwayIndex - 1]) / 2.0;
    }
}
