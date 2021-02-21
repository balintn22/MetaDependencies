using AssembliesByRepo.Logic;
using System;

namespace AssembliesByRepo
{
    class Program
    {
        static void Main(string[] args)
        {
            ICrawler crawler = new Crawler();
            Console.WriteLine(
                $"AssemblyName\tType\tPath" +
                $"\tIsPackage\tPackageId\tRepositoryUrl" +
                $"\tDescription\tTargetFrameworks");

            foreach (ProjInfo pi in crawler.Crawl(Environment.CurrentDirectory))
            {
                string relativePath = PathHelper.GetRelativePath(Environment.CurrentDirectory, pi.CsProjPath);
                Console.WriteLine(
                    $"{pi.AssName}\t{pi.AssType}\t{relativePath}" +
                    $"\t{pi.GeneratePackage}\t{pi.PackageId}\t{pi.RepositoryUrl}" +
                    $"\t{pi.Description}\t{pi.TargetFrameworks}");
            }
        }
    }
}
