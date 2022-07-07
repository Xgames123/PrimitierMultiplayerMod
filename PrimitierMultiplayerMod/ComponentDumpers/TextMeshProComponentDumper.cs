﻿using PrimitierModdingFramework.Debugging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

//TODO: replace with PMF one
namespace PrimitierMultiplayerMod.ComponentDumpers
{
	public class TextMeshProComponentDumper : ComponentDumper
	{
		public override string TargetComponentFullName => "TMPro.TextMeshPro";

		public override void OnDump(Component component, XmlElement xmlElement, ComponentDumperList dumperList)
		{
			try
			{
				var tmp = component.Cast<TMPro.TextMeshPro>();

				xmlElement.SetAttribute("Text", tmp.text);
				xmlElement.SetAttribute("FontSize", tmp.fontSize.ToString());
				if (tmp.font != null)
					xmlElement.SetAttribute("Font", tmp.font.name);
				else
					xmlElement.SetAttribute("Font", "Null");
				xmlElement.SetAttribute("Color", tmp.color.ToString());
				xmlElement.SetAttribute("FontWeight", tmp.fontWeight.ToString());
			}catch(NullReferenceException)
			{
				xmlElement.InnerText = "FAILED TO DUMP THIS COMPONENT (NullReferenceException)";
			}
		

		}
	}
}
