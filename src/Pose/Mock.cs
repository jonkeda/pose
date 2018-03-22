﻿using System;
using System.Linq;
using System.Reflection;
using Pose.Helpers;

namespace Pose
{
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
