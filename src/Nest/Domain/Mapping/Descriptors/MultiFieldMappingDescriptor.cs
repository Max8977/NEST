﻿using System;
using System.Linq.Expressions;
using Nest.Resolvers;

namespace Nest
{
	public class MultiFieldMappingDescriptor<T> where T : class
	{
		internal MultiFieldMapping _Mapping = new MultiFieldMapping();

		public MultiFieldMappingDescriptor<T> Name(string name)
		{
			this._Mapping.Name = name;
			return this;
		}
		public MultiFieldMappingDescriptor<T> Name(Expression<Func<T, object>> objectPath)
		{
			var name = new PropertyNameResolver().ResolveToLastToken(objectPath);
			this._Mapping.Name = name;
			return this;
		}

		public MultiFieldMappingDescriptor<T> Properties(Func<PropertiesDescriptor<T>, PropertiesDescriptor<T>> propertiesSelector)
		{
			propertiesSelector.ThrowIfNull("propertiesSelector");
			var properties = propertiesSelector(new PropertiesDescriptor<T>());
			foreach (var p in properties.Properties)
			{
				var value = p.Value as IElasticCoreType;
				if (value == null)
					continue;
				
				_Mapping.Fields[p.Key] = value;
			}
			return this;
		}
		
	}
}