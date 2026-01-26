using System.IO;
using UnityEngine;

namespace PashaBibko.Pacore.Data
{
    public static class PacoreDataLoader
    {
        private const string BASE_PATH = "Pacore/Resources/PacoreData";
        public static string FullResourcesPath => Path.Join(Application.dataPath, BASE_PATH);
        public static string ResourcesPath => Path.Join("Assets", BASE_PATH);
    }
}
