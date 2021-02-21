using AssembliesByRepo.Logic;
using System;

namespace AssembliesByRepo
{
    class Program
    {
        static void Main(string[] args)
        {
            ICrawler crawler = new Crawler();
            foreach (ProjInfo assInfo in crawler.Crawl(Environment.CurrentDirectory))
            {
                string relativePath = PathHelper.GetRelativePath(Environment.CurrentDirectory, assInfo.CsProjPath);
                Console.WriteLine($"{assInfo.AssName}\t{assInfo.AssType}\t{relativePath}");
            }
        }
    }
}
