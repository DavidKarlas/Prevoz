using System;
using Xamarin.Forms;

namespace Prevoz
{
	public class FlowLayout : Layout<View>
	{
		#region implemented abstract members of Layout

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			if (Children.Count != 2)
				throw new InvalidOperationException ("This Layout only supports 2 children");
			var sizeRequest1 = Children [0].GetSizeRequest (width, height).Request;
			LayoutChildIntoBoundingRegion (Children [0], new Rectangle (x, y, sizeRequest1.Width, sizeRequest1.Height));
			var sizeRequest2 = Children [1].GetSizeRequest (width, height).Request;
			if (sizeRequest1.Width + 5 + sizeRequest2.Width > width) {
				LayoutChildIntoBoundingRegion (Children [1], new Rectangle (width - sizeRequest2.Width, y + sizeRequest1.Height, sizeRequest2.Width, sizeRequest2.Height));
			} else {
				LayoutChildIntoBoundingRegion (Children [1], new Rectangle (width - sizeRequest2.Width, y, sizeRequest2.Width, sizeRequest2.Height));
			}
		}

		#endregion

		protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
		{
			if (Children.Count != 2)
				throw new InvalidOperationException ("This Layout only supports 2 children");
			var sizeRequest1 = Children [0].GetSizeRequest (widthConstraint, widthConstraint).Request;
			var sizeRequest2 = Children [1].GetSizeRequest (widthConstraint, widthConstraint).Request;
			if (sizeRequest1.Width + 5 + sizeRequest2.Width > widthConstraint) {
				return new SizeRequest (new Size (Math.Max (sizeRequest1.Width, sizeRequest2.Width), sizeRequest1.Height + sizeRequest2.Height));
			} else {
				return new SizeRequest (new Size (sizeRequest1.Width + 5 + sizeRequest2.Width, Math.Max (sizeRequest1.Height, sizeRequest2.Height)));
			}
		}
	}
}

