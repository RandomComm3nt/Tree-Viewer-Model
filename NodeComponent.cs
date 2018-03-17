using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public abstract class NodeComponent
	{
		private static Dictionary<string, Type> componentMap;
		private static Dictionary<Type, List<FieldInfo>> componentFields;

		public static Dictionary<string, Type> ComponentMap
		{
			get
			{
				if (componentMap == null)
				{
					componentMap = AppDomain.CurrentDomain.GetAssemblies()
						.SelectMany(a => a.GetTypes())
						.Where(t => t.IsSubclassOf(typeof(NodeComponent)))
						.ToDictionary(t => t.Name, t => t);
				}
				return componentMap;
			}

			set
			{
				componentMap = value;
			}
		}
		
		public static Dictionary<Type, List<FieldInfo>> ComponentFields
		{
			get
			{
				if (componentFields == null)
				{
					componentFields = ComponentMap
						.Values
						.ToDictionary(t => t, t => t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
							.Where(f => f.GetCustomAttribute(typeof(ComponentPropertyAttribute)) != null)
							.ToList());
				}
				return componentFields;
			}

			set
			{
				componentFields = value;
			}
		}

		public XElement ToXml()
		{
			XElement node = new XElement("Component");
			node.Add(new XElement("ComponentName", GetType().ToString()));
			if (ComponentFields.ContainsKey(GetType()))
			{
				foreach (FieldInfo fi in ComponentFields[GetType()])
				{
					XElement propertyNode = new XElement("Property");
					propertyNode.Add(new XElement("Key", fi.Name));
					propertyNode.Add(new XElement("Value", fi.GetValue(this)));
					node.Add(propertyNode);
				}
			}
			return node;
		}

		public static NodeComponent FromXml(XElement component)
		{
			Type T;
			if (ComponentMap.TryGetValue(component.Element("ComponentName").Value, out T))
			{
				Object o = Activator.CreateInstance(T);
				if (!(o is NodeComponent))
				{
					throw new Exception("Attempt to deserialize XML failed. Object of type " + o.GetType().Name + " could not be cast to NodeComponent");
				}
				if (ComponentFields.ContainsKey(T))
				{
					foreach (XElement p in component.Elements("Property"))
					{
						// FieldInfo fi in ComponentFields[T]
						// read component
					}
				}

				return o as NodeComponent;
			}
			else
			{
				throw new Exception("Attempt to deserialize XML failed. Component of type " + component.Element("ComponentName").Value + " could not be found in component map");
			}
		}
	}
}