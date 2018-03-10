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
		public XElement ToXml()
		{
			throw new NotImplementedException();
		}

		public static NodeComponent FromXml(XElement component)
		{
			//Assembly.
			throw new NotImplementedException();
		}
	}
}