﻿using System;
using System.Security.Permissions;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;

namespace BlazorWebFormsComponents.System.Drawing.Design
{
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PaintValueEventArgs : EventArgs
	{
		private readonly ITypeDescriptorContext context;

		private readonly object valueToPaint;

		/// <include file='doc\PaintValueEventArgs.uex' path='docs/doc[@for="PaintValueEventArgs.graphics"]/*' />
		/// <devdoc>
		///     The graphics object with which the drawing should be done.
		/// </devdoc>
		private readonly Graphics graphics;

		/// <include file='doc\PaintValueEventArgs.uex' path='docs/doc[@for="PaintValueEventArgs.bounds"]/*' />
		/// <devdoc>
		///     The rectangle outlining the area in which the painting should be
		///     done.
		/// </devdoc>
		private readonly Rectangle bounds;

		/// <include file='doc\PaintValueEventArgs.uex' path='docs/doc[@for="PaintValueEventArgs.PaintValueEventArgs"]/*' />
		/// <devdoc>
		///     Creates a new PaintValueEventArgs with the given parameters.
		/// </devdoc>
		public PaintValueEventArgs(ITypeDescriptorContext context, object value, Graphics graphics, Rectangle bounds)
		{
			this.context = context;
			this.valueToPaint = value;

			this.graphics = graphics;
			if (graphics == null)
				throw new ArgumentNullException("graphics");

			this.bounds = bounds;
		}

		/// <include file='doc\PaintValueEventArgs.uex' path='docs/doc[@for="PaintValueEventArgs.Bounds"]/*' />
		/// <devdoc>
		///     The rectangle outlining the area in which the painting should be
		///     done.
		/// </devdoc>
		public Rectangle Bounds
		{
			get
			{
				return bounds;
			}
		}

		/// <include file='doc\PaintValueEventArgs.uex' path='docs/doc[@for="PaintValueEventArgs.Context"]/*' />
		/// <devdoc>
		///     ITypeDescriptorContext object for additional information about the context this value appears in.
		/// </devdoc>
		public ITypeDescriptorContext Context
		{
			get
			{
				return context;
			}
		}

		/// <include file='doc\PaintValueEventArgs.uex' path='docs/doc[@for="PaintValueEventArgs.Graphics"]/*' />
		/// <devdoc>
		///     Graphics object with which painting should be done.
		/// </devdoc>
		public Graphics Graphics
		{
			get
			{
				return graphics;
			}
		}

		/// <include file='doc\PaintValueEventArgs.uex' path='docs/doc[@for="PaintValueEventArgs.Value"]/*' />
		/// <devdoc>
		///     The value to paint.
		/// </devdoc>
		public object Value
		{
			get
			{
				return valueToPaint;
			}
		}
	}
}
