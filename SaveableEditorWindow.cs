using System;
using UnityEditor;

namespace Hagans.Editor
{
    /// <summary>
    /// EditorWindow with an attached class automatically stored at EditorPrefs.
    /// </summary>
    /// <typeparam name="T">Custom serializable class attached to this EditorWindow.</typeparam>
    public abstract class SaveableEditorWindow<T> : EditorWindow
    {
        SerializedObject _window;
        T _instanceData;

        /// <summary>
        /// This <see cref="EditorWindow"/> runtime <see cref="SerializedObject"/>.
        /// </summary>
        protected SerializedObject Window
        {
            get
            {
                if (_window == null) throw new NullReferenceException("Window is only avaiable when an instance is active.");
                return _window;
            }
            set
            {
                if (_window == null) throw new NullReferenceException("Window is only avaiable when an instance is active.");
                _window = value;
            }
        }

        /// <summary>
        /// Default value for T if <see cref="EditorPrefs"/> has nothing saved.
        /// </summary>
        protected abstract T InitialData { get; }

        /// <summary>
        /// T instance generated to be saved at <see cref="EditorPrefs"/> procedurally.
        /// </summary>
        protected abstract T OnSave { get; }


        /// <summary>
        /// Action to do with EditorPrefs T data when loaded.
        /// </summary>
        /// <param name="data"></param>
        protected abstract void OnLoad(T data);

        /// <summary>
        /// Unity message. Loads <see cref="Window"/> and updates runtime data. 
        /// Nothing before base.OnEnable() will has a <see cref="Window"/> 
        /// instance or an updated T data.
        /// </summary>
        protected virtual void OnEnable()
        {
            var json = EditorPrefs.GetString(GetType().Name);
            if (!string.IsNullOrEmpty(json))
            {
                _instanceData = InitialData;
                EditorJsonUtility.FromJsonOverwrite(json, _instanceData);
                OnLoad(_instanceData);
            }
            _window = new SerializedObject(this);
        }

        /// <summary>
        /// Unity message. Saves runtime T data to EditorPrefs. Nothing after base.OnEnable() will has a <see cref="Window"/> instance.
        /// </summary>
        protected virtual void OnDisable() => Save();

        void Save()
        {
            Window.ApplyModifiedProperties();
            var json = EditorJsonUtility.ToJson(OnSave);
            EditorPrefs.SetString(GetType().Name, json);
        }
    }
}