using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Fody.StandAlone
{
    public class WeavingParameters
    {
        public WeavingParameters(IEnumerable<WeaverEntry> weaverEntries, string references)
        {
            WeaverEntries = weaverEntries;
            References = references;
        }

        public IEnumerable<WeaverEntry> WeaverEntries { get; private set; }
        public string References { get; private set; }

        public static WeavingParameters GetWeavingParameters(Stream fodyXmlConfiguration, string weaverFolder, string references)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(fodyXmlConfiguration);

            var weaverEntries = new List<WeaverEntry>();

            foreach (XmlNode documentElementChildNode in xmlDocument.DocumentElement.ChildNodes)
            {
                weaverEntries.Add(new WeaverEntry()
                {
                    AssemblyName = documentElementChildNode.Name,
                    Element = documentElementChildNode.OuterXml,
                    TypeName = "ModuleWeaver",
                    AssemblyPath = Path.Combine(weaverFolder, $"{documentElementChildNode.Name}.Fody.dll")
                });
            }

            var weavingParameters = new WeavingParameters(weaverEntries, references);
            return weavingParameters;
        }
    }
}