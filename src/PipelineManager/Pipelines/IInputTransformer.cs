using System.IO;

namespace Pipelines
{
    public interface IInputTransformer<out T>
    {
        T Transform(Stream data);
    }
}