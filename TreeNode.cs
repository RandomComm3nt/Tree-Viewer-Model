using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public abstract class TreeNode
	{
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
	}
}
