# Data Binding

> Note: This is a `Beta` feature, and is being actively developed. To use this feature, you need [`importerVersion`](xref:FasterGames.T4.Editor.TextTemplateImporter.importerVersion) to be set to [`Beta`](xref:FasterGames.T4.Editor.ImporterVersion.Beta).

Data binding allows sharing data from Unity with a template. This allows the template to access and use that data during generation.

## Getting started


- Create a class to represent the data you wish to embed:
```
[CreateAssetMenu(menuName = "EmbedData")]
public class EmbedData : ScriptableObject
{
	public List<string> Animals;
}
```
- Create an instance of this data in the Editor [Project window](https://docs.unity3d.com/Manual/ProjectView.html) (Right click -> Create -> EmbedData)
- Create a TT file to use this data:
```
<#@ template debug="false" hostspecific="true" language="C#" #>
<#@ output extension=".cs" #>
<#@ import namespace="System.IO" #>
using System;

<#
    foreach (var animal in Host.Data.Animals)
    {
#>

[Serializable]
public class <#= animal#>Test {}

<#
    }
#>
```
- Select the TT file in the [Project window](https://docs.unity3d.com/Manual/ProjectView.html)
- In the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html) ensure [`importerVersion`](xref:FasterGames.T4.Editor.TextTemplateImporter.importerVersion) is set to [`Beta`](xref:FasterGames.T4.Editor.ImporterVersion.Beta)
- Drag the data instance you created in the [Project window](https://docs.unity3d.com/Manual/ProjectView.html) to the [`embeddedData`](xref:FasterGames.T4.Editor.TextTemplateImporter.embeddedData) section in the [Inspector](https://docs.unity3d.com/Manual/UsingTheInspector.html)
- Select "Apply"

This will cause the template to be run such that `Host.Data` is the instance of your data.

## Troubleshooting

### Does the template need to be host specific?

Yes - to inform the template engine to use the host with our data, [`hostspecific`](https://docs.microsoft.com/en-us/visualstudio/modeling/t4-template-directive?view=vs-2019#hostspecific-attribute) must be set to `true`.

For instance:
```
<#@ template debug="false" hostspecific="true" language="C#" #>
```

### Why does my template break when embeddedData is enabled?

If  your template fails to generate content after embedding data, this may be because the data class you're using is complex, and references other types that the template environment is not aware of. Usually if this is the case, the error will inform you of what type is missing. You may specify this type in [`additionalTypes`](xref:FasterGames.T4.Editor.TextTemplateImporter.additionalTypes) list, or simplify your data class.

If you're not able to get it working, please [open an Issue](https://github.com/faster-games/t4/issues/new).
