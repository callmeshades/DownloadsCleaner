using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace DownloadsCleaner
{
    class Downloads
    {
        private readonly HostDetails hostDetails;
        private List<string> folderNames;
        private JObject jsonFiles;

        public Downloads()
        {
            hostDetails = new HostDetails();
            ReadJsonFile();
            hostDetails.ChangeWorkingPath(); // Change to the downloads folder
            CreateAllFolders();
            MoveAllFiles();
            Console.Write("Press any key to continue...");
            Console.ReadKey();
        }

        // Function is used for reading all information about the JSON file
        // and storing it in memory for organizing folders
        private void ReadJsonFile()
        {
            folderNames = new List<string>(); // Create the list

            string filePath = "./FileTypes.json"; // JSON filepath

            // Using ensures the StreamReader is automatically closed
            // once finished reading all information
            using (var r = new StreamReader(filePath))
            {
                var json = r.ReadToEnd();
                jsonFiles = JObject.Parse(json); // Parse the Json file

                // Get all names for the folders required in
                // the Downloads folder
                foreach (var item in jsonFiles)
                {
                    folderNames.Add(item.Key);
                }
            }
        }

        private void CreateAllFolders()
        {
            // Get all folders in the Downloads directory
            // return only the names of the subfolders not the
            // full path
            Console.WriteLine("Search for and creating all folders!");
            var currentDir = Directory.GetCurrentDirectory(); // To prevent calling multiple times in loop
            foreach (var subdirectory in folderNames)
            {
                var di = new DirectoryInfo($@"{currentDir}\{subdirectory}");
                // Check if the directory exists
                // if it doesn't create it
                if (!di.Exists)
                {
                    di.Create();
                }
            }
        }

        // Main loop for moving all files
        // Calls other methods to complete this operation
        private void MoveAllFiles()
        {
            var currentDir = Directory.GetCurrentDirectory(); // To prevent calling the directory multiple times again
            string[] allFiles = Directory.GetFiles(currentDir);
            foreach (var item in allFiles)
            {
                var currentFileType = item.Split(".");
                CheckMatchingFiletype(item, currentFileType[currentFileType.Length - 1]);
            }
        }

        // Used for going through all JSON objects and their key values
        // Moves items based on children
        private void CheckMatchingFiletype(string currentFilesPath, string currentFilesType)
        {
            // Go through all current parent JSON objects
            // Parent JSON key
            foreach (var allFileTypes in jsonFiles)
            {
                // Go through the current JSON objects children filenames
                // All this information is found in FileTypes.json
                foreach (var childrenFileType in jsonFiles[allFileTypes.Key])
                {
                    // If the current children key (string of text is say "png")
                    // and equal to the current filetype for the selected file
                    // Move that item based on the parent JSON key 
                    if (childrenFileType.ToString().ToLower() == currentFilesType)
                    {
                        // Will be used by the MoveFileByItemType method
                        MoveFileByItemType(currentFilesPath, allFileTypes.Key);
                    }
                }
            }
        }

        // Actually moves the item to the respectful parent folder location
        private void MoveFileByItemType(JToken selectedFile, string itemType)
        {
            // Get the current file name without the path
            var shorthandName = Path.GetFileName(selectedFile.ToString());
            // Builds the new filepath for the item
            var newFilePath = @$"{hostDetails.ReturnDownloadsPath()}\{itemType}\{shorthandName}";

            // Moves the file and logs it to the console
            try
            {
                File.Move(selectedFile.ToString(), newFilePath);
                Console.WriteLine($"Moved {shorthandName} to {newFilePath}");
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {shorthandName} - Reason: {ex.Message}");
            }
        }
    }
}
