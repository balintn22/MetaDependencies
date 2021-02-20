using System.Collections.Generic;
using System.IO;

namespace AssembliesByRepo.Logic
{
    public interface ICrawler
    {
        IEnumerable<AssInfo> Crawl(string path);
    }

    public class Crawler : ICrawler
    {
        public IEnumerable<AssInfo> Crawl(string path)
        {
            // Enumerate csprojs in the current folder
            string[] csProjs = Directory.GetFiles(path, "*.csproj");
            foreach (string csProj in csProjs)
            {
                AssInfo assInfo;
                try { assInfo = CsProjHelper.GetAssInfoFrom(csProj); }
                catch { assInfo = null; }

                if(assInfo != null)
                    yield return assInfo;
            }

            // Recurse into subfolders
            string[] subDirs = Directory.GetDirectories(path);
            foreach (string subDir in subDirs)
            {
                foreach(AssInfo assInfo in Crawl(Path.Combine(path, subDir)))
                    yield return assInfo;
            }
        }
    }
}
