# t4

T4 text template generative importer for Unity3D. 📝🏗

![Project logo; A pink package on a grey background, next to the text "T4 Templates" in purple](./Documentation~/header.png)

![GitHub package.json version](https://img.shields.io/github/package-json/v/faster-games/t4)
[![openupm](https://img.shields.io/npm/v/com.faster-games.t4?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.faster-games.t4/)
[![CI](https://github.com/faster-games/t4/actions/workflows/main.yml/badge.svg)](https://github.com/faster-games/t4/actions/workflows/main.yml)
[![Discord](https://img.shields.io/discord/862006447919726604)](https://discord.gg/QfQE6rWQqq)

[T4 Text Templates](https://docs.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates) provide a way to generate csharp code from templates, which are a mixture of text blocks and control logic. If you've ever used [Mushtache](http://mustache.github.io/mustache.5.html) or [Go's html/template](https://gowebexamples.com/templates/), T4 templates are pretty similar. This package allows [Unity](http://unity3d.com/) developers to author T4 templates (`.tt` files), and rely on the [Editor](https://docs.unity3d.com/Manual/UsingTheEditor.html) to process them and generate code.

## Installing

This package supports [openupm](https://openupm.com/packages/com.faster-games.t4/) - you can install it using the following command:

```
openupm add com.faster-games.t4
```

Or by adding directly to your `manifest.json`:

> Note: You may also use specific versions by appending `#{version}` where version is a [Release tag](https://github.com/faster-games/t4/releases) - e.g. `#v1.2.0`.

```
dependencies: {
	...
	"com.faster-games.whiskey": "git+https://github.com/faster-games/t4.git"
}
```

Or by using [Package Manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) to "Add a package from Git URL", using the following url:

```
https://github.com/faster-games/t4.git
```

## Documentation

<center>

[Manual 📖](https://t4.faster-games.com/manual/getting-started.html) | [Scripting API 🔎](https://t4.faster-games.com/ref/FasterGames.T4.Editor.html)

</center>

Please also see these resources from Microsoft, as they are the authors and maintainers of the `T4` templating featureset and format:

- [MS Docs: Code Generation and T4 Text Templates](https://docs.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates)
- [MS Docs: Design Time Templates](https://docs.microsoft.com/en-us/visualstudio/modeling/design-time-code-generation-by-using-t4-text-templates)

### Quickstart

- Add a `.tt` file under `Assets`
- Select it in the [Project window](https://docs.unity3d.com/Manual/ProjectView.html)
- Note that the importer used is `TextTemplateImporter` - that's us!
- Any processing errors will be shown in the [Console](https://docs.unity3d.com/Manual/Console.html)

## Supporting the project

If this project saved you some time, and you'd like to see it continue to be invested in, consider [buying me a coffee. ☕](https://www.buymeacoffee.com/bengreenier) I do this full-time, and every little bit helps! 💙
