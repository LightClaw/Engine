﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LightClaw.Engine.Graphics.OpenGL;
using LightClaw.Extensions;

namespace LightClaw.Engine.Graphics
{
    public class SamplerEffectUniform : EffectUniform, IBindable
    {
        private Sampler _Sampler;

        public Sampler Sampler
        {
            get
            {
                return _Sampler;
            }
            set
            {
                this.SetProperty(ref _Sampler, value);
            }
        }

        private Texture _Texture;

        public Texture Texture
        {
            get
            {
                return _Texture;
            }
            set
            {
                this.SetProperty(ref _Texture, value);
            }
        }

        private TextureUnit _TextureUnit = 0;

        public TextureUnit TextureUnit
        {
            get
            {
                Contract.Ensures(Contract.Result<TextureUnit>() >= TextureUnit.Zero);

                return _TextureUnit;
            }
            set
            {
                Contract.Requires<ArgumentNullException>(value >= TextureUnit.Zero);

                this.SetProperty(ref _TextureUnit, value);
            }
        }

        public SamplerEffectUniform(EffectPass pass, ProgramUniform uniform)
            : this(pass, uniform, 0)
        {
            Contract.Requires<ArgumentNullException>(pass != null);
            Contract.Requires<ArgumentNullException>(uniform != null);
        }

        public SamplerEffectUniform(EffectPass pass, ProgramUniform uniform, TextureUnit textureUnit)
            : base(pass, uniform)
        {
            Contract.Requires<ArgumentNullException>(pass != null);
            Contract.Requires<ArgumentNullException>(uniform != null);
            Contract.Requires<ArgumentOutOfRangeException>(textureUnit >= TextureUnit.Zero);

            this.TextureUnit = textureUnit;
        }

        public override Binding Bind()
        {
            Texture texture = this.Texture;
            if (texture != null)
            {
                this.Uniform.Set(this.TextureUnit);

                Sampler sampler = this.Sampler;
                if (sampler != null)
                {
                    sampler.Bind(this.TextureUnit);
                }

                texture.Bind(this.TextureUnit);

                return new Binding(this);
            }
            else
            {
                Log.Warn(() => "Texture to bind to the sampler in the shader was null and thus will not be bound. This is presumably unwanted behaviour!");
            }

            return default(Binding);
        }

        public void Set(Texture texture, TextureUnit textureUnit)
        {
            Contract.Requires<ArgumentOutOfRangeException>(textureUnit >= TextureUnit.Zero);

            this.Set(texture, null, textureUnit);
        }

        public void Set(Texture texture, Sampler sampler, TextureUnit textureUnit)
        {
            Contract.Requires<ArgumentOutOfRangeException>(textureUnit >= TextureUnit.Zero);

            this.Texture = texture;
            this.TextureUnit = textureUnit;
            this.Sampler = sampler;
        }

        public override void Unbind()
        {
            Texture texture = this.Texture;
            if (texture != null)
            {
                texture.Unbind();
            }
            Sampler sampler = this.Sampler;
            if (sampler != null)
            {
                sampler.Unbind();
            }
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(this._TextureUnit >= 0);
        }
    }
}
