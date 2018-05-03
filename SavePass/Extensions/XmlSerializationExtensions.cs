namespace SavePass.Extensions
{
    public static class XmlSerializationExtensions
    {
        public static T FromXml<T>(this string xml) where T : class
            => XmlSerialization.DeserializeObject<T>(xml);

        public static string ToXml<T>(this T obj) where T : class
            => XmlSerialization.SerializeObject(obj);
    }
}
