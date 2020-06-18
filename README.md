# Episerver Dynamic Templates

A method of allowing editorial control over object rendering in Episerver.

Demo video:

https://www.dropbox.com/s/5xe82trve10nlwg/dev-demo-video.mp4?dl=0

Installation of demo:

1. Create a new Alloy site
2. Copy the `DynamicTemplates` folder to the root of Alloy
3. Copy the `Views/Elements` folder to the `Views` folder of Alloy
4. Install Fluid: `install-package Fluid.Core -Prerelease`
5. It should recompile and run just fine
6. Add a property called `Template` to `ArticlePage` (or whatever page you like). A sample is in `DynamicTemplates/sample-template-property.txt`.
7. Delete all the page-specific stuff in the `ArticlePage` view (there's some nav menu stuff you should leave -- start with the `H1` and delete down from there). Replace it with the `Template` property you created in step 6: `@Html.PropertyFor(x => x.CurrentPage.Template)`
8. Recompile and start up the site
9. You can add elements to the `Template` property on a specific page. Or you can create a `TemplateBlock` (anywhere), add elements to that, and drag it into the `Template` property. Or -- and this is really the whole point -- you can create a top-level assets folder called `Templates` and put a `TemplateBlock` in there, named for the page type name (so, "ArticlePage," for example). If there is no value in the `Template` property, it will find that template and use it.

## How It Works

EDT works on two basic architectural principles.

**"Pass-Through" Blocks:** The "element" blocks operate on the principle of "passing-through" content properties from the rendering page, which means they show things from the page on which they are currently being displayed. You could embed the same `HeadingElementBlock` on two different pages, and it would display different things, because at its default, it just "passes-through" the `Name` of the page on which it's being displayed.  These blocks extend from `TemplateElementBaseBlock` which provides utility methods to find data from the rendering page.

**Delegation of the Template Property:** The template property that you add in step 6 above will "delegate" its value if it has no value of its own. If there are no blocks in it (or if it's `null`), that property will go looking for a `TemplateBlock` named for the page type in the "Templates" folder. If it finds one, the property will return the `Elements` property from that block as its own value. So, every page that doesn't provide a value for their template property will "fallback" to central template, which is the entire point of templating.

Those two basic principles are what make EDT work.

There's a third principle which isn't really "architectural," but just a feature hack:

**Template Execution:** EDT includes [Fluid](https://github.com/sebastienros/fluid) integration, with automatic content injection. An element block can process its output as a Fluid template, and content from the currently rendering page is injected into the context. This can be used to modify the output, or to provide a `ShowIf` expression to dictate whether the block is shown at all.