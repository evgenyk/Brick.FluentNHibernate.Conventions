using System;
using Brick.FluentNHibernate.Conventions.Conventions.Attributes;
using FluentNHibernate;
using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Steps;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;
using FluentNHibernate.MappingModel.ClassBased;
using FluentNHibernate.MappingModel.Collections;

namespace Brick.FluentNHibernate.Conventions.Conventions
{
    /// <summary>
    /// Credits: http://stackoverflow.com/questions/6822430/fluent-nhibernate-mapping-hasmanytomany-by-convention/12198533#12198533
    /// </summary>
    class ExplicitHasManyToManyStep : IAutomappingStep
    {
        readonly IAutomappingConfiguration _configuration;
        readonly IAutomappingStep _defaultManyToManyStep;

        public ExplicitHasManyToManyStep(IAutomappingConfiguration configuration, IAutomappingStep defaultManyToManyStep)
        {
            _configuration = configuration;
            _defaultManyToManyStep = defaultManyToManyStep;
        }

        public bool ShouldMap(Member member)
        {
            var shouldMap = _defaultManyToManyStep.ShouldMap(member) || member.MemberInfo.IsDefined(typeof(HasManyToManyAttribute), true);
            return shouldMap;

            //modify this statement to check for other attributes or conventions
        }

        public void Map(ClassMappingBase classMap, Member member)
        {
            if (_defaultManyToManyStep.ShouldMap(member))
            {
                _defaultManyToManyStep.Map(classMap, member);
                return;
            }

            var collection = CreateManyToMany(classMap, member);
            classMap.AddCollection(collection);
        }

        CollectionMapping CreateManyToMany(ClassMappingBase classMap, Member member)
        {
            var parentType = classMap.Type;
            var childType = member.PropertyType.GetGenericArguments()[0];

            var collection = CollectionMapping.For(CollectionTypeResolver.Resolve(member));
            collection.ContainingEntityType = parentType;
            collection.Set(x => x.Name, Layer.Defaults, member.Name);
            collection.Set(x => x.Relationship, Layer.Defaults, CreateManyToMany(member, parentType, childType));
            collection.Set(x => x.ChildType, Layer.Defaults, childType);
            collection.Member = member;

            SetDefaultAccess(member, collection);
            SetKey(member, classMap, collection);
            return collection;
        }

        void SetDefaultAccess(Member member, CollectionMapping mapping)
        {
            var resolvedAccess = MemberAccessResolver.Resolve(member);

            if (resolvedAccess != Access.Property && resolvedAccess != Access.Unset)
            {
                mapping.Set(x => x.Access, Layer.Defaults, resolvedAccess.ToString());
            }

            if (member.IsProperty && !member.CanWrite)
            {
                mapping.Set(x => x.Access, Layer.Defaults, _configuration.GetAccessStrategyForReadOnlyProperty(member).ToString());
            }
        }

        static ICollectionRelationshipMapping CreateManyToMany(Member member, Type parentType, Type childType)
        {
            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, childType.Name + "Id");

            var mapping = new ManyToManyMapping { ContainingEntityType = parentType };
            mapping.Set(x => x.Class, Layer.Defaults, new TypeReference(childType));
            mapping.Set(x => x.ParentType, Layer.Defaults, parentType);
            mapping.Set(x => x.ChildType, Layer.Defaults, childType);
            mapping.AddColumn(Layer.Defaults, columnMapping);

            return mapping;
        }

        static void SetKey(Member property, ClassMappingBase classMap, CollectionMapping mapping)
        {
            var columnName = property.DeclaringType.Name + "Id";
            var columnMapping = new ColumnMapping();
            columnMapping.Set(x => x.Name, Layer.Defaults, columnName);

            var key = new KeyMapping { ContainingEntityType = classMap.Type };
            key.AddColumn(Layer.Defaults, columnMapping);

            mapping.Set(x => x.Key, Layer.Defaults, key);
        }
    }
}