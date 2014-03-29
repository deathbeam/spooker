//-----------------------------------------------------------------------------
// Transform.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: None
// License: MIT
//-----------------------------------------------------------------------------

namespace Spooker.Graphics
{
	public class Transform
	{
		#region Private fields

		private SFML.Graphics.Transform _transform;

		#endregion

		#region SFML Helpers

		internal Transform(SFML.Graphics.Transform copy)
		{
			_transform = copy;
		}

		internal SFML.Graphics.Transform ToSfml()
		{
			return _transform;
		}

		#endregion

		#region Constructors

		public Transform ()
		{
			_transform = new SFML.Graphics.Transform ();
		}

		public Transform (
			float a00, float a01, float a02,
			float a10, float a11, float a12,
			float a20, float a21, float a22)
		{
			_transform = new SFML.Graphics.Transform (
				a00, a01, a02,
				a10, a11, a12,
				a20, a21, a22);
		}

		#endregion

		#region Public methods

		public Transform GetInverse(Transform transform)
		{
			return new Transform(_transform.GetInverse());
		}

		public void Combine(Transform transform)
		{
			_transform.Combine (transform.ToSfml());
		}

		public void Rotate(float angle)
		{
			_transform.Rotate (angle);
		}

		public void Rotate(float angle, Vector2 center)
		{
			_transform.Rotate (angle, center.ToSfml());
		}

		public void Scale(Vector2 factors)
		{
			_transform.Scale (factors.ToSfml());
		}

		public void Scale(Vector2 factors, Vector2 center)
		{
			_transform.Scale (factors.ToSfml(), center.ToSfml());
		}

		public void Translate(Vector2 offset)
		{
			_transform.Translate (offset.ToSfml());
		}

		public Vector2 TransformPoint(Vector2 point)
		{
			return new Vector2(_transform.TransformPoint (point.ToSfml()));
		}

		public Rectangle TransformRect(Rectangle rect)
		{
			var tmp = _transform.TransformRect (
				new SFML.Graphics.FloatRect (
					rect.X,
					rect.Y,
					rect.Width,
					rect.Height));

			return new Rectangle(
				(int)tmp.Left,
				(int)tmp.Top,
				(int)tmp.Width,
				(int)tmp.Height);
		}

		#endregion
	}
}

