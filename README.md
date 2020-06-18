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
