using System;
using System.Collections.Generic;
using System.Text;

namespace PrimitierMultiplayer.ClientLib
{
	public interface IChat
	{
		void Clear();
		void AddMessage(string sender, string message, ChatColor color);
	}
}
