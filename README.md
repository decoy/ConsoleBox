# ConsoleBox #

ConsoleBox is something of a proof of concept project for creating a UI framework for console applications under dotnet core.  Somewhere between [termbox](https://github.com/nsf/termbox), [blessed](https://github.com/chjj/blessed), and [ncurses](https://www.gnu.org/software/ncurses/) in scope.


## What it does

ConsoleBox consists of three layers.

At the 'core', it has the same scope of termbox:  
 * Providing a way to write individual 'cells' of a console, treating it as a grid. 
 * Event management for keyboard and mouse inputs.
 * And standardizing the API for the above across platforms.
 * It also consists of a 'buffer' class that can be written to instead of writing directly to the console, allowing updates to be batched, and only changed cells to be written.

The second layer consists of the "Elements" which provide a two step way of drawing things onto this grid, similar to WPF.  Every element can 'measure' the space it will take within a constraint, and then actually draw into that space.  Parent elements can use the measure function of children to calculate layouts.

The third layer consists of "Widgets".  These wrap the elements and help with managing UI state.  You don't usually think about it, but elements like HTML inputs require state to be maintained between keystrokes:  the previously written text, the cursor position, etc.  Widgets are similar to "Components" in React.

In action:

![Console Box](https://decoy.github.com/consolebox/consolebox.gif)


## What it would take to be useable

There are two main issues with the framework as is:

First, the API is not 'complete'.  It was originally inspired by React and Flutter, but because it doesn't implement an element tree, a lot of the concepts used by those frameworks don't work.  At the time this was intentional, but a few more iterations would be necessary to stabilize this API.

More importantly, the cross-platform access to the more advanced console API is just not there in .NET core.  The current POC uses a bunch of hacks to make it 'work' on the various platforms, but there are lots of holes.

Unless the System.Console API is expanded in future releases of .NET, the best path forward would be to create a native library for each platform to act as an interoperability layer.  (Or maybe just wrap termbox)

## What next

Creating that interop layer for various platforms was out of scope of what I wanted for this project, so it's just published here for reference.

Feel free to peruse the source code for inspiration, or send a pull request if you want to see it grow!