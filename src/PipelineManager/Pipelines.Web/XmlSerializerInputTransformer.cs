using System;
using System.IO;
using System.Xml.Serialization;

namespace Pipelines.Web
{
    public class XmlSerializerInputTransformer<T> : IInputTransformer<T>
    {
        public T Transform(Stream data)
        {
            return (T)new XmlSerializer(typeof (T)).Deserialize(data);
        }
    }
}