# Episerver Dynamic Templates

This code offers a method of allowing editorial control over object rendering in Episerver. It's referred to as "templating" because a single output configuration can be used by many different content objects -- it can be bound to a particular content type, a defined set of content objects, or even a single content object. If the output configuration (the "template") changes, the output of all content objects using it will also change.

In Episerver, there has traditionally only been one type of templating --

**Developer Templating:** This is done in Razor, from files, using HTML markup and C#. It's low-level and above the skill level of most editors. It normally requires a code deployment (though there are [ways around that](https://github.com/davidknipe/VirtualTemplateSystem)), and it's prone to catastrophic problems if done poorly.

EDT offers a new type of templating _in addition_ to developer templating:

**Editorial Templating:** This is done from the Episerver interface, by choosing and configuring blocks and optionally using some lightweight markup and logical templating code. It requires no development and no deployment.

The two styles can co-exist and work together. Developers can template the "surround" -- the outer layout, which includes the navigation and deeper integration -- while allowing editors and front-end developers a "sandbox" in which they can control the output of a specific object on the page.

## Status

Ridiculously alpha. This is a promising prototype, at best.

## Demo Video

This video is about 11 minutes. If you're not a developer or don't care how it's installed, skip ahead to 2:40. 

https://www.dropbox.com/s/5xe82trve10nlwg/dev-demo-video.mp4?dl=0

## Installation of the Demo

