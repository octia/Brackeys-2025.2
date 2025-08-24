#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;

public static class AssetDatabaseExtensions
{
    /// <summary>
    /// Finds all assets of the specified type.
    /// </summary>
    /// <typeparam name="T">Type of asset to find.</typeparam>
    /// <param name="type">The type context (ignored, for extension syntax).</param>
    /// <returns>List of found assets of type T.</returns>
    public static List<T> FindAssets<T>(this Type type)
        where T : UnityEngine.Object
    {
        var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
        var results = new List<T>();

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset != null)
            {
                results.Add(asset);
            }
        }

        return results;
    }
}
#endif
