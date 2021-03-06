﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Metrolib.Controls.Charts.Line.Canvas
{
	/// <summary>
	///     Responsible for drawing an individual <see cref="ILineSeries" />.
	/// </summary>
	public abstract class AbstractLineSeriesCanvas
		: IDisposable
	{
		private readonly ILineSeries _lineSeries;
		private Range _displayedXRange;
		private Range _displayedYRange;
		private double _height;

		private bool _isDirty;
		private bool _isValuesDirty;
		private INotifyCollectionChanged _observableValues;
		private Point[] _values;
		private double _width;

		/// <summary>
		///     Initializes this canvas.
		/// </summary>
		/// <param name="lineSeries"></param>
		protected AbstractLineSeriesCanvas(ILineSeries lineSeries)
		{
			_lineSeries = lineSeries;
			_lineSeries.PropertyChanged += LineSeriesOnPropertyChanged;

			OnValuesChanged(_lineSeries.Values);
			MakeDirty();
		}

		/// <summary>
		///     The values this canvas should display.
		/// </summary>
		protected IEnumerable<Point> Values
		{
			get { return _values; }
		}

		/// <summary>
		///     The final range of x-values represented by this canvas.
		/// </summary>
		public abstract Range XRange { get; }

		/// <summary>
		///     The final range of y-values represented by this canvas.
		/// </summary>
		public abstract Range YRange { get; }

		/// <summary>
		///     The range of x-values that shall be displayed by this canvas.
		/// </summary>
		public Range DisplayedXRange
		{
			get { return _displayedXRange; }
			set
			{
				if (value == _displayedXRange)
					return;

				_displayedXRange = value;
				MakeDirty();
			}
		}

		/// <summary>
		///     The range of y-values that shall be displayed by this canvas.
		/// </summary>
		public Range DisplayedYRange
		{
			get { return _displayedYRange; }
			set
			{
				if (value == _displayedYRange)
					return;

				_displayedYRange = value;
				MakeDirty();
			}
		}

		/// <summary>
		///     The width in independent device units that this canvas may use.
		/// </summary>
		public double Width
		{
			get { return _width; }
			set
			{
				if (value == _width)
					return;

				_width = value;
				MakeDirty();
			}
		}

		/// <summary>
		///     The height in independent device units that this canvas may use.
		/// </summary>
		public double Height
		{
			get { return _height; }
			set
			{
				if (value == _height)
					return;

				_height = value;
				MakeDirty();
			}
		}

		/// <summary>
		///     The series this canvas is responsible for drawing.
		/// </summary>
		public ILineSeries Series
		{
			get { return _lineSeries; }
		}

		/// <summary>
		///     Disposes of this canvas.
		/// </summary>
		public void Dispose()
		{
			_lineSeries.PropertyChanged -= LineSeriesOnPropertyChanged;
		}

		/// <summary>
		///     Marks this canvas as dirty so the next call to <see cref="Update" />
		///     actually does something instead of early exiting.
		/// </summary>
		protected void MakeDirty()
		{
			_isDirty = true;
		}

		/// <summary>
		///     Projects the given values into view-space, taking into account the range
		///     this canvas should actually display, as well as the dimensions of this canvas.
		/// </summary>
		/// <param name="values"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		[Pure]
		protected List<Point> ProjectToView(IEnumerable<Point> values, int count)
		{
			var ret = new List<Point>(count);
			if (values != null)
			{
				foreach (Point point in values)
				{
					double x = _displayedXRange.GetRelative(point.X);
					double y = _displayedYRange.GetRelative(point.Y);

					var view = new Point(
						x*_width,
						_height*(1 - y)
						);
					ret.Add(view);
				}
			}
			return ret;
		}

		private void OnValuesChanged(IEnumerable<Point> values)
		{
			if (_observableValues != null)
			{
				_observableValues.CollectionChanged -= OnValuesCollectionChanged;
			}

			_values = values != null ? values.ToArray() : new Point[0];
			_observableValues = values as INotifyCollectionChanged;

			if (_observableValues != null)
			{
				_observableValues.CollectionChanged += OnValuesCollectionChanged;
			}
		}

		private void OnValuesCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
		{
			_isValuesDirty = true;
		}

		/// <summary>
		///     Updates this collection, if necessary.
		/// </summary>
		/// <returns>true when something has changed so this series needs to be redrawn.</returns>
		public virtual bool Update()
		{
			if (_isValuesDirty)
			{
				_values = Series.Values.ToArray();
			}

			if (_isDirty)
			{
				_isDirty = false;
				return true;
			}

			return false;
		}

		/// <summary>
		///     This method is called to actually draw the <see cref="ILineSeries" /> represented by this canvas.
		/// </summary>
		/// <param name="drawingContext"></param>
		public abstract void OnRender(DrawingContext drawingContext);

		private void LineSeriesOnPropertyChanged(object sender, PropertyChangedEventArgs args)
		{
			_isDirty = true;

			switch (args.PropertyName)
			{
				case "Values":
					OnValuesChanged(_lineSeries.Values);
					break;
			}
		}
	}
}