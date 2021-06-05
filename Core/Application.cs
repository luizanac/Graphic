using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using Graphic.Shaders;

namespace Graphic.Core
{
	public class Application : GameWindow
	{
		private readonly float[] _vertices =
		{
			-0.5f, -0.5f, 0.0f, //Bottom-left vertex
			 0.5f, -0.5f, 0.0f, //Bottom-right vertex
			 0.0f,  0.5f, 0.0f  //Top vertex
		};

		private int _vertexBufferObject;
		private int _vertexArrayObject;

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

			_vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayObject);

			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);

			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexAttribArray(0);

			_shader = new Shader("Shaders/Shader.vert", "Shaders/Shader.frag");

			_shader.Use();


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

			GL.DrawArrays(PrimitiveType.Triangles, 0, 3);

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