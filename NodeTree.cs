
#region Using Statements
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
#endregion

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public class NodeTree
	{
		#region Fields

		private List<TreeNode> nodes;

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

		public void Load(string path)
		{

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

		#endregion
	}
}
