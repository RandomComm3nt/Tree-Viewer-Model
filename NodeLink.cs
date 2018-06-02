
#region Using Statements

#endregion

using System;
using System.Xml.Linq;

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public class ComponentNodeLink
	{
		#region Fields

		public NodeComponent parentComponent;
		public TreeNode childNode;

		// type

		#endregion

		#region Methods
		
		public ComponentNodeLink(XElement e)
		{
			if (e != null)
				FromXml(e);
		}

		internal object ToXml()
		{
			throw new NotImplementedException();
		}

		public void FromXml(XElement e)
		{

		}

		#endregion
	}
}
