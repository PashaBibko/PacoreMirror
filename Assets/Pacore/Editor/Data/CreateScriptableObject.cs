using System.IO;
using UnityEditor;
using UnityEngine;

namespace PashaBibko.Pacore.Editor.Data
{
    public static class ScriptableObjectGenerator
    {
        public static T Create<T>(string path, string name) where T : ScriptableObject
        {
            /* Creates the asset instance */
            T asset = ScriptableObject.CreateInstance<T>();

            /* Creates the file on disk */
            string fullpath = Path.Join(path, name);
            AssetDatabase.CreateAsset(asset, fullpath);
            
            /* Refreshes the asset database before returning */
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return asset;
        }
    }
}
