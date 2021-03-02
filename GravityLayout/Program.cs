using Dgml;
using GravityLayout.Logic;
using GravityLayout.Logic.Physics;
using System;
using System.Linq;
using System.Reflection;

namespace GravityLayout
{
    class Program
    {
        private const int ITERATIONS_DEFAULT = 100;

        private static IDgmlRepo _dgmlRepo;
        private static string _inputPath;
        private static string _outputPath;
        private static int _iterations;


        static void Main(string[] args)
        {
            var helpArgs = new string[] { "help", "-help", "/help" };

            if (args.Any(x => helpArgs.Contains(x.ToLower())))
            {
                ShowUsage();
                return;
            }

            _dgmlRepo = new DgmlRepo();

            ProcessArgs(args);

            if (_inputPath == null)
            {
                Console.WriteLine("Input .dgml file must be specified.");
                ShowUsage();
                return;
            }

            DirectedGraph graph = _dgmlRepo.Load(_inputPath);
            var layouter = new GravityLayouter(100, 1, Rope.Characteristics.Linear, 1);
            DirectedGraph result = layouter.Layout(graph, 1, 100, PrintIterationInfo);
            _dgmlRepo.Save(result, _outputPath);
        }

        private static void PrintIterationInfo(IterationResult iResult)
        {
            var bounds = iResult.Graph.GetBoundingRect();
            Console.WriteLine($"{iResult.Count}.\tMaxShift: {iResult.MaxShift}\tBounds: ({bounds?.Width} x {bounds?.Height})");
        }

        private static void ShowUsage()
        {
            string appName = Assembly.GetExecutingAssembly().GetName().Name;

            Console.WriteLine(
$"{appName} Having a DGML graph, arranges its nodes using the anti-gravity " +
$"method, where nodes have mass, proportional to their size(bounds)," +
$"that repels them from one another.Links are considered ropes," +
$"pulling nodes together.\n" +
$"If the graph has already been laid out starts from those node positions. If not, " +
$"starts from a random layout." +
$"\n" +
$"Usage:\n" +
$"  {appName} -in=\"input.dgml\" [-out=\"output.dgml\"] [-iterations=4]\n" +
$"\n" +
$" where\n" +
$"  -in  specifies source .dgml file to be laid out.\n" +
$"  -out specifies the name of the output .dgml file. Defaults to overwrite the input.\n" +
$"  -iteration specifies the number of times forces are calculated and shifts applied. Defaults to {ITERATIONS_DEFAULT}."
            );
        }

        private static void ProcessArgs(string[] args)
        {
            var argBag = args.Select(a => a.Split('='));
            _outputPath = null;
            _iterations = ITERATIONS_DEFAULT;

            foreach (var argKeyValue in argBag)
            {
                switch (argKeyValue[0].ToLower())
                {
                    case "-in": _inputPath = argKeyValue[1]; break;
                    case "-out": _outputPath = argKeyValue[1]; break;
                    case "-iterations": _iterations = int.Parse(argKeyValue[1]); break;
                }
            }

            _outputPath = _outputPath ?? _inputPath;
        }
    }
}
