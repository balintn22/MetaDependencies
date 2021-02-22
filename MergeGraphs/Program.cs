using MergeGraphs.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace MergeGraphs
{
    class Program
    {
        private static IDgmlRepo _dgmlRepo;
        private static IMerger _merger;
        private static string _inputFolderPath;

        static void Main(string[] args)
        {
            var helpArgs = new string[] { "help", "-help", "/help" };

            if (args.Any(x => helpArgs.Contains(x.ToLower())))
            {
                ShowUsage();
                return;
            }

            _dgmlRepo = new DgmlRepo();
            _merger = new Merger();

            ProcessArgs(args);
            string[] dgmlsFilePaths = Directory.GetFiles(_inputFolderPath, "*.dgml");
            List<Dgml.DirectedGraph> graphs = dgmlsFilePaths.Select(f => _dgmlRepo.Load(f)).ToList();
            Dgml.DirectedGraph merged = _merger.Merge(graphs);
            _dgmlRepo.Save(merged, "Merged.dgml");
        }

        private static void ShowUsage()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;

            Console.WriteLine(
                $"TODO"
            );
        }

        private static void ProcessArgs(string[] args)
        {
            var argBag = args.Select(a => a.Split('='));

            foreach (var argKeyValue in argBag)
            {
                switch (argKeyValue[0].ToLower())
                {
                    case "-indir": _inputFolderPath = argKeyValue[1]; break;
                }
            }
        }
    }
}
