# Getting Started

Some tips and tricks on how to begin using T4 Templating.

## Official Docs

- [MS Docs: Code Generation and T4 Text Templates](https://docs.microsoft.com/en-us/visualstudio/modeling/code-generation-and-t4-text-templates)
- [MS Docs: Design Time Templates](https://docs.microsoft.com/en-us/visualstudio/modeling/design-time-code-generation-by-using-t4-text-templates)

## Quick Examples

### Generate UnityEvent classes

Sometimes you may want to have many [UnityEvents](https://docs.unity3d.com/Manual/UnityEvents.html) with various typed parameters. Using a Text Template, you can quickly generate these classes:

```cs
<#@ template language="C#" #>
<#@ import namespace="System.Text" #>
<#@ import namespace="System.Collections.Generic" #>

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{

<#
    foreach (var type in new string[] {"Vector2", "Vector3"})
    {
#>
    [Serializable]
    public class <#= type #>Event : UnityEvent<<#= type #>> {}

<#
    }
#>

}
```

We embed a `foreach` loop that walks a list of type names (as strings). For each type, we create a class named `{Type}Event`, that inherits from `UnityEvent<{Type}>`. Here's the output:

```cs

using System;
using UnityEngine;
using UnityEngine.Events;

namespace Events
{
    [Serializable]
    public class Vector2Event : UnityEvent<Vector2> {}

    [Serializable]
    public class Vector3Event : UnityEvent<Vector3> {}
}
```
