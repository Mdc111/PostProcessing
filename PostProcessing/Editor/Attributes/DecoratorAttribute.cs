using System;

namespace UnityEditor.Rendering.PostProcessing
{
    /// <summary>
                /// Tells a class which inspector attribute it's a decorator for. - Test Change
                /// </summary>
                            [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class DecoratorAttribute : Attribute
    {
        /// <summary>
                                /// The attribute type that this decorator can inspect.
                                /// </summary>
                                                        public readonly Type attributeType;

        /// <summary>
                                /// Creates a new attribute.
                                /// </summary>
                                                        public DecoratorAttribute(Type attributeType)
        {
            this.attributeType = attributeType;
        }
    }
}
