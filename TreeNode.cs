using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public abstract class TreeNode
	{
		private string name = "TestName";
		private List<NodeComponent> components = new List<NodeComponent>();

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

		public void AddComponent(Type T)
		{
			components.Add((NodeComponent)Activator.CreateInstance(T));
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
	}
}
