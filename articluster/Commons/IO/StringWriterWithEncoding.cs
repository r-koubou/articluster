using System.IO;
using System.Text;

namespace ArtiCluster.Commons.IO;

public sealed class StringWriterWithEncoding( Encoding encoding ) : StringWriter
{
    public StringWriterWithEncoding()
        : this( Encoding.Unicode ) {}

    public override Encoding Encoding { get; } = encoding;
}
