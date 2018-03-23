using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pose.Helpers;

namespace Pose
{
    public class Mock : IShims
    {
        public static Mock It(Type mockType)
        {
            MockAttribute attr = mockType.GetCustomAttribute<MockAttribute>();
            if (attr == null)
            {
                throw new ArgumentException(nameof(mockType), "MockAttribute is missing.");
            }
            return new Mock(attr.OriginalType, mockType);
        }

        public static Mock It(Type originalType, Type mockType)
        {
            return new Mock(originalType, mockType);
        }

        public static Mock It<TO, TM>()
        {
            return new Mock(typeof(TO), typeof(TM));
        }

        public static Mock It<TM>(object originalValue)
        {
            if (originalValue == null)
            {
                throw new ArgumentException(nameof(originalValue));
            }
            return new Mock(originalValue, typeof(TM));
        }

        public static Mock It(object originalValue, Type mockType)
        {
            if (originalValue == null)
            {
                throw new ArgumentException(nameof(originalValue));
            }
            return new Mock(originalValue, mockType);
        }

        private Mock(Type originalType, Type mockType)
        {
            CreateMethodShims(originalType, mockType, null);
        }

        private Mock(object originalValue, Type mockType)
        {
            Type originalType = originalValue.GetType();

            CreateMethodShims(originalType, mockType, originalValue);
        }


        private void CreateMethodShims(Type originalType, Type mockType, object target)
        {
            foreach (MethodInfo mockMethod in mockType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                MethodBase originalMethod;
                if (mockMethod.Name == originalType.Name)
                {
                    originalMethod = FindConstructorLike(originalType, mockMethod);
                }
                else
                {
                    originalMethod = FindMethodLike(originalType, mockMethod);
                }
                if (originalMethod != null)
                {
                    _shims.Add(Shim.Create(originalMethod, originalType, target, mockMethod));
                }
            }
        }

        private ConstructorInfo FindConstructorLike(Type originalType, MethodInfo mockMethod)
        {
            Type[] types = mockMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            return originalType.GetConstructor(types);
        }

        private MethodInfo FindMethodLike(Type originalType, MethodInfo mockMethod)
        {
            Type[] types = mockMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            return originalType.GetMethod(mockMethod.Name, types);
        }

        private readonly ShimCollection _shims = new ShimCollection();

        public IEnumerable<Shim> GetShims()
        {
            foreach (Shim shim in _shims)
            {
                yield return shim;
            }
        }
    }
}
