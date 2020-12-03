# PacManager 
Program for extracting and then re-packaging .pac archive files for the purpose of translating Visual Novels. 

### How to Use 

To extract the data from a pac file, use the following command:
* PacManager -u [source .pac file] [destination directory]
If no destination directory is provided, it will create one with the same name as the source .pac file in the same location. All individual files in the source .pac file will be extracted to the destination directory. 

To build a new .pac file using the data, use the following command:
* PacManager -p [source directory] [destination filename] 
If no destination filename is provided, it will create one using the same name as the source directory. All files in the source directory will be added to the new .pac file. 

General use is to run "PacMaster -u srp.pac" to extract all data from the .pac file. Translate the data in the extracted directory. Then run "PacMaster -p srp" to re-package it all into a .pac file to be used in the game. 

### Pac File Format 
A .pac is an archive file, somewhat similar to a zip, merging multiple files together in a single archive. 

* File Count (2 bytes): The number of separate files in the .pac 
* Filename Length (2 bytes): The length of the individual names for the files, in bytes 
* Data Offset (8 bytes): The position in the file where the header ends and the data begins, in bytes  

Next, for each file in the .pac archive 
* Filename (X bytes): The name of the file, without the file extension. Length is determined by the Filename Length value above. For names shorter than the file name length, the unused bytes are all 0x00. The names appear to be using Shift-JIS encoding. 
* Offset (8 bytes): The offset for where the data for this file starts. This is its position in the file, in bytes, relative to the Data Offset value above 
* Length (4 bytes): The length of the file, in bytes 

After this header information, the rest of the .pac is the data for all the files it contains. The data is all stored back to back. 

### Srp File Format 
A .srp file defines a single "scene" in the visual novel. 

* Command Count (4 bytes): The number of separate "commands" in the scene. 

The rest of the file are the individual "commands" for the scene. Commands include changing background, drawing art assets on screen, playing musing, showing text, etc. 
* Length (2 bytes): The length of the command, in bytes 
* Type (2 bytes): An integer representing the type of command. 0 is text related, 1 includes things like choices for the player, 2 is art related, 3 is music/sound, etc. 
* Flags (2 bytes): Appears to be a bit flag. The purpose for the individual bits seems to vary based on the Type above. 
* Data (X bytes): The remainder of the command is data for the command. This includes things like the name of art or sound assets, and the text being written to the screen. 

### Var File Format 
A .var file defines a number of values used by the visual novel. 

The .var file is just a sceries of lines setting variables with no special header:
* Length (2 bytes): The length of the command, in bytes 
* Data (X bytes): Just a string of text that sets a value to a name (Name=Value). This string is in Shift-JIS format. 

Be warned, there might be some unwritten restrictions on these values determined in the code (for example, character limits). There's no way to tell this from the file itself.
