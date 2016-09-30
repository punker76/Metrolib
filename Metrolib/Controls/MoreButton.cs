﻿using System.Windows;
using Metrolib.Controls.Base;

namespace Metrolib.Controls
{
	/// <summary>
	///     A button that allows the user to show more content than is regularly visible, for example through
	///     a context-menu.
	/// </summary>
	public class MoreButton
		: FlatButton
	{
		static MoreButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof (MoreButton), new FrameworkPropertyMetadata(typeof (MoreButton)));
		}
	}
}