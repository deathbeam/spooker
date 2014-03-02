//-----------------------------------------------------------------------------
// Particle.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
// Website: http://xbox.create.msdn.com/en-US/education/catalog/sample/particle
// Other Contributors: Thomas Slusny @ http://indiearmory.com
// License: Microsoft Permissive License (Ms-PL)
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using SFML.Graphics;
using SFML.Window;
using SFGL.Graphics;
using SFGL.Time;
using SFGL.Utils;

namespace SFGL.Particles
{
	/// <summary>
	/// Used to specify the method of acceleration for a particle system.
	/// </summary>
	public enum AccelerationMode
	{
		/// <summary>
		/// The particle system does not use acceleration.
		/// </summary>
		None,

		/// <summary>
		/// The particle system computes the acceleration by using the
		/// MinAccelerationScale and MaxAccelerationScale values to compute a random
		/// scalar value which is then multiplied by the direction of the particles.
		/// </summary>
		Scalar,

		/// <summary>
		/// The particle system computes the acceleration by using the EndVelocity
		/// value and solving the equation vt = v0 + (a0 * t) for a0. See
		/// ParticleSystem.cs for more details.
		/// </summary>
		EndVelocity,

		/// <summary>
		/// The particle system computes the acceleration by using the
		/// MinAccelerationVector and MaxAccelerationVector values to compute a random
		/// vector value which is used as the acceleration of the particles.
		/// </summary>
		Vector
	}

	/// <summary>
	/// Settings class describes all the tweakable options used
	/// to control the appearance of a particle system. Many of the
	/// settings are marked with an attribute that makes them optional
	/// so that XML files can be simpler if they wish to use the default
	/// values.
	/// </summary>
	[Serializable]
	public class ParticleSettings
	{
		// Sets the range of particles used for each "effect" when the particle system
		// is used.
		public int MinNumParticles;
		public int MaxNumParticles;

		// Name of the texture used by this particle system.  
		public string TextureFilename = null;

		// MinDirectionAngle and MaxDirectionAngle are used to control the possible
		// directions of motion for the particles. We use degrees instead of radians
		// for the settings to make it easier to construct the XML. The ParticleSystem
		// will convert these to radians as it needs.
		public float MinDirectionAngle = 0;
		public float MaxDirectionAngle = 360;

		// MinInitialSpeed and MaxInitialSpeed are used to control the initial speed
		// of the particles.      
		public float MinInitialSpeed;
		public float MaxInitialSpeed;

		// Sets the mode for computing the acceleration of the particles.
		public AccelerationMode AccelerationMode = AccelerationMode.None;

		// Controls how the particle velocity will change over their lifetime. If set
		// to 1, particles will keep going at the same speed as when they were created.
		// If set to 0, particles will come to a complete stop right before they die.
		// Values greater than 1 make the particles speed up over time. This field is
		// used when using the AccelerationMode.EndVelocity mode.
		public float EndVelocity = 1f;

		// Controls the minimum and maximum acceleration for the particle when using the
		// AccelerationMode.Scalar mode.
		public float MinAccelerationScale = 0;
		public float MaxAccelerationScale = 0;

		// Controls the minimum and maximum acceleration for the particle when using the
		// AccelerationMode.Vector mode.
		public Vector2 MinAccelerationVector = Vector2.Zero;
		public Vector2 MaxAccelerationVector = Vector2.Zero;

		// Controls how much particles are influenced by the velocity of the object
		// which created them. AddParticles takes in a Vector2 which is the base velocity
		// for the particles being created. That velocity is first multiplied by this
		// EmitterVelocitySensitivity to determine how much the particles are actually
		// affected by that velocity.
		public float EmitterVelocitySensitivity = 0;

		// Range of values controlling how fast the particles rotate. Again, these
		// values should be in degrees for easier XML authoring.
		public float MinRotationSpeed = 0;
		public float MaxRotationSpeed = 0;

		// Range of values controlling how long a particle will last.
		public float MinLifetime;
		public float MaxLifetime;

		// Range of values controlling how big the particles are
		public float MinSize = 1;
		public float MaxSize = 1;

		// Controls the gravity applied to the particles. This can pull particles down
		// to simulate gravity, up for effects like smoke, or any other direction.        
		public Vector2 Gravity = Vector2.Zero;

		// Alpha blending settings. Our default gives us a BlendState equivalent to
		// BlendState.AlphaBlend which is suitable for many particle effects.
		public BlendMode BlendMode = BlendMode.Alpha;

		public ParticleSettings()
		{
		}

		public ParticleSettings(string filename)
		{
			StreamReader reader = new StreamReader (filename);
			XmlSerializer ser = new XmlSerializer (this.GetType ());
			ParticleSettings temp = (ParticleSettings)ser.Deserialize (reader);
			reader.Close ();

			this.MinNumParticles = temp.MinNumParticles;
			this.MaxNumParticles = temp.MaxNumParticles;
			this.TextureFilename = temp.TextureFilename;
			this.MinDirectionAngle = temp.MinDirectionAngle;
			this.MaxDirectionAngle = temp.MaxDirectionAngle;
			this.MinInitialSpeed = temp.MinInitialSpeed;
			this.MaxInitialSpeed = temp.MaxInitialSpeed;
			this.AccelerationMode = temp.AccelerationMode;
			this.EndVelocity = temp.EndVelocity;
			this.MinAccelerationScale = temp.MinAccelerationScale;
			this.MaxAccelerationScale = temp.MaxAccelerationScale;
			this.MinAccelerationVector = temp.MinAccelerationVector;
			this.MaxAccelerationVector = temp.MaxAccelerationVector;
			this.EmitterVelocitySensitivity = temp.EmitterVelocitySensitivity;
			this.MinRotationSpeed = temp.MinRotationSpeed;
			this.MaxRotationSpeed = temp.MaxRotationSpeed;
			this.MinLifetime = temp.MinLifetime;
			this.MaxLifetime = temp.MaxLifetime;
			this.MinSize = temp.MinSize;
			this.MaxSize = temp.MaxSize;
			this.Gravity = temp.Gravity;
			this.BlendMode = temp.BlendMode;
		}
	}
}
