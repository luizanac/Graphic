using Graphic.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Desktop;
using System;

const int Width = 800, Height = 600;

GameWindowSettings gameWindowSettings = new()
{
	IsMultiThreaded = true,
	RenderFrequency = 60,
	UpdateFrequency = 60
};

NativeWindowSettings nativeWindowSettings = new()
{
	Title = "Graphic",
	Size = new Vector2i(Width, Height),
	Location = new Vector2i(1600, 300),
	StartFocused = false
};

using var application = new Application(gameWindowSettings, nativeWindowSettings);

Console.WriteLine("App running");
application.Run();
