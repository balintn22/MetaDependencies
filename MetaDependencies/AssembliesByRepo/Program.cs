using AssembliesByRepo.Logic;
using System;
using System.Linq;
using System.Reflection;

namespace AssembliesByRepo
{
    class Program
    {
        static void Main(string[] args)
        {
            var helpArgs = new string[] { "help", "-help", "/help" };

            if (args.Any(x => helpArgs.Contains(x.ToLower())))
            {
                ShowUsage();
                return;
            }

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

        private static void ShowUsage()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;

            Console.WriteLine(
                $"{appName} crawls all subdirectories and find .csproj files. " +
                "It'll then output information about those assemblies, including " +
                "build output type (exe/dll), nuget packaging properties, " +
                "assembly description and target frameworks, etc.\n" +
                "it supports old style (VS2017 and before) and SDK style .csproj files.\n" +
                "\n" +
                "Usage:\n" +
                $"   Add {appName} to yuor path, or copy {appName} and {appName}.Logic to the folder where you start your search.\n" +
                "   To display assembly info on screen:\n" +
                $"      {appName}\n" +
                "   To write assembly info to a text file:\n" +
                $"      {appName} > out.txt\n"
            );
        }
    }
}
