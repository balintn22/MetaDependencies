using System.IO;
using System.Xml.Serialization;

namespace AssInfo.Logic.Space
{
    // Code in this partial implementation is maintained manually.

    /// <summary>
    /// Helper class for old style csproj files.
    /// </summary>
    public partial class SdkStyleCsProj
    {
        public static ProjInfo ProjInfoFrom(string csProjPath)
        {
            using (var fileStream = File.Open(csProjPath, FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SdkStyleCsProj.Project));
                var parsed = (SdkStyleCsProj.Project)serializer.Deserialize(fileStream);
                var ppg = parsed.PropertyGroup;

                string assName = string.IsNullOrWhiteSpace(ppg.AssemblyName)
                    ? Path.GetFileName(csProjPath)?.Replace(".csproj", "")
                    : ppg.AssemblyName;

                return new ProjInfo
                {
                    AssType = ProjInfo.AssTypes.Dll,
                    AssName = assName,
                    CsProjPath = csProjPath,
                    Description = ppg.Description,
                    GeneratePackage = ppg.GeneratePackageOnBuild || ppg.PublishPackage,
                    PackageId = ppg.GeneratePackageOnBuild || ppg.PublishPackage
                        ? string.IsNullOrWhiteSpace(ppg.PackageId)
                            ? assName : ppg.PackageId
                        : null,
                    RepositoryUrl = ppg.RepositoryUrl,
                    TargetFrameworks = ppg.TargetFramework ?? ppg.TargetFrameworks,
                };
            }
        }
    }
}
