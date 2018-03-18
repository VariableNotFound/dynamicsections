# DynamicSections

This component allows you to register markup sections from Razor views, partial views, HTML helpers or any reusable view component and render them in the layout.

The approach is similar to the Razor `@section` directive, but in a more flexible way because you can register and render code sections at runtime, instead of doing it at compilation time.

## Installation

Grab it from Nuget:

```
install-package DynamicSections
```
Also, if you plan to use _tag helpers_ (very recommended!), you must include the following directive in the views or pages you are going to use dynamic sections, or, even better, in your *_ViewImports.cshtml* file so it will be available everywhere:
```
@addTagHelper *, DynamicSections
```

## Quickstart

You can register code blocks from virtually any location of your application (views, partial views, Razor pages, view components, helpers, tag helpers... and even, although it is not recommended, from controllers, filters, or other server components).

From a Razor view or page you can register a code block in the dynamic section called using the following tag helper:
```html
<register-block dynamic-section="scripts" key="test">
    <script>
        console.log('Hi!');
    </script>
</register-block>
```
> You can also use the HTML helper alternative for this: `@Html.RegisterBlock("scripts", "test", "<script>console.log('Hi!');</script>")`.

The attribute `dynamic-section` specifies the name of the section where this code block is being registered, in the same way you do with '@section` in Razor. You can define and use all the sections you need in your application.

> If this attribute is omitted, a default (`String.Empty`) section name will be used.

The attribute `key` is used as unique identifier for this block into the dynamic section. So, if you add a new block with the same key and section from another point of your application, it will be overwritten. Obviously, you can register all the blocks you need in your applications and containing any kind  of code (scripts, CSS, HTML, etc.)

Later on, you can render the sections using the `<dynamic-section>` tag helper. For example, the following code shows to do it from the site's layout, although we could use the tag helper from any point:
```html
@* For example, in _Layout.cshtml *@
<html>
    <body>
        ...
        <dynamic-section name="scripts" />
    </body>
</html>
```
This code will render all the code blocks registered in the section called "scripts". In our scenario, this will generate the following output:
```html
<html>
    <body>
        ...
        <script>
            console.log('Hi!');
        </script>        
    </body>
</html>
```
> You can also use the HTML helper alternative `@Html.DynamicSection()`.
## FAQs

### 1. Can I render all the registered dynamic sections, instead of doing it one by one?

Sure, you can just use "*" as section name when rendering:
```html
<dynamic-section name="*"></dynamic-section>
```

### 2. By default, when I render a dynamic section it is removed from memory. Can I keep it so I can render it later?
This behavior is by design, to avoid rendering the same section more than once. But you can avoid it by setting to `false` the optional attribute `remove`, like in the following example:
```html
<dynamic-section name="*" remove="false"></dynamic-section>
```
Or, using the HTML helper alternative:
```html
@Html.DynamicSection()
```

### 3. Instead of rendering a section into a view, could I render it to a string to preprocess it before adding the content to the page?
You can do it using the helper `@Html.DynamicSection()` as follows:
```html
@{
    var result = Html.DynamicSection("*", remove: false).ToString().Replace("e", "E");
    @result
}
```

### 4. Can I register blocks from a controller? Or from a filter?
First, don't do it: these kind of components belongs to the Controller layer and should not register frontend code. But if you had to do it, you could use the `HttpContext` extension method like in the following MVC action:
```cs
public IActionResult Index()
{
    HttpContext.RegisterBlock("scripts", "home-index", "<!-- Hello from the controller! -->");
    return View();
}
```
This way you'll be able to register blocks wherever you have a `HttpContext` instance.

### 5. I have a partial view used several times in a page, and I want it to register a common code for all the instances but also a specific code for each instance. How can I do it?

Easy, just use the same block key for the common code, and a different key for each instance, like in the following example:

```html
@{
    var id = "c" + DateTime.Now.Ticks;
}

<div id="@id" style="border: 1px solid #666; padding: 10px; margin: 10px; background-color: #fafafa">
    <h3>MyPartialView</h3>
    <p>Hello from my partial view instance @id</p>
</div>

<register-block dynamic-section="scripts" key="my-partial-view-common">
    <!-- Common initialization code for MyPartialView (generated only once) -->
    <script>
        console.log("Initialization code for MyPartialView");
    </script>
</register-block>

<register-block dynamic-section="scripts" key="my-partial-view-@id">
    <!-- Initialization code for MyPartialView @id  -->
    <script>
        console.log("Initialization code for instance '@id'");
    </script>
</register-block>
```
