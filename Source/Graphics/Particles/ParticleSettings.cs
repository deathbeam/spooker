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
using System.IO;
using System.Xml.Serialization;

namespace Spooker.Graphics.Particles
{
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
		/// <summary>
		/// Sets the minimum range of particles used for each "effect" when the particle system
        /// is used.
		/// </summary>
		public int MinNumParticles;

        /// <summary>
        /// Sets the maximum range of particles used for each "effect" when the particle system
        /// is used.
        /// </summary>
		public int MaxNumParticles;

        /// <summary> Name of the texture used by this particle system.</summary>  
		public string TextureFilename = null;

        /// <summary> 
        /// MinDirectionAngle and MaxDirectionAngle are used to control the possible
		/// directions of motion for the particles. We use degrees instead of radians
		/// for the settings to make it easier to construct the XML. The ParticleSystem
        /// will convert these to radians as it needs.
        /// </summary>
		public float MinDirectionAngle = 0;

        /// <summary> 
        /// MinDirectionAngle and MaxDirectionAngle are used to control the possible
        /// directions of motion for the particles. We use degrees instead of radians
        /// for the settings to make it easier to construct the XML. The ParticleSystem
        /// will convert these to radians as it needs.
        /// </summary>
		public float MaxDirectionAngle = 360;

        /// <summary> 
		/// MinInitialSpeed and MaxInitialSpeed are used to control the initial speed
		/// of the particles.
        /// </summary>
		public float MinInitialSpeed;

        /// <summary> 
        /// MinInitialSpeed and MaxInitialSpeed are used to control the initial speed
        /// of the particles.
        /// </summary>
		public float MaxInitialSpeed;

        /// <summary> Sets the mode for computing the acceleration of the particles.</summary>
		public AccelerationMode AccelerationMode = AccelerationMode.None;

		/// <summary>
		/// Controls how the particle velocity will change over their lifetime. If set
		/// to 1, particles will keep going at the same speed as when they were created.
		/// If set to 0, particles will come to a complete stop right before they die.
		/// Values greater than 1 make the particles speed up over time. This field is
		/// used when using the AccelerationMode.EndVelocity mode.
		/// </summary>
		public float EndVelocity = 1f;

		/// <summary>
		/// Controls the minimum acceleration for the particle when using the
		/// AccelerationMode.Scalar mode.
		/// </summary>
		public float MinAccelerationScale = 0;

		/// <summary>
		/// Controls the maximum acceleration for the particle when using the
		/// AccelerationMode.Scalar mode.
		/// </summary>
		public float MaxAccelerationScale = 0;

		/// <summary>
		/// Controls the minimum acceleration for the particle when using the
		/// AccelerationMode.Vector mode.
		/// </summary>
		public Vector2 MinAccelerationVector = Vector2.Zero;

		/// <summary>
		/// Controls the maximum acceleration for the particle when using the
		/// AccelerationMode.Vector mode.
		/// </summary>
		public Vector2 MaxAccelerationVector = Vector2.Zero;

		/// <summary>
		/// Controls how much particles are influenced by the velocity of the object
		/// which created them. AddParticles takes in a Vector2 which is the base velocity
		/// for the particles being created. That velocity is first multiplied by this
		/// EmitterVelocitySensitivity to determine how much the particles are actually
		/// affected by that velocity.
		/// </summary>
		public float EmitterVelocitySensitivity = 0;

		/// <summary>
		/// Range of values controlling how fast the particles rotate. Again, these
		/// values should be in degrees for easier XML authoring.
		/// </summary>
		public float MinRotationSpeed = 0;

		/// <summary>
		/// Range of values controlling how fast the particles rotate. Again, these
		/// values should be in degrees for easier XML authoring.
		/// </summary>
		public float MaxRotationSpeed = 0;

		/// <summary>
		/// Range of values controlling how long a particle will last.
		/// </summary>
		public float MinLifetime;

		/// <summary>
		/// Range of values controlling how long a particle will last.
		/// </summary>
		public float MaxLifetime;

		/// <summary>
		/// Range of values controlling how big the particles are
		/// </summary>
		public float MinSize = 1;

		/// <summary>
		/// Range of values controlling how big the particles are
		/// </summary>
		public float MaxSize = 1;

		/// <summary>
		/// Controls the gravity applied to the particles. This can pull particles down
		/// to simulate gravity, up for effects like smoke, or any other direction.   
		/// </summary>     
		public Vector2 Gravity = Vector2.Zero;

		/// <summary>
		/// When you are lazy to recolor particle texture, use this :)
		/// </summary>     
		public Color Color = Color.White;

		/// <summary>
		/// Alpha blending settings. Our default gives us a BlendState equivalent to
		/// BlendState.AlphaBlend which is suitable for many particle effects.
		/// </summary>
		public SpriteBlendMode BlendMode = SpriteBlendMode.Alpha;

		/// <summary>
		/// Creates new instance of ParticleSettings class
		/// </summary>
		public ParticleSettings()
		{
		}

		/// <summary>
		/// Creates new instance of ParticleSettings class from file
		/// </summary>
		/// <param name="filename"></param>
		public ParticleSettings(string filename)
			: this(new FileStream(filename, FileMode.Open))
		{
		}

		/// <summary>
		/// Creates new instance of ParticleSettings class from file
		/// </summary>
		/// <param name="stream"></param>
		public ParticleSettings(Stream stream)
		{
			var reader = new StreamReader (stream);
			var ser = new XmlSerializer (GetType ());
			var temp = (ParticleSettings)ser.Deserialize (reader);
			reader.Close ();

			MinNumParticles = temp.MinNumParticles;
			MaxNumParticles = temp.MaxNumParticles;
			TextureFilename = temp.TextureFilename;
			MinDirectionAngle = temp.MinDirectionAngle;
			MaxDirectionAngle = temp.MaxDirectionAngle;
			MinInitialSpeed = temp.MinInitialSpeed;
			MaxInitialSpeed = temp.MaxInitialSpeed;
			AccelerationMode = temp.AccelerationMode;
			EndVelocity = temp.EndVelocity;
			MinAccelerationScale = temp.MinAccelerationScale;
			MaxAccelerationScale = temp.MaxAccelerationScale;
			MinAccelerationVector = temp.MinAccelerationVector;
			MaxAccelerationVector = temp.MaxAccelerationVector;
			EmitterVelocitySensitivity = temp.EmitterVelocitySensitivity;
			MinRotationSpeed = temp.MinRotationSpeed;
			MaxRotationSpeed = temp.MaxRotationSpeed;
			MinLifetime = temp.MinLifetime;
			MaxLifetime = temp.MaxLifetime;
			MinSize = temp.MinSize;
			MaxSize = temp.MaxSize;
			Gravity = temp.Gravity;
			BlendMode = temp.BlendMode;
			Color = temp.Color;
		}
	}
}
