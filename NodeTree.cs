
#region Using Statements
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
#endregion

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public class NodeTree
	{
		#region Fields

		private List<TreeNode> nodes;
		private string name;

		#endregion

		#region Properties

		public List<TreeNode> Nodes
		{
			get
			{
				return nodes;
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

		public NodeTree()
		{
			nodes = new List<TreeNode>();
		}

		public void AddNode(TreeNode node)
		{
			nodes.Add(node);
		}

		public void Save(string path)
		{
			XDocument document = new XDocument();
			XElement rootElement = new XElement("NodeList");
			document.Add(rootElement);
			foreach (TreeNode n in nodes)
			{
				rootElement.Add(n.ToXml());
			}
			File.WriteAllText(path, document.ToString());
		}

		public void Load(TextAsset textAsset)
		{
			XDocument document = XDocument.Parse(textAsset.text);
			nodes = document
				.Element("NodeList")
				.Elements("Node")
				.Select(n => new TreeNode(n))
				.ToList();
		}

		#endregion
	}
}
