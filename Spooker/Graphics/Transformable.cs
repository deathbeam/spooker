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

		public Transformable()
		{
		}

		public Transformable(Transformable transformable)
		{
			Origin = transformable.Origin;
			Position = transformable.Position;
			Rotation = transformable.Rotation;
			Scale = transformable.Scale;
		}
		
		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}
		
		public float Rotation
		{
			get { return _rotation; }
			set { _rotation = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}
		public Vector2 Scale
		{
			get { return _scale; }
			set { _scale = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}
		
		public Vector2 Origin
		{
			get { return _origin; }
			set { _origin = value; _matrixNeedUpdate = true; _inverseNeedUpdate = true; }
		}
		
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