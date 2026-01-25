using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace PashaBibko.Pacore.Editor.Caches
{
    public static class MonoScriptCache
    {
        private static Dictionary<string, MonoScript> Cache { get; } = new();

        static MonoScriptCache()
        {
            /* Finds all MonoScripts and adds them to the dictionary by name */
            MonoScript[] scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            foreach (MonoScript script in scripts)
            {
                Type scriptType = script.GetClass();
                if (scriptType is not null)
                {
                    string name = scriptType.FullName;
                    Cache.TryAdd(name, script);
                }
            }
        }

        public static MonoScript Get(string name)
        {
            /* Fetches the value (if there is one) without creating a default val */
            Cache.TryGetValue(name, out MonoScript script);
            return script;
        }
    }
}