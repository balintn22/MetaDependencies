using System.IO;
using System.Xml.Serialization;

namespace Dgml
{
    public interface IDgmlRepo
    {
        Dgml.DirectedGraph Load(string dgmlFilePath);

        /// <summary>Saves a graph as .dgml, overwriting the target file if it exists.</summary>
        void Save(Dgml.DirectedGraph graph, string dgmlFilePath);
    }

    public class DgmlRepo : IDgmlRepo
    {
        public Dgml.DirectedGraph Load(string dgmlFilePath)
        {
            using (var fileStream = File.Open(dgmlFilePath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Dgml.DirectedGraph));
                return (Dgml.DirectedGraph)serializer.Deserialize(fileStream);
            }
        }

        public void Save(Dgml.DirectedGraph graph, string dgmlFilePath)
        {
            if (File.Exists(dgmlFilePath))
                File.Delete(dgmlFilePath);

            using (var fileStream = File.Open(dgmlFilePath, FileMode.CreateNew))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Dgml.DirectedGraph));
                serializer.Serialize(fileStream, graph);
            }
        }
    }
}
