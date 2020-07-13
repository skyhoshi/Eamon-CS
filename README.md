# Eamon CS
### The Wonderful World of Eamon (C# Branch)

#### Note: the Wiki is now serving as a development log; it will be updated periodically with the current project status.

#### Last Wiki Update 20200101

This is Eamon CS (ECS), a C# port of the classic Eamon roleplaying game that debuted on the Apple II.  Created initially by Donald Brown, there have been many variants over the years, on a variety of computer systems.  ECS is the production version of Eamon AC (EAC), a prototype intended to extract the game from BASIC.  Hopefully, this Eamon will be the definitive version for the C family of languages, as EAC is obsolete.

#### Prerequisites

Eamon CS requires a .NET Standard 2.0 compliant runtime; for example, .NET 4.7.2+ on Windows and Mono 5.18.0+ on Unix.  Also needed is the .NET Core 2.X runtime or SDK (for developers).  All modern Windows platforms come with the latest .NET runtime installed, but for Unix, you may have to do a manual Mono install depending on your distribution.

Eamon CS Mobile currently runs on devices using Android 4.0 through 8.1.

#### Installing

There is no formal installer for Eamon CS.  You can obtain a copy of this repository (and a full set of binaries) in one of two ways.  Either do a Git Clone using Visual Studio 2017+ or, more simply, download a .zip file using the green Code button above.  (To obtain and install Eamon CS Mobile, look at the Wiki Development Log entry dated 20170613.)

If you are on Windows and choose the second option, before unzipping the file, you should right-click on it.  Then select Properties and click the Unblock checkbox (or button) in the lower right corner of the form.  Finally, click Apply and OK.  The gameplay experience will improve by eliminating security warning message boxes.

It is possible to have multiple ECS repositories present on your system.  They will not interfere with each other, provided they are in separate directories.  It can be useful in upgrade scenarios.

#### Playing

ECS programs are launched using a collection of batch files (or shell scripts in Unix) located under the QuickLaunch directory.  You can create a shortcut to this folder on your desktop for easy access to the system.

ECS Mobile mirrors the hierarchical directory structure of ECS Desktop, making the experience very similar.

#### Contributing

Like all Eamons, ECS allows you to create adventures with no programming involved, via the EamonDD data file editor.  But for the intrepid game designer, the engine is infinitely extensible, using typical C# subclassing mechanisms.  The documentation has improved, and many adventures can be recompiled in Debug mode and stepped through to gain a better understanding of the system.  See WorkingDraft under DOCUMENT LINKS on the Eamon CS website for more details.

If you are interested in contributing to the Eamon CS project, or you wish to port your own game or build a new one, please contact me.  I can provide insight if there are areas of the code that need clarification.  Eamon has always been an ideal programmer's learning tool.  If you build a game, you aren't just contributing to the system; you're honing your skills as a C# developer while having fun doing it!

#### Roadmap

The current plan is to produce fully polished games as time allows.  If you have an old BASIC game that you'd like to see ported and are willing to assist in that task (just through your insight), you'll get priority.  Otherwise, the emphasis is quality over quantity.

There are currently plans to port Eamon CS Mobile to iOS.

Many 3rd party technologies can seamlessly integrate with ECS, some of which may push the game in new directions.  Stay tuned and see what comes of it.

#### License

Eamon CS is free software released under the GNU General Public License.  See LICENSE under DOCUMENT LINKS on the Eamon CS website for more details.

