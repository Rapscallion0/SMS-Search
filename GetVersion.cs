using System;
using System.Windows.Forms;
using System.Xml;
namespace Versions
{
	public class GetVersion
	{
		public Version GetNewVersion(string url, string xmlfile, string xmlRoot)
		{
			Version result = null;
			try
			{
				XmlTextReader xmlTextReader = new XmlTextReader(url + xmlfile);
				xmlTextReader.MoveToContent();
				string text = "";
				if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == xmlRoot)
				{
					while (xmlTextReader.Read())
					{
						if (xmlTextReader.NodeType == XmlNodeType.Element)
						{
							text = xmlTextReader.Name;
						}
						else
						{
							string a;
							if (xmlTextReader.NodeType == XmlNodeType.Text && xmlTextReader.HasValue && (a = text) != null && a == "version")
							{
								result = new Version(xmlTextReader.Value);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return result;
		}
	}
}
