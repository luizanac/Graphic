using OpenTK.Graphics.OpenGL4;
using System;
using System.IO;
using System.Text;

namespace Graphic.Shaders
{
	public class Shader
	{
		public readonly int Program;

		public Shader(string vertexPath, string fragmentPath)
		{
			var vertexShader = CompileShader(vertexPath, ShaderType.VertexShader);
			var fragmentShader = CompileShader(fragmentPath, ShaderType.FragmentShader);

			Program = GL.CreateProgram();
			GL.AttachShader(Program, vertexShader);
			GL.AttachShader(Program, fragmentShader);

			GL.LinkProgram(Program);

			GL.DetachShader(Program, vertexShader);
			GL.DetachShader(Program, fragmentShader);
			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);
		}

		public void Use()
		{
			GL.UseProgram(Program);
		}

		public int GetAttribLocation(string attribName)
		{
			return GL.GetAttribLocation(Program, attribName);
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
				GL.DeleteProgram(Program);
				_disposed = true;
			}
		}

		~Shader()
		{
			GL.DeleteProgram(Program);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
