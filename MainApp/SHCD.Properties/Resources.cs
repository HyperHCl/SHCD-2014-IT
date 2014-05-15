using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
namespace SHCD.Properties
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;
		private static CultureInfo resourceCulture;
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("SHCD.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}
		internal static Bitmap doneL
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("doneL", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap doneM
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("doneM", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Icon Main
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("Main", Resources.resourceCulture);
				return (Icon)@object;
			}
		}
		internal static Bitmap security
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("security", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap undoL
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("undoL", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap undoM
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("undoM", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal static Bitmap win
		{
			get
			{
				object @object = Resources.ResourceManager.GetObject("win", Resources.resourceCulture);
				return (Bitmap)@object;
			}
		}
		internal Resources()
		{
		}
	}
}