1. Create a new Alloy site
2. Copy the `DynamicTemplates` folder to the root of Alloy
3. Copy the `Views/Elements` folder to the `Views` folder of Alloy
4. Install Fluid: `install-package Fluid.Core -Prerelease`
5. It should recompile and run just fine (if it doesn't...well, _fix it_)
6. Add a property called `Template` to `ArticlePage` (or whatever page you like). This is just a `ContentArea` with some extra code in the getter. A sample is in `DynamicTemplates/sample-template-property.txt`. (Note: there's nothing magic about the name "Template." Call it whatever you want. When I refer to "the `Template` property" below, I mean this property, whatever you called it.)
7. Delete all the page-specific stuff in the `ArticlePage` view (there's some nav menu stuff you should leave -- start with the `H1` and delete down from there). Replace it with the `Template` property you created in step 6: `@Html.PropertyFor(x => x.CurrentPage.Template)`
8. Recompile and start up the site

## How to Template

Once a page type is "enabled" for templating by (1) adding a `Template` property as described in step 6 above, and (2) outputting _just_ that property in the view), you have three options to template that object. These three options are in listed decreasing order of specificity.

1. You can add blocks directly to the `Template` property on a specific content object. This is really no different than how Episerver has always worked. The only enhancement might be to use some of the "pass-through" blocks as described below. This is specific to _this_ content object, and it doesn't gain you any efficiencies -- it's not really "templating" because you'd need to do it for every single object, which defeats the purpose (but can be handy on an exception basis).
2. You can create a `TemplateBlock` block, add some blocks to that, then drag it into the `Template` property. This is a little better because you can change that `TemplateBlock` and it will change the output of every page linked to it. You could link every content object of a particular type to a single `TemplateBlock` and centrally control their output. But this is still not ideal because you'd need to remember to add this to every property, which can be tedious and prone to error.
3. You can leave the `Template` property blank. Then create a top-level folder called "Templates" in the asset panel, and create a `TemplateBlock` in that folder named for the page type it should be used for ("ArticlePage," for example). If the `Template` property on an object is empty, EDT will find the template for its type and use it. This allows you to designate a specific template to be used automatically for _all_ pages of a specific type, which will likely be the most common use case.

>**NOTE:** The template resolution is injected via the `DynamicTemplateResolver`. You're welcome to implement your own service to use another method. Also, the default implementation has a static property called `TEMPLATE_FOLDER_NAME` that you can change if you want to use something different.

All three options can be used simultaneously. You might have all content of a specific type use a default template (option #3 from above), but have a group of them linked to an alternate template (#2), and maybe one weird one that's uniquely templated (#1).

## How It Works: Theory

EDT works on two underlying architectural principles.

**"Pass-Through" Blocks:** The "element" blocks operate on the principle of "passing-through" content properties from the rendering page, which means they show things from the page on which they are currently being displayed. You could embed the same `HeadingElementBlock` on two different pages, and it would display different things, because at its default, it just "passes-through" the `Name` of the page on which it's being displayed. Put another way, the "content" of the block is a _command_ which effective tells the block to retrieve and display a property from the rendering page.  These blocks extend from `TemplateElementBaseBlock` which provides utility methods to find data from the rendering page.

**Delegation of the Template Property:** The template property that you add in step 6 above will "delegate" its value if it has no value of its own. If there are no blocks in it (or if it's `null`), that property will go looking for a `TemplateBlock` named for the page type in the "Templates" folder. If it finds one, the property will return the `Elements` property from that block as its own value. So, every page that doesn't provide a value for their template property will "fallback" to central template, which is the entire point of templating.

Those two principles are what make EDT work.

There's a third principle which isn't really "architectural," but just a feature hack:

**Template Execution:** EDT includes [Fluid](https://github.com/sebastienros/fluid) integration, with automatic content injection. An element block can process its output as a Fluid template, and content from the currently rendering page is injected into the context. This can be used to modify the output, or to provide a `ShowIf` expression to dictate whether the block is shown at all.

## How it Works: Process

Using the demo created above, here is a step-by-step description of what's happening when a page is being rendered.

When execution gets to the view of `ArticlePage` it encounters the `Template` property.

Retrieving that property value will happen normally, if the property has a value. That value could be actual blocks, or it could be a `TemplateBlock` which contains an `Elements` content area. The view for `TemplateBlock` just renders the `Elements` content area...which is a roundabout way of saying this will render exactly how you expect -- the `TemplateBlock` will recurse down through its blocks and render each one.

If the `Template` property of the page has no value, it will use the `IDynamicTemplateResolver` service to try and find a template (the logic for evaluating the local value is bundled up in here too). The default service implementation will look for a "Templates" folder in the root of the asset tree. If it finds that, it will look in it for a `TemplateBlock` object with a name that matches the page type name ("ArticlePage" in this case). If it finds that, the service will return its `Elements` property which will then be returned from the `Template` property of the page, as if it was its own.

>**TODO:** The logic currently contained in the sample template property should probably be encapsulated in a custom property, rather than a generic `ContentArea`. If this happened, we might be able to create new page types from the UI and template them, which would be an fantastic increase in functionality.

Blocks rendered from this `Template` property (wherever they came from), will operate normally. However, so-called "element blocks" are those which extend from `TemplateElementBaseBlock`. That base class has some utility properties for `RenderingPageLink` and `RenderingPageData` that will give your code access to the page that is rendering -- so the page that block is being displayed on during that specific request. These properties can be used in your view models to power logic that decides where content should come from.

`TemplateElementBaseBlock` also includes a `ShowIf` property into which an editor can place an expression. This expression is evaluated by the templating engine, and it powers a `Show` boolean. If this property returns `false`, the block shouldn't be shown at all. This value can be used in your controller to abort display of the block. (Alternately, you could use it in your view to abort rendering via an early `return` or an `if...then` to modify rendering, but it's probably safer to do this in the controller.)

Inside your block view model, you can access `TemplatingService` which provides methods for processing templates which are injected with content from the rendering page.

>**TODO:** The current default implementation of `TemplatingService` uses Fluid as a template engine. This is fine, but it exposes Fluid in the interface (some methods take in a `TemplateContext`, which is Fluid-specific). This defeats the entire purpose of DI. The external interface should be engine-agnostic, so it can be injected with some other implementation.

## Creating a New Element Block

First of all, do you need to create a new type of block? Remember that _all blocks work the same way they always have_. So if you just want to output some content in a template, you can use the default `EditorialBlock` that comes with Alloy. A `TemplateBlock` or `Template` property is basically just a `ContentArea`, and it will output any block as its default behavior.

But an "element block" can be said to be a block that extends from `TemplateElementBaseBlock`. By extending from that base, you gain some extra capabilities:

* `RenderingPageLink`: A `ContentLink` of the page on which the block is embedded
* `RenderingPageData`: A `PageData` of the page on which the block is embedded

You can use those two properties to "pass-through" content from the rendering page in your view model logic.

* `Process`: A method that will execute a string against the `TemplatingService` which has been injected with variables representing the properties of the rendering page
* `Show:` A boolean from the evaluation of the `ShowIf` expression that represents whether block should be displayed
* `StyleOutput`: A formatted CSS string from the `Style` property below
* `IsBoilerplate`: A boolean that returns `true` if the block is (1) being shown in Edit Mode, and (2) appears to be embedded on the home page. This can be used in our view to return "boilerplate" content when the template itself is being viewed (i.e. "Lorem Ipsum").

>**TODO:** Clearly, this won't work for templates _actually_ executing on the home page. For now, we're going to just accept this as an edge case.

Here here are some properties which are displayed in the UI:

* `ShowIf`: A template expression that should return true/false (example: `PageCreated | days_ago > 2`). The result of this will be surfaced in the `Show` property which should be used in the block controller to show or hide the block entirely.
* `Style`: A set of CSS style rules. You can write one per line, and they'll be rolled up and formatted in the `StyleOuput` property. It's up to the view to decide where to use this.
* `ClassName`: A CSS classname. This might be used in the view, depending on the element. It's up to the view to decide where to use this.
* `BoilerplateOutput`: Default content to show when the template is being used. If provided, the view for this block should use this when `IsBoilerplate` returns true. It's up to the view to decide where to use this.

Your specific block will likely provide additional properties, specific to whatever it is (i.e. `Align` for the `ImageElementBlock`).

## Where EDT Functionality Comes From

When considering EDT, there are multiple axes on which functionality can grow.

1. **The number and types of elements.** For the POC, four elements were created. There are clearly dozens more which could exist. This is becomes an exercise in codifying design patterns.
2. **The configurability of elements.** Each element is a UI and logic puzzle to determine the best way to codifify all it different permutations as an Episerver block. For each element, there exists an ideal balance between simplicity and flexibility. The trick is finding it.
3. **The variables injected into the template context.** For the POC, the only variables injected are the properties of the rendering page. However, other variables could be injected: the properties of the block itself, some data from the page's ancestors, the username of the active user, a list of the groups they belong to, the data from the `HttpRequest` object, etc.
4. **The custom functions injected into the template context.** For the POC, there are four custom functions: `upper`, `lower`, `format`, and `days_ago`. (It's worth noting that Fluid seems to support [the functions from Liquid](https://shopify.github.io/liquid/) as well.) There are dozens of Episerver-specific functions, tags, and blocks which could be injected into Fluid.

The last two items speak to a larger point: if Fluid has a future as a templating option for Episerver (at any level), there needs to be some conventions developed so that users can expect that certain variables, filters, tags, and blocks are generally available in all Fluid contexts. There needs to be a "canonical Fluid context" which users can generally count on.