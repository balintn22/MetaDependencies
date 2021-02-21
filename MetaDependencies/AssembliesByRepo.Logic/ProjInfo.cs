namespace AssembliesByRepo.Logic
{
    /// <summary>
    /// Represents information about a located assembly.
    /// </summary>
    public class ProjInfo
    {
        public enum AssTypes
        {
            Exe,
            Dll,
        }

        public AssTypes AssType { get; set; }

        /// <summary>The name of the assembly.</summary>
        public string AssName { get; set; }

        /// <summary>Contains the path of the .csproj file, relative to the start directory.</summary>
        public string CsProjPath { get; set; }

        /// <summary>True if a nuget package is generated from this project.</summary>
        public bool GeneratePackage { get; set; }

        /// <summary>Name (publication id) of the nuget package.</summary>
        public string PackageId { get; set; }

        /// <summary>
        /// URL of the code repository, as spevified in nuget package properties.
        /// Populated only for projects, where a nuget package is generated.
        /// Contents is provided by developers, not necessarily accurate, or may even be empty.
        /// </summary>
        public string RepositoryUrl { get; set; }
    }
}
