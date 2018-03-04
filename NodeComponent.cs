using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Assets.Scripts.Model.Data.TreeViewer
{
	public abstract class NodeComponent
	{
		internal object ToXml()
		{
			throw new NotImplementedException();
		}

		public static NodeComponent FromXml(XElement component)
		{
			return null;
		}
	}
}