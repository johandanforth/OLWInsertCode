Open Live Writer paste code from VS
===

_Pasting code copied from Visual Studio into Open Live Writer_

Introduction
---

There are a couple of ways to show syntax-highlighted code in your blog posts. The majority of solutions use some special JavaScript library that is loaded along with the page, and this styles your code `<pre>` blocks in the post client-side. All well and good, but unless you mess around with the CSS you're stuck with the color choices made by the JavaScript library. 

At the time I wrote this, I just wanted to mimic the color choices I'd set in Visual Studio. The nice thing about Visual Studio is that if you copy some code to the clipboard, it also adds an RTF (rich text format) version of the code alongside the plain text version. That RTF version contains all the font colors you'd set in Visual Studio, encoded as part of the RTF. This plug-in will look for an RTF encoding of some code in the clipboard and convert it to HTML with `<span>` elements galore. It also gets wrapped in a class-named `<pre>` code block so you can set some general CSS for all your code blocks in your posts.


Using with Open Live Writer
===

Compiling the code with Visual Studio will produce a DLL called `OLWInsertCode.dll`. Make sure you are not running Open Live Writer, then copy this DLL to the `Plugins` folder off from the installation folder for Open Live Writer (you might need to create it if this is the first plugin you're using). Restart Open Live Writer.

To paste some code, highlight and copy it from Visual Studio. Switch to Open Live Writer, go to the Insert tab, and look in the Plug-ins ribbon group. Select Paste Code from VS and the code will be inserted into the document, as formatted in Visual Studio.

Code block format
===

The code will be wrapped in a special code block like this:

`<div class=\"jmbcodeblock\"><pre>` _your formatted code_ `</pre></div>`

You can then style both the `.jmbcodeblock` and `.jmbcodeblock pre` selectors as you wish. As an example, in my main blog's CSS I use:

    pre, code { 
        font-family: Inconsolata, Consolas, "Courier New", Courier, monospace; 
    }

    .jmbcodeblock pre {
        font-size: 16px;
        line-height: 1.1;
        color: Black; 
        background: White;
        border: 1px solid #5F5F5F;
        overflow: auto;
        margin-bottom: 20px;
        padding: 10px 20px;
        max-height: 400px;
    }


