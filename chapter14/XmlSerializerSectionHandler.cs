using System; 
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

namespace SitePoint.Cookbook.Configuration
{ 
	/// <summary>
	/// Flexible configuration section handler that uses XML serialization 
	/// to convert XML within a config file into an object.
	/// </summary>
	/// <remarks>
	/// <p>
	/// Usage: In your config file, use the following to map a section of 
	/// the configuration file to the handler:</p>
	/// <code>
	/// &lt;configuration&gt;
	///		&lt;configSections&gt;
	///			&lt;section name="MySection" type="Haack.Configuration.XmlSerializerSectionHandler, Haack.Configuration" /&gt;
	///		&lt;/configSections&gt;
	/// &lt;/configuration&gt;
	/// </code>
	/// <p>
	/// The section should look something like:
	/// <code>
	/// &lt;configuration&gt;
	///		&lt;MySection type="NameSpace.MyConfigSettingObject, MyAssembly"&gt;
	///			&lt;Foo&gt;1234&lt;/Foo&gt;
	///			&lt;Bar&gt;Blah&lt;/Bar&gt;
	///			&lt;Quux&gt;true&lt;/Quux&gt;
	///		&lt;/MySection&gt;
	/// &lt;/configuration&gt;
	/// </code>
	/// </p>
	/// <p>
	/// Then you can obtain the section via: </p>
	/// <code>ConfigurationSettings.GetConfig("MySection");</code>
	/// <p>
	/// And it will return an instance of the class "NameSpace.MyConfigSettingObject" 
	/// with the values from the config section.
	/// </p>
	/// <code>(MyConfigSettingObject)ConfigurationSettings.GetConfig("MySection");</code>
	/// </remarks>
	public class XmlSerializerSectionHandler : IConfigurationSectionHandler 
	{ 
		public object Create(object parent, object context, XmlNode section)
		{
			XPathNavigator navigator = section.CreateNavigator();

			string typename = (string)navigator.Evaluate("string(@type)");

			Type type = Type.GetType(typename);
			if (type == null)
				throw new ConfigurationErrorsException("The type '" + typename + "' is not a valid type. Double check the type parameter.");
			XmlSerializer serializer = new XmlSerializer(type);

			return serializer.Deserialize(new XmlNodeReader(section));
		}
	} 
}