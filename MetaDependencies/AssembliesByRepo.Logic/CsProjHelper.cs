using AssInfo.Logic.Space;
using System;

namespace AssInfo.Logic
{
    /// <summary>
    /// Helps to get information from .csproj files.
    /// </summary>
    public static class CsProjHelper
    {
        public static ProjInfo GetAssInfoFrom(string csProjPath)
        {
            try { return OldStyleCsProj.ProjInfoFrom(csProjPath); }
            catch (Exception) { }

            try { return SdkStyleCsProj.ProjInfoFrom(csProjPath); }
            catch (Exception) { }

            throw new Exception("Uknown csproj file format or failed to parse csproj file.");
        }
    }
}
