using System;

namespace Pose
{
    public sealed class MockAttribute : Attribute
    {
        public MockAttribute(Type originalType)
        {
            OriginalType = originalType;
        }

        public Type OriginalType { get; }
    }
}