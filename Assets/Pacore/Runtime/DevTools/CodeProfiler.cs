using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace PashaBibko.Pacore.DevTools
{
    public static class CodeProfiler
    {
        private static Dictionary<string, List<long>> ProfilerSnippets { get; } = new();
        public static IReadOnlyDictionary<string, List<long>> GetProfilerSnippets() => ProfilerSnippets;
        
        private static void AddProfileSnippet(string name, long ms)
        {
            if (!ProfilerSnippets.ContainsKey(name))
            {
                ProfilerSnippets.Add(name, new List<long>());
            }
            
            ProfilerSnippets[name].Add(ms);
        }

        public static ProfilerSnippetHandle Start(string name) => new(name);

        public class ProfilerSnippetHandle : IDisposable
        {
            private Stopwatch Stopwatch { get; }
            private string SnippetName { get; }
            
            public ProfilerSnippetHandle(string name)
            {
                Stopwatch = Stopwatch.StartNew();
                SnippetName = name;
            }

            public void Dispose()
            {
                Stopwatch.Stop();
                AddProfileSnippet(SnippetName, Stopwatch.ElapsedMilliseconds);
            }
        }
    }
}