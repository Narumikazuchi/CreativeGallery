namespace Narumikazuchi.CreativeGallery.Data;

public sealed class VolatileString : IDisposable
{
    static public VolatileString From(Span<Char> chars)
    {
        Span<Byte> bytes = MemoryMarshal.Cast<Char, Byte>(span: chars);
        Byte[] raw = new Byte[bytes.Length];
        bytes.CopyTo(destination: raw);
        s_Random.GetBytes(data: bytes);
        return new VolatileString(bytes: raw);
    }
    static public VolatileString From(Span<Byte> bytes)
    {
        Byte[] raw = new Byte[bytes.Length];
        bytes.CopyTo(destination: raw);
        s_Random.GetBytes(data: bytes);
        return new VolatileString(bytes: raw);
    }

    public void Dispose()
    {
        s_Random.GetBytes(data: m_Bytes);
        GC.SuppressFinalize(obj: this);
    }

    public new ReadOnlySpan<Char> ToString()
    {
        ReadOnlySpan<Char> chars = MemoryMarshal.Cast<Byte, Char>(span: m_Bytes);
        return chars;
    }

    public Byte[] RawBytes
    {
        get
        {
            return m_Bytes;
        }
    }

    static private readonly RandomNumberGenerator s_Random = RandomNumberGenerator.Create();

    private VolatileString(Byte[] bytes)
    {
        m_Bytes = bytes;
    }

    private readonly Byte[] m_Bytes;
}