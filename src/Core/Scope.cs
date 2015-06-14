//------------------------------------------------------------------------------
// <copyright file="Scope.cs">
//     Copyright (c) gsksoft. All rights reserved.
// </copyright>
// <description></description>
//------------------------------------------------------------------------------
namespace Gsksoft.GScript.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Scope
    {
        public static readonly Scope Global = new Scope(null);

        private Dictionary<string, object> m_objects = new Dictionary<string, object>();

        private Scope m_parent;

        public Scope(Scope parent)
        {
            m_parent = parent;
        }

        public void DefineVariable(string name, object value)
        {
            m_objects[name] = value;
        }

        public void SetValue(string name, object value)
        {
            Scope scope = LookupScope(name);
            if (scope != null)
            {
                scope.m_objects[name] = value;
            }
        }

        public object GetValue(string name)
        {
            Scope scope = LookupScope(name);
            if (scope == null)
            {
                return null;
            }

            return scope.m_objects[name];
        }

        private Scope LookupScope(string name)
        {
            Scope scope = this;
            do
            {
                var objects = scope.m_objects;
                if (objects.ContainsKey(name))
                {
                    return scope;
                }
                scope = scope.m_parent;
            } while (scope != null);

            return null;
        }
    }
}
