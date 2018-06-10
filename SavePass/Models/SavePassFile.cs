using System.Xml.Serialization;

namespace Zabrodin.SavePass.Models
{
    public class SavePassFile
    {
        public SavePassFile()
        {
        }

        public SavePassFile(SavePassItem[] items)
        {
            Items = items;
        }

        [XmlArray(nameof(Items))]
        [XmlArrayItem(nameof(SavePassItem), typeof(SavePassItem))]
        public SavePassItem[] Items { get; set; }
    }
}