using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace StudentApp.DAL.Providers
{
    public class XmlDataProvider<T> : IDataProvider<T> where T : class
    {
        public IEnumerable<T> Read(string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                if (fs.Length == 0)
                {
                    return new List<T>();
                }
                
                var data = (List<T>)serializer.Deserialize(fs);
                return data ?? new List<T>();
            }
        }

        public void Write(IEnumerable<T> data, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<T>));

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, data.ToList());
            }
        }
    }
}