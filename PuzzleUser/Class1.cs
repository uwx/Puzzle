using System.Runtime.InteropServices;
using System.Text;
using JetBrains.Annotations;
using Microsoft.CSharp.RuntimeBinder;
using Puzzle;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.PixelFormats;

namespace PuzzleUser;

[PublicAPI]
public class PuzzleUser
{
    public static void Main()
    {
    }
    
    public static LuminosityLevel[] LoadAndGenerateSignature0(
        ReadOnlySpan<byte> data,
        uint gridSize = 9,
        double noiseCutoff = 2.0,
        double sampleSizeRatio = 2.0,
        bool enableAutocrop = true
    )
    {
        using var image = Image.Load<L8>(data);
        return new SignatureGenerator(gridSize, noiseCutoff, sampleSizeRatio, enableAutocrop).GenerateSignature(image);
    }

#pragma warning disable CA1822
    public Task<object> LoadAndGenerateSignature1(
#pragma warning restore CA1822
        dynamic input
    )
    {
        byte[] data = input.data;
        var gridSize = GetPropOrDefault(() => input.gridSize, (uint)9);
        var noiseCutoff = GetPropOrDefault(() => input.noiseCutoff, 2.0);
        var sampleSizeRatio = GetPropOrDefault(() => input.sampleSizeRatio, 2.0);
        var enableAutocrop = GetPropOrDefault(() => input.enableAutocrop, true);
        return Task.FromResult<object>(LoadAndGenerateSignature0(data.AsSpan(), gridSize, noiseCutoff, sampleSizeRatio, enableAutocrop));
    }

    private static T GetPropOrDefault<T>(Func<dynamic> func, T def)
    {
        try
        {
            return func();
        }
        catch (RuntimeBinderException)
        {
            return def;
        }
    }

    public static LuminosityLevel[] LoadAndGenerateSignature2(
        Stream data,
        uint gridSize = 9,
        double noiseCutoff = 2.0,
        double sampleSizeRatio = 2.0,
        bool enableAutocrop = true
    )
    {
        using var image = Image.Load<L8>(data);
        return new SignatureGenerator(gridSize, noiseCutoff, sampleSizeRatio, enableAutocrop).GenerateSignature(image);
    }

    public static LuminosityLevel[] LoadAndGenerateSignature3(
        string fileName,
        uint gridSize = 9,
        double noiseCutoff = 2.0,
        double sampleSizeRatio = 2.0,
        bool enableAutocrop = true
    )
    {
        using var image = Image.Load<L8>(fileName);
        return new SignatureGenerator(gridSize, noiseCutoff, sampleSizeRatio, enableAutocrop).GenerateSignature(image);
    }

    [UnmanagedCallersOnly(EntryPoint = "load_and_generate_signature")]
    public static unsafe int LoadAndGenerateSignature4(
        byte* data,
        int dataLength,
        LuminosityLevel* output, // must be of length gridSize * gridSize * 8
        int gridSize,
        double noiseCutoff,
        double sampleSizeRatio,
        bool enableAutocrop,
        byte* exception,
        int exceptionLength
    )
    {
        try
        {
            using var image = Image.Load<L8>(new Span<byte>(data, dataLength));

            var outputSpan = new Span<LuminosityLevel>(output, (int)(gridSize * gridSize * 8));
            new SignatureGenerator((uint)gridSize, noiseCutoff, sampleSizeRatio, enableAutocrop).GenerateSignature(image, outputSpan);

            return 0;
        }
        catch (Exception ex)
        {
            var exText = new Span<byte>(exception, exceptionLength);
            Encoding.UTF8.TryGetBytes(ex.ToString(), exText, out var bytesWritten);
            return bytesWritten;
        }
    }
}