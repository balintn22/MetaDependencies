using System;

namespace AssembliesByRepo.Logic
{
    public static class PathHelper
    {
        public static string GetRelativePath(string fromPath, string toPath)
        {
            string fromPathLow = fromPath.ToLower();
            string toPathLow = toPath.ToLower();

            int i;
            for (i = 0; i < fromPath.Length; i++)
            {
                if (i >= toPathLow.Length)
                    throw new Exception($"Can't get a relative path from {fromPath} to {toPath}.");

                if (fromPathLow[i] != toPathLow[i])
                    break;
            }

            if (toPath[i] == '\\' || toPath[i] == '/')
                i++;

            return toPath.Substring(i);
        }
    }
}
