using System.Collections.Generic;

using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;


namespace TileCrusher.Scoring
{
    [XmlRoot("dictionary")]
    public class SerializableDictionary<DKey, DValue>
        : Dictionary<DKey, DValue>, IXmlSerializable
    {
        const string ItemTag = "item";
        const string KeyTag = "key";
        const string ValueTag = "value";

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            if (IsEmpty(reader))
            {
                return;
            }

            XmlSerializer keySerializer = new XmlSerializer(typeof(DKey));
            XmlSerializer valueSerializer
                = new XmlSerializer(typeof(DValue));

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                if (reader.NodeType == XmlNodeType.None)
                {
                    return;
                }
                ReadItem(reader, keySerializer, valueSerializer);
                reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        private bool IsEmpty(XmlReader reader)
        {
            bool isEmpty = reader.IsEmptyElement;
            reader.Read();
            return isEmpty;
        }

        private void ReadItem(XmlReader reader,
                              XmlSerializer keySerializer,
                              XmlSerializer valueSerializer)
        {
            reader.ReadStartElement(ItemTag);
            this.Add(ReadKey(reader, keySerializer),
                     ReadValue(reader, valueSerializer));
            reader.ReadEndElement();
        }

        private DKey ReadKey(XmlReader reader, XmlSerializer keySerializer)
        {
            reader.ReadStartElement(KeyTag);
            DKey key = (DKey)keySerializer.Deserialize(reader);
            reader.ReadEndElement();
            return key;
        }

        private DValue ReadValue(XmlReader reader,
                                 XmlSerializer valueSerializer)
        {
            reader.ReadStartElement(ValueTag);
            DValue value = (DValue)valueSerializer.Deserialize(reader);
            reader.ReadEndElement();
            return value;
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(DKey));
            XmlSerializer valueSerializer
                = new XmlSerializer(typeof(DValue));

            foreach (DKey key in this.Keys)
            {
                WriteItem(writer, keySerializer, valueSerializer, key);
            }
        }

        private void WriteItem(XmlWriter writer,
                               XmlSerializer keySerializer,
                               XmlSerializer valueSerializer,
                               DKey key)
        {
            writer.WriteStartElement(ItemTag);
            WriteKey(writer, keySerializer, key);
            WriteValue(writer, valueSerializer, key);
            writer.WriteEndElement();
        }

        private void WriteKey(XmlWriter writer,
                              XmlSerializer keySerializer,
                              DKey key)
        {
            writer.WriteStartElement(KeyTag);
            keySerializer.Serialize(writer, key);
            writer.WriteEndElement();
        }

        private void WriteValue(XmlWriter writer,
                                XmlSerializer valueSerializer,
                                DKey key)
        {
            writer.WriteStartElement(ValueTag);
            valueSerializer.Serialize(writer, this[key]);
            writer.WriteEndElement();
        }
    }
}
