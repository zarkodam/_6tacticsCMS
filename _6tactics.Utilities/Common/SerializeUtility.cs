using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace _6tactics.Utilities.Common
{
    public static class SerializeUtility
    {
        public static string Serialize (object obj)
		{
			var stringBuilder = new StringBuilder ();
            using (var writer = new StringWriter(stringBuilder))
                new XmlSerializer(obj.GetType()).Serialize(writer, obj);

			return stringBuilder.ToString ();
		}

		public static T Deserialize<T> (string xml) where T: class
		{
			using (var reader = new StringReader(xml))
				return new XmlSerializer (typeof(T)).Deserialize (reader) as T;
		}
    }
}

