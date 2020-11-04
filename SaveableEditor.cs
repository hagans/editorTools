using System;
using UnityEditor;

namespace Hagans.Editor
{
    /// <summary>
    /// Methods indorder to get serialized data from <see cref="SaveableEditorWindow{T}"/>.
    /// </summary>
    public static class SaveableEditor
    {
        /// <summary>
        /// Denotes the existence of an entry at <see cref="EditorPrefs"/>. Overwrites result if possible.
        /// </summary>
        /// <typeparam name="TSaveable">Saveable editor window target.</typeparam>
        /// <typeparam name="TData">Desired data type. Must match with data type.</typeparam>
        /// <param name="data">Instance where data will be recorded. It must be non null.</param>
        /// <returns>True if <see cref="EditorPrefs"/> serialized data wasn't null. Otherwise false.</returns>
        /// <exception cref="ArgumentException">Data to overwrite and <see cref="EditorPrefs"/> serialized data has not the same <see cref="Type"/>.</exception>
        public static bool TryGetValue<TSaveable, TData>(ref TData data) where TSaveable : SaveableEditorWindow<TData>
        {
            var json = EditorPrefs.GetString(typeof(TSaveable).Name);
            if (string.IsNullOrEmpty(json))
                return false;
            try { EditorJsonUtility.FromJsonOverwrite(json, data); }
            catch { throw new ArgumentException(typeof(TData).Name + " does not match with " + typeof(TSaveable)); }
            return true;
        }
    }
}