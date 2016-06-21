using System;
using System.Collections;
using System.Reflection;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace Brick.FluentNHibernate.Conventions.Fluent
{
    public abstract class FluentConfigurationBase : DefaultAutomappingConfiguration
    {
        public override bool IsId(Member member)
        {
            var isId = member.MemberInfo.GetCustomAttribute<IdAttribute>() != null || base.IsId(member);
            return isId;
        }

        public override bool IsDiscriminated(Type type)
        {
            return true;
        }
    }
}