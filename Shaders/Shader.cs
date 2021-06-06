using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace Graphic.Shaders
{
	public class Shader
	{
		private readonly int _program;

		public Shader(string vertexPath, string fragmentPath)
		{
			var vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
			var fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);

			_program = GL.CreateProgram();
			GL.AttachShader(_program, vertexShader);
			GL.AttachShader(_program, fragmentShader);

			GL.LinkProgram(_program);

			GL.DetachShader(_program, vertexShader);
			GL.DetachShader(_program, fragmentShader);
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}

		public void Use()
		{
			GL.UseProgram(_program);
		}

		public int GetAttribLocation(string attribName)
		{
			return GL.GetAttribLocation(_program, attribName);
		}

		private static int CompileShader(string path, ShaderType shaderType)
		{
			using var reader = new StreamReader(path, Encoding.UTF8);
			var shaderSource = reader.ReadToEnd();
			var shader = GL.CreateShader(shaderType);
			GL.ShaderSource(shader, shaderSource);
			GL.CompileShader(shader);

			var infoLogVert = GL.GetShaderInfoLog(shader);
			if (infoLogVert != string.Empty)
				Console.WriteLine(infoLogVert);

			return shader;
		}

		private bool _disposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				GL.DeleteProgram(_program);
				_disposed = true;
			}
		}

		~Shader()
		{
			GL.DeleteProgram(_program);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
