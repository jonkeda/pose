using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Pose.Helpers;

namespace Pose
{
    public partial class Shim : IShims
    {
        private MethodBase _original;
        private ShimDelegate _replacement;
        private Object _instance;
        private Type _type;
        private bool _setter;

        internal MethodBase Original
        {
            get
            {
                return _original;
            }
        }

        internal ShimDelegate Replacement
        {
            get
            {
                return _replacement;
            }
        }

        internal Object Instance
        {
            get
            {
                return _instance;
            }
        }

        internal Type Type
        {
            get
            {
                return _type;
            }
        }

        private Shim(MethodBase original, object instanceOrType)
        {
            _original = original;
            if (instanceOrType is Type type)
                _type = type;
            else
                _instance = instanceOrType;
        }

        private Shim(MethodBase original, object instanceOrType, object target, MethodInfo mockMethod) : this(original, instanceOrType)
        {
            _replacement = new ShimDelegate(target, mockMethod);
        }

        public static Shim Create(MethodBase original, object instanceOrType, object target, MethodInfo mockMethod)
        {
            return new Shim(original, instanceOrType, target, mockMethod);
        }

        public static Shim Replace(Expression<Action> expression, bool setter = false)
            => ReplaceImpl(expression, setter);

        public static Shim Replace<T>(Expression<Func<T>> expression, bool setter = false)
            => ReplaceImpl(expression, setter);

        private static Shim ReplaceImpl<T>(Expression<T> expression, bool setter)
        {
            MethodBase methodBase = ShimHelper.GetMethodFromExpression(expression.Body, setter, out object instance);
            return new Shim(methodBase, instance) { _setter = setter };
        }

        private Shim WithImpl(Delegate replacement)
        {
            ShimHelper.ValidateReplacementMethodSignature(this._original, replacement.Method, _instance?.GetType() ?? _type, _setter);
            _replacement = new ShimDelegate(replacement);
            return this;
        }

        public IEnumerable<Shim> GetShims()
        {
            yield return this;
        }
    }
}