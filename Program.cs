﻿using System.Buffers;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<MyBenchmark>();
// /*
// TestCase = {
//     ValidCode: 0x60015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C0000,
//     InvalidCode: 0xF260015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C000060015C0000
// }
// */

// var benchy = new MyBenchmark();
// // call functions on my benchmark and write result
// // get all method in benchy using refletion
// benchy.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
//     .Where(meth => meth.CustomAttributes.Any(attr => attr.AttributeType == typeof(BenchmarkAttribute)))
//     .ToList()
//     .ForEach(method => {
//         Console.Write($"{method.Name} : ");
//         Console.WriteLine(method.Invoke(benchy, null));
//     });