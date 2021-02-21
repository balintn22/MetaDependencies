using AssembliesByRepo.Logic.Space;
using System;

namespace AssembliesByRepo.Logic
{
    /// <summary>
    /// Helps to get information from .csproj files.
    /// </summary>
    public static class CsProjHelper
    {
        public static ProjInfo GetAssInfoFrom(string csProjPath)
        {
            try { return OldStyleCsProj.AssInfoFrom(csProjPath); }
            catch (Exception) { }

            try { return SdkStyleCsProj.AssInfoFrom(csProjPath); }
            catch (Exception) { }

            throw new Exception("Uknown csproj file format or failed to parse csproj file.");
        }
    }
}
