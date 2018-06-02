
#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
#endregion

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public class TreeNode
	{
		private string name = "TestName";
		private List<NodeComponent> components = new List<NodeComponent>();

		#region Properties

		public List<NodeComponent> Components
		{
			get
			{
				return components;
			}

			set
			{
				components = value;
			}
		}

		public string Name
		{
			get
			{
				return name;
			}

			set
			{
				name = value;
			}
		}

		#endregion

		#region Methods

		public TreeNode()
		{ }

		public TreeNode(XElement element)
		{
			FromXML(element);
		}

		public NodeComponent AddComponent(Type T)
		{
			NodeComponent c = (NodeComponent)Activator.CreateInstance(T);
			components.Add(c);
			return c;
		}

		public XElement ToXml()
		{
			XElement node = new XElement("Node");
			node.Add(new XAttribute("Name", name));
			
			foreach (NodeComponent c in components)
			{
				node.Add(c.ToXml());
			}

			return node;
		}

		public void FromXML(XElement node)
		{
			name = node.Attribute("Name").Value;

			components = node
				.Elements("Component")
				.Select(c => NodeComponent.FromXml(c))
				.ToList();
		}

		#endregion
	}
}
