using System.Diagnostics.CodeAnalysis;

namespace Bravellian;

public interface IBvParsable<TSelf>
    where TSelf : IBvParsable<TSelf>?
{
    static abstract TSelf Parse(string value);

    static abstract bool TryParse([NotNullWhen(true)] string? value, [MaybeNullWhen(false)] out TSelf result);
}
