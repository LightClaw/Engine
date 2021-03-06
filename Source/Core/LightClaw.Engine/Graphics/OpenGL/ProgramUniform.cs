﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using LightClaw.Engine.Core;
using LightClaw.Engine.Threading;
using LightClaw.Extensions;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace LightClaw.Engine.Graphics.OpenGL
{
    /// <summary>
    /// Represents a shader uniform.
    /// </summary>
    [DebuggerDisplay("Name: {Name}, Location: {Location}, Type: {Type}")]
    public class ProgramUniform : DispatcherEntity
    {
        private int _Location;

        /// <summary>
        /// The uniform's location.
        /// </summary>
        public int Location
        {
            get
            {
                Contract.Ensures(Contract.Result<int>() >= 0);

                return _Location;
            }
            private set
            {
                Contract.Requires<ArgumentOutOfRangeException>(value >= 0);

                this.SetProperty(ref _Location, value);
            }
        }

        /// <summary>
        /// The uniforms name.
        /// </summary>
        public override string Name
        {
            get
            {
                return base.Name;
            }
            set
            {
                throw new NotSupportedException("The {0}s name cannot be set. It is hardcoded in the shader file.".FormatWith(typeof(ProgramUniform).Name));
            }
        }

        private ShaderProgram _Program;

        /// <summary>
        /// The <see cref="ShaderProgram"/> the <see cref="ProgramUniform"/> belongs to.
        /// </summary>
        public ShaderProgram Program
        {
            get
            {
                Contract.Ensures(Contract.Result<ShaderProgram>() != null);

                return _Program;
            }
            private set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                this.SetProperty(ref _Program, value);
            }
        }

        private ActiveUniformType _Type;

        /// <summary>
        /// The <see cref="Type"/> of the <see cref="ProgramUniform"/>.
        /// </summary>
        public ActiveUniformType Type
        {
            get
            {
                return _Type;
            }
            private set
            {
                this.SetProperty(ref _Type, value);
            }
        }

        /// <summary>
        /// Initializes a new <see cref="ProgramUniform"/>.
        /// </summary>
        /// <param name="program">The <see cref="ShaderProgram"/> the <see cref="ProgramUniform"/> belongs to.</param>
        /// <param name="index">The uniform index as per GL.GetProgramInterface.</param>
        public ProgramUniform(ShaderProgram program, int index)
        {
            Contract.Requires<ArgumentNullException>(program != null);
            Contract.Requires<ArgumentOutOfRangeException>(index >= 0);

            this.Program = program;

            int size;
            ActiveUniformType uniformType;
            base.Name = GL.GetActiveUniform(this.Program, index, out size, out uniformType);
            this.Location = GL.GetUniformLocation(program, this.Name);

            this.Type = uniformType; // Set indirectly to fire event
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(int value)
        {
            this.VerifyAccess();
            GL.ProgramUniform1(this.Program, this.Location, value);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        [CLSCompliant(false)]
        public void Set(uint value)
        {
            this.VerifyAccess();
            GL.ProgramUniform1(this.Program, this.Location, value);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(float value)
        {
            this.VerifyAccess();
            GL.ProgramUniform1(this.Program, this.Location, value);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(double value)
        {
            this.VerifyAccess();
            GL.ProgramUniform1(this.Program, this.Location, value);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public void Set(int value1, int value2)
        {
            this.VerifyAccess();
            GL.ProgramUniform2(this.Program, this.Location, value1, value2);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        [CLSCompliant(false)]
        public void Set(uint value1, uint value2)
        {
            this.VerifyAccess();
            GL.ProgramUniform2(this.Program, this.Location, value1, value2);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public void Set(float value1, float value2)
        {
            this.VerifyAccess();
            GL.ProgramUniform2(this.Program, this.Location, value1, value2);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        public void Set(double value1, double value2)
        {
            this.VerifyAccess();
            GL.ProgramUniform2(this.Program, this.Location, value1, value2);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        public void Set(int value1, int value2, int value3)
        {
            this.VerifyAccess();
            GL.ProgramUniform3(this.Program, this.Location, value1, value2, value3);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        [CLSCompliant(false)]
        public void Set(uint value1, uint value2, uint value3)
        {
            this.VerifyAccess();
            GL.ProgramUniform3(this.Program, this.Location, value1, value2, value3);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        public void Set(float value1, float value2, float value3)
        {
            this.VerifyAccess();
            GL.ProgramUniform3(this.Program, this.Location, value1, value2, value3);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        public void Set(double value1, double value2, double value3)
        {
            this.VerifyAccess();
            GL.ProgramUniform3(this.Program, this.Location, value1, value2, value3);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        /// <param name="value4">The fourth value.</param>
        public void Set(int value1, int value2, int value3, int value4)
        {
            this.VerifyAccess();
            GL.ProgramUniform4(this.Program, this.Location, value1, value2, value3, value4);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        /// <param name="value4">The fourth value.</param>
        [CLSCompliant(false)]
        public void Set(uint value1, uint value2, uint value3, uint value4)
        {
            this.VerifyAccess();
            GL.ProgramUniform4(this.Program, this.Location, value1, value2, value3, value4);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        /// <param name="value4">The fourth value.</param>
        public void Set(float value1, float value2, float value3, float value4)
        {
            this.VerifyAccess();
            GL.ProgramUniform4(this.Program, this.Location, value1, value2, value3, value4);
        }

        /// <summary>
        /// Sets the values of the uniform.
        /// </summary>
        /// <param name="value1">The first value.</param>
        /// <param name="value2">The second value.</param>
        /// <param name="value3">The third value.</param>
        /// <param name="value4">The fourth value.</param>
        public void Set(double value1, double value2, double value3, double value4)
        {
            this.VerifyAccess();
            GL.ProgramUniform4(this.Program, this.Location, value1, value2, value3, value4);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(Vector2 value)
        {
            this.Set(value.X, value.Y);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(Vector3 value)
        {
            this.Set(value.X, value.Y, value.Z);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(Vector4 value)
        {
            this.Set(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        public void Set(Quaternion value)
        {
            this.Set(value.X, value.Y, value.Z, value.W);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix2 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix2 value, bool transpose)
        {
            this.VerifyAccess();

            fixed (Matrix2* pValue = &value)
            {
                GL.ProgramUniformMatrix2(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix2x3 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix2x3 value, bool transpose)
        {
            this.VerifyAccess();

            fixed (Matrix2x3* pValue = &value)
            {
                GL.ProgramUniformMatrix2x3(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix2x4 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix2x4 value, bool transpose)
        {
            this.VerifyAccess();

            fixed (Matrix2x4* pValue = &value)
            {
                GL.ProgramUniformMatrix2x4(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix3x2 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix3x2 value, bool transpose)
        {
            this.VerifyAccess();
            
            fixed (Matrix3x2* pValue = &value)
            {
                GL.ProgramUniformMatrix3x2(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix3 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix3 value, bool transpose)
        {
            this.VerifyAccess();
            
            fixed (Matrix3* pValue = &value)
            {
                GL.ProgramUniformMatrix3(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix3x4 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix3x4 value, bool transpose)
        {
            this.VerifyAccess();

            fixed (Matrix3x4* pValue = &value)
            {
                GL.ProgramUniformMatrix3x4(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix4x2 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix4x2 value, bool transpose)
        {
            this.VerifyAccess();

            fixed (Matrix4x2* pValue = &value)
            {
                GL.ProgramUniformMatrix4x2(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix4x3 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix4x3 value, bool transpose)
        {
            this.VerifyAccess();

            fixed (Matrix4x3* pValue = &value)
            {
                GL.ProgramUniformMatrix4x3(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <remarks>The value will not be transposed.</remarks>
        /// <param name="value">The value.</param>
        public void Set(ref Matrix4 value)
        {
            this.Set(ref value, false);
        }

        /// <summary>
        /// Sets the value of the uniform.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="transpose">Indicates whether to transpose the matrix value before transmitting it to the shader.</param>
        public unsafe void Set(ref Matrix4 value, bool transpose)
        {
            this.VerifyAccess();

            fixed (Matrix4* pValue = &value)
            {
                GL.ProgramUniformMatrix4(this.Program, this.Location, 1, transpose, (float*)pValue);
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._Location >= 0);
            Contract.Invariant(this._Program != null);
        }
    }
}
