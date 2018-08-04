# Eamon CS
### The Wonderful World of Eamon (C# Branch)

#### Note: the Wiki is now serving as a development log; it will be updated periodically with the current project status.

#### Last Wiki Update 20180707

This is Eamon CS (ECS), a C# port of the classic Eamon roleplaying game that debuted on the Apple II.  Eamon was created by Donald Brown, but there have been many versions over the years, on a variety of computer systems.  ECS is the production version of Eamon AC (EAC), a prototype intended to extract the game from BASIC.  EAC has been obsoleted in favor of this Eamon, which hopefully will be the definitive version for the C family of languages.

#### Prerequisites

Eamon CS requires a .NET Standard 2.0 compliant runtime; for example, .NET 4.6.1+ on Windows and Mono 5.2.0+ on Unix.  The .NET Core 2.0+ runtime or SDK (for developers) is also required.  All modern Windows platforms come with the latest .NET runtime installed, but for Unix you may have to do a manual Mono install depending on your distribution.

Eamon CS Mobile currently runs on devices using Android 4.0 through 7.1.

#### Installing

There is no formal installer for Eamon CS.  To obtain a copy of this repository (which contains a full set of binaries) you can either do a Git Clone using Visual Studio 2017 or, more simply, download a .zip file using the green Clone Or Download button above.  (To obtain and install Eamon CS Mobile, take a look at the Wiki Development Log entry dated 20170613.)

If you are on Windows and choose the second option, prior to unzipping the file, you should right click on it, select Properties and click the Unblock check box (or button) in the lower right corner of the form.  Then click Apply and OK.  This will improve the gameplay experience by eliminating security warning message boxes.

It is possible to have multiple ECS repositories present on your system; provided they are in separate directories, they will not interfere with each other.  This can be useful in upgrade scenarios.

#### Playing

ECS programs are launched using a collection of batch files (or shell scripts in Unix) that are located under the QuickLaunch directory.  You can create a shortcut to this folder on your desktop for easy access to the system.

ECS Mobile mirrors the hierarchical directory structure of ECS Desktop, making the experience very similar.

#### Contributing

Like all Eamons, ECS allows you to create adventures with no programming involved, via the EamonDD data file editor.  But for the intrepid game designer, the engine is infinitely extensible, using typical C# subclassing mechanisms.  The documentation at this point is improving and there are multiple example adventures that can be recompiled in Debug mode and stepped through to gain a better understanding of the system.  The WorkingDraft.pdf file goes through this in more detail.

If you are interested in contributing to the Eamon CS project, or you wish to port your own game, or build a new one, please contact me.  I can provide insight if there are areas of the code that need clarification.  Eamon has always been an ideal programmer's learning tool; if you build a game you aren't just contributing to the system, you're honing your skills as a C# developer while having fun doing it!

#### Roadmap

The current plan is to produce fully polished games as time allows.  If you have an old BASIC game that you'd like to see ported and are willing to assist in that task (just through your insight) you'll get priority.  Otherwise, the emphasis here is quality over quantity.

There are currently plans to port Eamon CS Mobile to iOS.

There are many 3rd party technologies that can seamlessly integrate with ECS, some of which may push the game in new directions.  Stay tuned and see what comes of it.

#### License

Eamon CS is released as free software under the GNU General Public License.  See LICENSE.txt in the Documentation directory for more details.

