﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using LightClaw.Engine.Core;
using LightClaw.Engine.Graphics;
using LightClaw.Engine.Graphics.OpenGL;
using LightClaw.Engine.IO;
using LightClaw.Extensions;
using OpenTK.Graphics.OpenGL4;
using LCBuffer = LightClaw.Engine.Graphics.OpenGL.Buffer;

namespace LightClaw.GameCode
{
    [DataContract]
    public class BasicMesh : Component
    {
        private const string vertexShaderSource = 
@"#version 400

in vec3 inVertexPosition;
in vec4 inVertexColor;

out vec4 vertexColor

void main(void)
{
	gl_Position = inVertexPosition;
	vertexColor = inVertexColor;
}";

        private const string fragmentShaderSource =
@"#version 400

in vec4 vertexColor;

out vec3 finalColor;

void main(void)
{
	finalColor = vertexColor;
}";

        private IBuffer indexBuffer;

        private VertexArrayObject vao;

        private ShaderProgram program;

        private IBuffer vertexBuffer;

        public BasicMesh() { }

        protected override void Dispose(bool disposing)
        {
            VertexArrayObject vao = this.vao;
            if (vao != null)
            {
                vao.Dispose();
            }
            IBuffer iBuffer = this.indexBuffer;
            if (iBuffer != null)
            {
                iBuffer.Dispose();
            }
            IBuffer vBuffer = this.vertexBuffer;
            if (vBuffer != null)
            {
                vBuffer.Dispose();
            }
            ShaderProgram program = this.program;
            if (program != null)
            {
                program.Dispose();
            }

            base.Dispose(disposing);
        }

        protected override void OnLoad()
        {
            (vertexBuffer = new LCBuffer(BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw)).Set(
                new Vertex[] { 
                    new Vertex(new Vector3(-1.0f, -1.0f, 0.0f), Vector3.Zero, Vector2.Zero, Color.Red),
                    new Vertex(new Vector3(1.0f, -1.0f, 0.0f), Vector3.Zero, Vector2.Zero, Color.Blue),
                    new Vertex(new Vector3(0.0f,  1.0f, 0.0f), Vector3.Zero, Vector2.Zero, Color.Yellow)
                }
            );
            (indexBuffer = new LCBuffer(BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw)).Set(new int[] { 1, 2, 3 });

            VertexAttributePointer[] pointers = new VertexAttributePointer[]
            {
                new VertexAttributePointer(VertexAttributeLocation.Position, 3, VertexAttribPointerType.Float, false, Vertex.SizeInBytes, 0),
                new VertexAttributePointer(VertexAttributeLocation.Color, 4, VertexAttribPointerType.UnsignedByte, false, Vertex.SizeInBytes, 32),
            };
            BufferDescription desc = new BufferDescription(this.vertexBuffer, pointers);
            this.vao = new VertexArrayObject(this.indexBuffer, desc);

            //IContentManager contentMgr = this.IocC.Resolve<IContentManager>();
            //Task<string> vertexShaderSourceTask = contentMgr.LoadAsync<string>("Shaders/Basic.vert");
            //Task<string> fragmentShaderSourceTask = contentMgr.LoadAsync<string>("Shaders/Basic.frag");

            //Task<string[]> shaderLoadTask = Task.WhenAll(vertexShaderSourceTask, fragmentShaderSourceTask);
            //shaderLoadTask.ContinueWith(t =>
            //{
            Shader[] shaders = new Shader[] 
            { 
                new Shader(vertexShaderSource, ShaderType.VertexShader, new[] { new VertexAttributeDescription("inVertexPosition", VertexAttributeLocation.Position), new VertexAttributeDescription("inVertexColor", VertexAttributeLocation.Color) }),
                new Shader(fragmentShaderSource, ShaderType.FragmentShader)
            };
            this.program = new ShaderProgram(shaders);
            //}, TaskContinuationOptions.OnlyOnRanToCompletion);
            //shaderLoadTask.ContinueWith(
            //    t => Logger.Warn(ex => "An exception of type '{0}' occured while loading the shaders.".FormatWith(ex.GetType().AssemblyQualifiedName), t.Exception, t.Exception),
            //    TaskContinuationOptions.OnlyOnFaulted
            //);

            base.OnLoad();
        }

        protected override void OnDraw()
        {
            VertexArrayObject vao = this.vao;
            ShaderProgram program = this.program;
            if (vao != null && program != null)
            {
                using (Binding programBinding = new Binding(program))
                using (Binding vaoBinding = new Binding(vao))
                {
                    vao.DrawIndexed();
                }
            }
            base.OnDraw();
        }
    }
}
