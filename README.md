# URM Simulator
URM Simulator is an unlimited register machine simulator. For more information on UR Machine please read: https://proofwiki.org/wiki/Definition:Unlimited_Register_Machine

Getting Started
---------------

Premade binaries for Windows and Linux and examples are available at: https://mega.nz/#F!DJAQ1LSR!lIWyNeUTuxyfqI_1pAjIEQ

A video of application in use is also available at: https://youtu.be/3PyiAAIsqzo

Building from Source
---------------

To get the source code do the following in your git client:
```
git clone https://github.com/cra0zy/URMSimulator.git
git submodule update --init --recursive
```

Open URMSimulator.sln with MonoDevelop (or VisualStudio/Xamarin Studio), and build the project for your platform:
 - URMSimulator.Gtk2 - Linux, using Gtk 2 as GUI toolkit
 - URMSimulator.Gtk3 - Linux, using Gtk 3 as GUI toolkit, make sure you have newest gtk-sharp made from source: https://github.com/mono/gtk-sharp.git
 - URMSimulator.Wpf - Windows
