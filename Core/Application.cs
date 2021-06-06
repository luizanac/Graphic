using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using Graphic.Shaders;
using System;

namespace Graphic.Core
{
	public class Application : GameWindow
	{
		private readonly Vector3[] _vertices =
		{
			new Vector3(0.5f,  0.5f, 0.0f),
			new Vector3(0.5f, -0.5f, 0.0f),
			new Vector3(-0.5f, -0.5f, 0.0f),
			new Vector3(-0.5f,  0.5f, 0.0f)
		};

		private readonly uint[] _indices =
		{
			0, 1, 3,
			1, 2, 3
		};

		private int _vertexBufferObject;
		private int _vertexArrayObject;
		private int _elementBufferObject;

		private Shader _shader;

		public Application(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
			: base(gameWindowSettings, nativeWindowSettings)
		{
		}


		protected override void OnLoad()
		{
			GL.ClearColor(Color4.Black);

			_vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
			unsafe
			{
				GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(Vector3), _vertices, BufferUsageHint.StaticDraw);
			}

			_vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayObject);


			_shader = new Shader("Shaders/Shader.vert", "Shaders/Shader.frag");
			_shader.Use();

			GL.VertexAttribPointer(_shader.GetAttribLocation("aPosition"), 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			_elementBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
			GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(float), _indices, BufferUsageHint.StaticDraw);


			base.OnLoad();
		}

		protected override void OnUnload()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DeleteBuffer(_vertexBufferObject);
			_shader?.Dispose();
			base.OnUnload();
		}

		protected override void OnRenderFrame(FrameEventArgs args)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			_shader.Use();

			GL.BindVertexArray(_vertexArrayObject);

			//GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
			GL.DrawElements(PrimitiveType.TriangleStrip, _indices.Length, DrawElementsType.UnsignedInt, 0);

			Context.SwapBuffers();
			base.OnRenderFrame(args);
		}

		protected override void OnUpdateFrame(FrameEventArgs args)
		{
			if (KeyboardState.IsKeyDown(Keys.Escape))
			{
				Close();
			}

			base.OnUpdateFrame(args);
		}

		protected override void OnResize(ResizeEventArgs e)
		{
			GL.Viewport(0, 0, e.Width, e.Height);

			base.OnResize(e);
		}
	}
}