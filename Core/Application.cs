using OpenTK;
using OpenTK.Graphics;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Graphics.OpenGL4;
using Graphic.Shaders;
using System;
using System.Diagnostics;

namespace Graphic.Core
{
	public class Application : GameWindow
	{
		private readonly float[] _vertices =
		{
			//Position          Texture coordinates
			0.5f,  0.5f, 0.0f, 1.0f, 1.0f, // top right
			0.5f, -0.5f, 0.0f, 1.0f, 0.0f, // bottom right
			-0.5f, -0.5f, 0.0f, 0.0f, 0.0f, // bottom left
			-0.5f,  0.5f, 0.0f, 0.0f, 1.0f  // top left
		};

		private readonly float[] _textureCordinates =
		{
			0.0f, 0.0f,
			1.0f, 0.0f,
			0.5f, 1.0f
		};

		private int _vertexBufferObject;
		private int _vertexArrayObject;

		private Stopwatch _timer;
		private Shader _shader;
		private Texture _texture;

		public Application(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
			: base(gameWindowSettings, nativeWindowSettings)
		{
			_timer = new Stopwatch();
			_timer.Start();
		}


		protected override void OnLoad()
		{
			GL.ClearColor(Color4.Black);

			_vertexBufferObject = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);

			GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);


			_vertexArrayObject = GL.GenVertexArray();
			GL.BindVertexArray(_vertexArrayObject);

			_shader = new Shader("Shaders/Shader.vert", "Shaders/Shader.frag");
			_shader.Use();

			var aPositionIndex = _shader.GetAttribLocation("aPosition");
			GL.VertexAttribPointer(aPositionIndex, 3, VertexAttribPointerType.Float, false, 5 * sizeof(float), 0);
			GL.EnableVertexAttribArray(aPositionIndex);

			int texCoordLocation = _shader.GetAttribLocation("aTexCoord");
			GL.EnableVertexAttribArray(texCoordLocation);
			GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false, 5 * sizeof(float), 3 * sizeof(float));

			_texture = Texture.LoadFromFile("Resources/wall.jpg");
			_texture.Use(TextureUnit.Texture0);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);

			float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

			GL.GetInteger(GetPName.MaxVertexAttribs, out int maxAttributeCount);
			Console.WriteLine($"Maximum number of vertex attributes supported: {maxAttributeCount}");

			base.OnLoad();
		}

		protected override void OnUnload()
		{
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
			GL.DeleteBuffer(_vertexBufferObject);
			GL.DeleteTexture(_texture.Handle);
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