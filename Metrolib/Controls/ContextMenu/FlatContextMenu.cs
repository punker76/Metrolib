﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

// ReSharper disable CheckNamespace

namespace Metrolib.Controls
// ReSharper restore CheckNamespace
{
	/// <summary>
	///     A context menu that is styled in the typical flat look.
	/// </summary>
	public class FlatContextMenu
		: ContextMenu
	{
		/// <summary>
		///     Definition of the <see cref="IsFirstItemHovered" /> property.
		/// </summary>
		public static readonly DependencyProperty IsFirstItemHoveredProperty =
			DependencyProperty.Register("IsFirstItemHovered", typeof (bool), typeof (FlatContextMenu),
			                            new PropertyMetadata(default(bool)));

		/// <summary>
		///     Definition of the <see cref="AnchorAlignment" /> property.
		/// </summary>
		public static readonly DependencyProperty AnchorAlignmentProperty =
			DependencyProperty.Register("AnchorAlignment", typeof (HorizontalAlignment), typeof (FlatContextMenu),
			                            new PropertyMetadata(default(HorizontalAlignment)));

		private readonly DispatcherTimer _timer;

		static FlatContextMenu()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof (FlatContextMenu),
			                                         new FrameworkPropertyMetadata(typeof (FlatContextMenu)));
		}

		/// <summary>
		///     Initializes this <see cref="FlatContextMenu" />.
		/// </summary>
		public FlatContextMenu()
		{
			_timer = new DispatcherTimer(DispatcherPriority.Normal) {Interval = TimeSpan.FromMilliseconds(10)};
			_timer.Tick += TimerOnTick;
			_timer.Start();
		}

		/// <summary>
		///     The horizontal alignment of the anchor.
		///     Is set to <see cref="HorizontalAlignment.Left" /> by default.
		/// </summary>
		public HorizontalAlignment AnchorAlignment
		{
			get { return (HorizontalAlignment) GetValue(AnchorAlignmentProperty); }
			set { SetValue(AnchorAlignmentProperty, value); }
		}

		/// <summary>
		///     Is set to true when the first item is hovered.
		/// </summary>
		public bool IsFirstItemHovered
		{
			get { return (bool) GetValue(IsFirstItemHoveredProperty); }
			set { SetValue(IsFirstItemHoveredProperty, value); }
		}

		private void TimerOnTick(object sender, EventArgs eventArgs)
		{
			FlatMenuItem first = FirstMenuItem();
			if (first != null)
			{
				first.MouseEnter += FirstOnMouseEnter;
				first.MouseLeave += FirstOnMouseLeave;
				_timer.Stop();
			}
		}

		private void FirstOnMouseLeave(object sender, MouseEventArgs mouseEventArgs)
		{
			IsFirstItemHovered = false;
		}

		private void FirstOnMouseEnter(object sender, MouseEventArgs mouseEventArgs)
		{
			IsFirstItemHovered = true;
		}

		private FlatMenuItem FirstMenuItem()
		{
			return Items.OfType<FlatMenuItem>().FirstOrDefault();
		}
	}
}