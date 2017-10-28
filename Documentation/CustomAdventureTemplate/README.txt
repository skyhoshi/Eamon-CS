
1.  Make copy of CustomAdventureTemplate directory

2.  In the copied directory, rename the directories and files whose names contain *YourAdventureName*

3.  In the copied directory, do find and replace in files on the following (perhaps use Notepad++ or Visual Studio):

	a. YourAdventureName

		Should be replaced with the name of your adventure, suitable for use as a directory or file name (eg, TheSubAquanLaboratory)

	b. YourAuthorName

		Should be replaced with your name (eg, Michael R. Penner)
		
	c. YourAuthorInitials

		Should be replaced with your initials (eg, MRP)

4.  Copy QuickLaunch files into the repository under QuickLaunch

5.  Copy renamed adventure directory into the repository under Adventures

6.  Add a new record to the ADVENTURES.XML file and also the appropriate hierarchical genre-specific file (eg, FANTASY.XML, etc)

	a. You should use EamonDD to do this unless you know how to manually edit these files by hand

7.  Run Visual Studio, open Eamon.Desktop.sln and add your new adventure .csproj file (eg, TheSubAquanLaboratory.csproj) to the Adventures folder

8.  Compile the entire solution - you should now have an adventure .dll file in the System\Bin directory

9.  You can now edit your adventure using EamonDD and play it using EamonMH
