//
//  SPDX-FileName: GenerateSignature.cs
//  SPDX-FileCopyrightText: Copyright (c) Jarl Gullberg
//  SPDX-License-Identifier: AGPL-3.0-or-later
//

using System;
using System.IO;
using System.Linq;
using Puzzle.Tests.Data;
using Puzzle.Tests.Extensions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using Xunit;

#pragma warning disable CS1591, SA1600

namespace Puzzle.Tests.Tests.SignatureGeneratorTests;

public partial class SignatureGeneratorTests
{
    public class GenerateSignature
    {
        private readonly SignatureGenerator _generator;

        public GenerateSignature()
        {
            _generator = new SignatureGenerator();
        }

        [Fact]
        public void GeneratedSignatureMatchesExpectedAlgorithmResults()
        {
            var expectedSignature = SampleData.MonaLisaSignature.Value;
            var actualSignature = _generator.GenerateSignature(SampleImages.MonaLisa.Value);

            if (!SampleImages.MonaLisa.Value.DangerousTryGetSinglePixelMemory(out var pixels))
            {
                throw new InvalidOperationException("The backing buffer for the image was not contiguous.");
            }

            File.WriteAllBytes(@"E:\TempStuff\Puzzle\Puzzle\mona.raw", pixels.ToArray().Select(e => e.PackedValue).ToArray());

            AssertExtensions.SequenceEqual(expectedSignature.Span, actualSignature);
        }

        [Theory]
        [MemberData(nameof(SampleImages.SmallImages), MemberType = typeof(SampleImages))]
        public void CanGenerateSignatureForSmallImages(Image<L8> smallImage)
        {
            _ = _generator.GenerateSignature(smallImage);
        }

        [Fact]
        public void CanGenerateSignatureForLargeImage()
        {
            _ = _generator.GenerateSignature(SampleImages.Uniform8192.Value);
        }
    }
}
