using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace Pose
{
    public class ShimDelegate
    {
        public ShimDelegate(Delegate replacement)
        {
            Target = replacement.Target;
            Method = replacement.Method;
        }

        public ShimDelegate(object target, MethodInfo method)
        {
            Target = target;
            Method = method;
        }

        public object Target { get; }

        public MethodInfo Method { get; }
    }

    public class ShimCollection : Collection<Shim>
    { }

    public class Mock
    {
        public Mock(Type originalType, Type mockType)
        {
            CreateShims(originalType, mockType);
        }

        private void CreateShims(Type originalType, Type mockType)
        {
            CreateMethodShims(originalType, mockType);
        }

        private void CreateMethodShims(Type originalType, Type mockType)
        {
            foreach (MethodInfo mockMethod in mockType.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                MethodInfo originalMethod = FindMethodLike(originalType, mockMethod);
                if (originalMethod != null)
                {
                    object target = null;
                    _shims.Add(Shim.Create(originalMethod, originalType, null, mockMethod));
                }
            }
            
        }

        private MethodInfo FindMethodLike(Type originalType, MethodInfo mockMethod)
        {
            Type[] types = mockMethod.GetParameters().Select(p => p.ParameterType).ToArray();
            return originalType.GetMethod(mockMethod.Name, types);
        }

        private readonly ShimCollection _shims = new ShimCollection();

        public Shim[] Shims
        {
            get { return _shims.ToArray(); }
        }

    }
}
