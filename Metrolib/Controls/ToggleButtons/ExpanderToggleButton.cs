﻿using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

// ReSharper disable CheckNamespace
namespace Metrolib.Controls
// ReSharper restore CheckNamespace
{
	/// <summary>
	///     A toggle button that is typically used to expand/contract an area of the UI.
	/// </summary>
	public class ExpanderToggleButton
		: ToggleButton
	{
		/// <summary>
		///     Definition of the <see cref="InvertedForeground" /> dependency property.
		/// </summary>
		public static readonly DependencyProperty InvertedForegroundProperty =
			DependencyProperty.Register("InvertedForeground", typeof (Brush), typeof (ExpanderToggleButton),
			                            new PropertyMetadata(default(Brush)));

		static ExpanderToggleButton()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof (ExpanderToggleButton),
			                                         new FrameworkPropertyMetadata(typeof (ExpanderToggleButton)));
		}

		/// <summary>
		///     The foreground color, used when <see cref="Properties.IsInvertedProperty" /> is set to true.
		/// </summary>
		public Brush InvertedForeground
		{
			get { return (Brush) GetValue(InvertedForegroundProperty); }
			set { SetValue(InvertedForegroundProperty, value); }
		}
	}
}