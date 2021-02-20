namespace AssembliesByRepo.Logic
{
    /// <summary>
    /// Represents information about a located assembly.
    /// </summary>
    public class AssInfo
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
    }
}
