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

## How It Works: Theory

EDT works on two basic architectural principles.

**"Pass-Through" Blocks:** The "element" blocks operate on the principle of "passing-through" content properties from the rendering page, which means they show things from the page on which they are currently being displayed. You could embed the same `HeadingElementBlock` on two different pages, and it would display different things, because at its default, it just "passes-through" the `Name` of the page on which it's being displayed.  These blocks extend from `TemplateElementBaseBlock` which provides utility methods to find data from the rendering page.

**Delegation of the Template Property:** The template property that you add in step 6 above will "delegate" its value if it has no value of its own. If there are no blocks in it (or if it's `null`), that property will go looking for a `TemplateBlock` named for the page type in the "Templates" folder. If it finds one, the property will return the `Elements` property from that block as its own value. So, every page that doesn't provide a value for their template property will "fallback" to central template, which is the entire point of templating.

Those two basic principles are what make EDT work.

There's a third principle which isn't really "architectural," but just a feature hack:

**Template Execution:** EDT includes [Fluid](https://github.com/sebastienros/fluid) integration, with automatic content injection. An element block can process its output as a Fluid template, and content from the currently rendering page is injected into the context. This can be used to modify the output, or to provide a `ShowIf` expression to dictate whether the block is shown at all.

## How it Works: Process

Using the demo created above, here is a step-by-step description of what's happening when a page is being rendered.

When rendering gets to the view of `ArticlePage` it encounters the `Template` property.

Retrieving that property value will happen normally, if the property has a value. That value could be actual blocks, or it could be a `TemplateBlock` which contains an `Elements` content area. The view for `TemplateBlock` just renders the `Elements` content area...which is a roundabout way of saying this will render exactly how you expect -- the `TemplateBlock` will recurse down through its blocks and render each one.

If the `Template` property of the page has no value, it will use the `DynamicTemplatesResolver` service to try and find a template (actually, the logic for evaluating the local value is bundled up in here too). The default implementation of this service will look for a "Templates" folder in the root of the asset tree. If it finds that, it will look in it for a `TemplateBlock` object with a name that matches the page type name ("ArticlePage" in this case). If it finds that, the service will return the `Elements` property which will then be returned from the `Template` property of the page, as if it was its own.

>**TODO:** The logic currently contained in the sample template property should probably be encapsulated in a custom property, rather than a generic `ContentArea`.

Blocks rendered from this `Template` property (wherever they came from), will operate normally. However, so-called "element blocks" are those which extend from `TemplateElementBaseBlock`. That base class has some utility properties for `RenderingPageLink` and `RenderingPageData` that will give your code access to the page that is rendering -- so the page that block is being displayed on during that specific request. These properties can be used in your view models to power logic that decides where content should come from.

`TemplateElementBaseBlock` also includes a `ShowIf` property into which an editor can place an expression. This expression is evaluated by the templating engine, and it powers a `Show` boolean. If this property returns `false`, the block shouldn't be shown at all. This value can be used in your controller to abort display of the block. (Alternately, you could use it in your view to abort rendering via an early `return` or an `if...then` to modify rendering, but it's probably safer to do this in the controller.)

Inside your block view model, you can access `TemplatingService` which provides methods for processing templates which are injected with content from the rendering page.

>**TODO:** The current default implementation of `TemplatingService` uses Fluid as a template engine. This is fine, but it exposes Fluid in the interface (some methods take in a `TemplateContext`, which is Fluid-specific). This defeats the entire purpose of DI. The external interface should be engine-agnostic, so it can be injected with some other implementation.