//-----------------------------------------------------------------------------
// Transformable.cs
//
// Spooker Open Source Game Framework
// Copyright (C) Indie Armory. All rights reserved.
// Website: http://indiearmory.com
// Other Contributors: Laurent Gomila @ http://sfml-dev.org
// License: MIT
//-----------------------------------------------------------------------------

using System;

namespace Spooker.Graphics
{
	/// <summary>
	/// Transformable.
	/// </summary>
	public class Transformable
	{
		private Vector2 _origin;
		private Vector2 _position;
		private float _rotation;
		private Vector2 _scale = Vector2.One;
		private Matrix _matrix;
		private Matrix _inverseMatrix;
		private bool _matrixNeedUpdate = true;
		private bool _inverseNeedUpdate = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Transformable"/> class.
		/// </summary>
		public Transformable()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Spooker.Graphics.Transformable"/> class.
		/// </summary>
		/// <param name="copy">Transformable.</param>
		public Transformable(Transformable copy)
		{
			Origin = copy.Origin;
			Position = copy.Position;
			Rotation = copy.Rotation;
			Scale = copy.Scale;
		}

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		/// <value>The position.</value>
		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}

		/// <summary>
		/// Gets or sets the rotation.
		/// </summary>
		/// <value>The rotation.</value>
		public float Rotation
		{
			get { return _rotation; }
			set { _rotation = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}

		/// <summary>
		/// Gets or sets the scale.
		/// </summary>
		/// <value>The scale.</value>
		public Vector2 Scale
		{
			get { return _scale; }
			set { _scale = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}

		/// <summary>
		/// Gets or sets the origin.
		/// </summary>
		/// <value>The origin.</value>
		public Vector2 Origin
		{
			get { return _origin; }
			set { _origin = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}

		/// <summary>
		/// Gets the transform.
		/// </summary>
		/// <value>The transform.</value>
		public Matrix Transform
		{
			get
			{
				if (_matrixNeedUpdate)
				{
					var angle = -_rotation * 3.141592654F / 180.0F;
					var cos = (float)Math.Cos (angle);
					var sin = (float)Math.Sin (angle);
					var sxc = _scale.X * cos;
					var syc = _scale.Y * cos;
					var sxs = _scale.X * sin;
					var sys = _scale.Y * sin;
					var tx = -_origin.X * sxc - _origin.Y * sys + _position.X;
					var ty = _origin.X * sxs - _origin.Y * syc + _position.Y;

					_matrix = new Matrix(
						sxc,  sys,  tx,
						-sxs, syc,  ty,
						0.0F, 0.0F, 1.0F);

					_matrixNeedUpdate = false;
				}
				return _matrix;
			}
		}

		/// <summary>
		/// Gets the inverse transform.
		/// </summary>
		/// <value>The inverse transform.</value>
		public Matrix InverseTransform
		{
			get
			{
				if (_inverseNeedUpdate)
				{
					_inverseMatrix = Matrix.Invert (Transform);
					_inverseNeedUpdate = false;
				}
				return _inverseMatrix;
			}
		}
	}
}