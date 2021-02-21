using System.IO;
using System.Xml.Serialization;

namespace AssembliesByRepo.Logic.Space
{
    // Code in this partial implementation is maintained manually.

    /// <summary>
    /// Helper class for old style csproj files.
    /// </summary>
    public partial class SdkStyleCsProj
    {
        public static ProjInfo AssInfoFrom(string csProjPath)
        {
            using (var fileStream = File.Open(csProjPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SdkStyleCsProj.Project));
                var parsed = (SdkStyleCsProj.Project)serializer.Deserialize(fileStream);

                return new ProjInfo
                {
                    AssType = ProjInfo.AssTypes.Dll,
                    AssName = string.IsNullOrWhiteSpace(parsed.PropertyGroup.AssemblyName)
                        ? Path.GetFileName(csProjPath)
                        : parsed.PropertyGroup.AssemblyName,
                    CsProjPath = csProjPath,
                };
            }
        }
    }
}
