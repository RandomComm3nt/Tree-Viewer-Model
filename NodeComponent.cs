
#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
#endregion

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public abstract class NodeComponent
	{
		#region Static

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
						.ToDictionary(t => t.ToString(), t => t);
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

		#endregion

		#region Fields

		private List<ComponentNodeLink> nodeLinks;

		#endregion

		#region Properties

		public List<ComponentNodeLink> NodeLinks
		{
			get
			{
				return nodeLinks;
			}
		}

		#endregion

		#region Methods

		public NodeComponent()
		{
			nodeLinks = new List<ComponentNodeLink>();
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

			foreach (ComponentNodeLink l in nodeLinks)
			{
				node.Add(l.ToXml());
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
					// for each Property element in xml, try to find the matching component variable
					foreach (XElement p in component.Elements("Property"))
					{
						foreach (FieldInfo fi in ComponentFields[T])
						{
							if (p.Element("Key").Value == fi.Name)
							{
								fi.SetValue(o, p.Element("Value").Value);
							}
						}
					}
				}

				if (component.Element("ComponentNodeLink") != null)
					(o as NodeComponent).nodeLinks = component
						.Elements("ComponentNodeLink")
						.Select(e => new ComponentNodeLink(e))
						.ToList();

				return o as NodeComponent;
			}
			else
			{
				throw new Exception("Attempt to deserialize XML failed. Component of type " + component.Element("ComponentName").Value + " could not be found in component map");
			}
		}

		#endregion
	}
}