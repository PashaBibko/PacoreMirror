using System.Collections.Generic;
using PashaBibko.Pacore.DevTools;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Pacore.Editor.EditorWindows
{
    public class ProfilerWindow : EditorWindow
    {
        private double LastRepaint = double.MinValue;
        private Vector2 ScrollPosition;
        
        [MenuItem("Pacore/Profiler")]
        public static void OpenWindow() => GetWindow<ProfilerWindow>();

        /* Makes sure the window is repainted often to show latest info */
        private void OnDisable() => EditorApplication.update -= CheckForRepaint;
        private void OnEnable() => EditorApplication.update += CheckForRepaint;

        private void CheckForRepaint()
        {
            /* Triggers a repaint when it has been over 1 second since last repaint */
            double now = EditorApplication.timeSinceStartup;
            if (now - LastRepaint > 1f)
            {
                LastRepaint = now;
                Repaint();
            }
        }

        private static void DrawEmptyProfiler()
        {
            GUILayout.BeginVertical(style: "box");
            
            GUILayout.Label(text: "No profiler snippets found", EditorStyles.boldLabel);
            GUILayout.Label(text: "Try running the game to collect profile samples");
            
            GUILayout.EndVertical();
        }

        private static void DrawProfilerSnippet(string name, List<long> times)
        {
            GUILayout.BeginVertical(style: "box");

            GUILayout.BeginHorizontal();
            GUILayout.Label(text: name, EditorStyles.boldLabel);
            GUILayout.EndHorizontal();

            float averageMs = times.Sum() / (float)times.Count;
            GUILayout.Label("Average time: " + averageMs);

            IGrouping<long, long>[] ordered = times // List<long>
                .GroupBy(time => time) // IEnumerable<IGrouping<long, long>>
                .OrderByDescending(time => time.Key) // IOrdererEnumerable<IGrouping<long, long>>
                .ToArray();

            foreach (IGrouping<long, long> group in ordered)
            {
                string text = group.Count() > 1
                    ? $"- {group.Key}ms {group.Count()}x"
                    : $"- {group.Key}ms";
                
                GUILayout.Label(text, EditorStyles.label);
            }

            GUILayout.EndVertical();
        }

        /* Draws the different snippets to the screen */
        private void OnGUI()
        {
            GUILayout.Space(5); // Stops the window rendering right at the top
            IReadOnlyDictionary<string, List<long>> snippets = CodeProfiler.GetProfilerSnippets();
            
            /* If there are no snippets, draws an inspector with instructions */
            if (snippets.Count == 0)
            {
                DrawEmptyProfiler();
                return;
            }
            
            /* Draws a quick overview of all snippets found */
            GUILayout.BeginVertical(style: "box");
            GUILayout.Label(text: $"[{snippets.Count}] different profiler snippets found");
            GUILayout.EndVertical();
            
            /* Draws each profiler snippet */
            ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);
            foreach (KeyValuePair<string, List<long>> snippet in snippets)
            {
                DrawProfilerSnippet(snippet.Key, snippet.Value);
            }
            EditorGUILayout.EndScrollView();
        }
    }
}