using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Text;

namespace Benchmarks.Generator;

[Generator]
public class MetricsGenerator : ISourceGenerator
{
    public enum LogDestination { Debug, Console, Prometheus, File, None }
    public enum InterceptionMode { ExecutionTime, CallCount, MetadataLog, None }
    private struct MetricData
    {
        public MetricData(params string[] args) : this(args[0], args[1], args[2]) { }
        public MetricData(string typename, string propertyName, string description)
        {
            Typename = typename;
            Description = description;
            PropertyName = propertyName;
        }
        public string Typename { get; }
        public string Description { get; }
        public string PropertyName { get; }
    }
    private string EmitProps(MetricData[] metrics)
    => metrics.Select(metric => @$"[Description(""{metric.Description}"")] public static {metric.Typename} {metric.PropertyName} {{ get; set; }}")
                .Aggregate((a, b) => $"{a}\n\t{b}");
    private string PartialMetrics(MetricData[] metrics) => @$"
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin;
public static partial class Metrics
{{
{EmitProps(metrics)}
}}
";
    public void Execute(GeneratorExecutionContext context)
    {
        var allFunctionsInCompilationUnit = GetMarkedFunctionBy(context.Compilation);
        var allAttributesToBeEmited = GetTargetAttributesFrom(allFunctionsInCompilationUnit, context.Compilation);

        var classToEmitSourceCode = PartialMetrics(allAttributesToBeEmited);
        context.AddSource("Metrics.g.cs", SourceText.From(classToEmitSourceCode, Encoding.UTF8));

    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }

    // Note(Ayman) :    looks for this Pattern @ [Metrics(TypeName , PropertyName , Description)]
    //                  looks for this Pattern @ [Monitor(InterceptionMode.interceptionMode, LogDestination.logDestination)]
    private MethodDeclarationSyntax[] GetMarkedFunctionBy(Compilation context)
    {
        IEnumerable<SyntaxNode> allNodes = context.SyntaxTrees.SelectMany(s => s.GetRoot().DescendantNodes());
        return allNodes
            .Where(d => d.IsKind(SyntaxKind.MethodDeclaration))
            .OfType<MethodDeclarationSyntax>()
            .ToArray();
    }

    private MetricData[] GetTargetAttributesFrom(IEnumerable<MethodDeclarationSyntax> allFunctions, Compilation context)
    {
        return allFunctions.SelectMany(funcDef =>
        {
            var semanticModel = context.GetSemanticModel(funcDef.SyntaxTree);
            return funcDef.AttributeLists
                .SelectMany(x => x.Attributes)
                .Where(attr =>
                {
                    var attrName = attr.Name.ToString();
                    return attrName == "Metrics" || attrName == "Monitor";
                })
                .Select(attr =>
                {
                    bool isMonitor = attr.Name.ToString() == "Monitor";
                    var args = attr.ArgumentList.Arguments
                                    .Select(arg =>
                                    {
                                        return (arg.NameColon?.Name.ToString(), semanticModel.GetConstantValue(arg.Expression).ToString());
                                    })
                                    .ToArray();
                    if (isMonitor)
                    {
                        var logDest = LogDestination.None;
                        var logMode = InterceptionMode.None;
                        foreach (var argNode in args)
                        {
                            if (argNode.Item1 is null)
                            {
                                throw new FormatException("Arguments of Attribute :Monitor must all be named arguments");
                            }
                            var argType = this.GetType().Assembly
                                                    .GetTypes()
                                                    .Where(tp => tp.Name == argNode.Item1)
                                                    .FirstOrDefault();
                            if (argType == null) continue;

                            var argValue = Enum.Parse(argType, argNode.Item2);
                            if (argType == typeof(LogDestination))
                            {
                                logDest = (LogDestination)argValue;
                            }
                            else if (argType == typeof(InterceptionMode))
                            {
                                logMode = (InterceptionMode)argValue;
                            }

                        }
                        var targtArg = funcDef.Identifier.ToString();
                        if (logMode is InterceptionMode.ExecutionTime or InterceptionMode.CallCount
                            && logDest is LogDestination.Prometheus)
                        {
                            var prop = $"{targtArg}{logMode}";
                            var desc = $"{targtArg} {logMode} Metrics";
                            return new string[] { $"long", prop, desc };

                        }
                    }
                    return Array.Empty<string>();
                })
                .Where(x => x.Length > 0)
                .ToArray();
        })
        .Select(args =>
        {
            return new MetricData(args);
        })
        .ToArray();
    }
}
