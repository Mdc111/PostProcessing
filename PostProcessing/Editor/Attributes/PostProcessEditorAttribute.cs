using System;

namespace UnityEditor.Rendering.PostProcessing
{
    ///     <summary>
        ///     Tells a <see cref="PostProcessEffectEditor{T}"/> class which run-time type it's an editor
        ///     for. When you make a custom editor for an effect, you need put this attribute on the editor
        ///     class. Test Change 2
        ///     </summary>
        ///     <seealso cref="T:UnityEditor.Rendering.PostProcessing.PostProcessEffectEditor`1"/>
            [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class PostProcessEditorAttribute : Attribute
    {
        ///     <summary>
                ///     The type that this editor can edit. test change 3
                ///     </summary>
                        public readonly Type settingsType;

        ///     <summary>
                ///     Creates a new attribute.
                ///     </summary>
                ///     <param name="settingsType">The type that this editor can edit</param>
                        public PostProcessEditorAttribute(Type settingsType)
        {
            this.settingsType = settingsType;
        }
    }
}
