using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace AssembliesByRepo.Logic.Space
{
    /// <summary>
    /// Helper class for old style csproj files.
    /// </summary>
    public partial class OldStyleCsProj
    {
        public static ProjInfo AssInfoFrom(string csProjPath)
        {
            using (var fileStream = File.Open(csProjPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(OldStyleCsProj.Project));
                var parsed = (OldStyleCsProj.Project)serializer.Deserialize(fileStream);
                ProjectPropertyGroup mainPropertyGroup = parsed.Items
                    .Where(item => item.GetType() == typeof(OldStyleCsProj.ProjectPropertyGroup))
                    ?.Select(item => item as OldStyleCsProj.ProjectPropertyGroup)
                    .FirstOrDefault(propertyGroup => !string.IsNullOrWhiteSpace(propertyGroup.AssemblyName));

                return new ProjInfo
                {
                    AssType = ResolveOutputType(mainPropertyGroup.OutputType),
                    AssName = mainPropertyGroup.AssemblyName,
                    CsProjPath = csProjPath,
                };
            }
        }

        private static ProjInfo.AssTypes ResolveOutputType(string outputType)
        {
            switch (outputType)
            {
                case "Exe": return ProjInfo.AssTypes.Exe;
                case "Dll": return ProjInfo.AssTypes.Dll;
                default: throw new ArgumentException($"Unsupported argument {outputType}");
            }
        }
    }
}
