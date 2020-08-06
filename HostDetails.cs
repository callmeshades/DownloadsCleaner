using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace DownloadsCleaner
{
    class HostDetails
    {
        private string Username { get; set; }
        private string DownloadsPath { get; set; }
        private string SystemDirectory { get; set; }

        private void GetCurrentUsername()
        {
            Username = Environment.UserName;
        }

        // Fetches the current OS drive
        // EX: The C: drive
        private void GetOperatingSystemDrive()
        {
            SystemDirectory = Path.GetPathRoot(Environment.SystemDirectory);
        }

        // Builds the downloads path of the current drive and user
        private void BuildDownloadsPath()
        {
            DownloadsPath = $"{SystemDirectory}Users\\{Username}\\Downloads";
        }

        // Checks to make sure the operating system is Windows
        // Could support future OS's in the future
        private void CheckOperatingSystem()
        {
            // Check if the OS is currently windows
            // Can support other operating systems in the future
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Console.WriteLine("Currently only Windows is supported! Exiting now.");
                Console.Write("Press any key to continue...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        // Get confirmation from the user the information about
        // their computer is correct before moving on
        private void ConfirmHostDetails()
        {
            Console.WriteLine($"Your username {Username}");
            Console.WriteLine($"Your downloads path: {DownloadsPath}");

            // Need a variable to be able to break from the while loop
            // Improves readability
            bool isCorrect = false;
            while (!isCorrect)
            {
                Console.Write("Is this correct? (y/n): ");
                var isCorrectKey = Console.ReadLine();
                switch (isCorrectKey.ToLower())
                {
                    case "y":
                        Console.WriteLine("Thanks for confirming! Moving on...");
                        isCorrect = true;
                        break;
                    case "n":
                        Console.WriteLine("Okay! Closing the application, please try again.");
                        Console.WriteLine("If the problem still persists please contact support!");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid character(s) entered! Please try again.");
                        break;
                }
            }
        }

        public void ChangeWorkingPath()
        {
            // Change the current program to be in the
            // Downloads folder for the current user & drive
            Directory.SetCurrentDirectory(DownloadsPath);
        }

        // Public version of the DownloadsPath
        // Required to prevent possible altering
        public string ReturnDownloadsPath()
        {
            return DownloadsPath;
        }

        public HostDetails()
        {
            // Grab the current host information before continuing on
            // Will need to implement checking to confirm these details are correct
            Console.WriteLine("Grabbing OS/host information...");
            GetOperatingSystemDrive();
            GetCurrentUsername();
            BuildDownloadsPath();
            CheckOperatingSystem();
            /*ConfirmHostDetails();*/
        }
        
    }
}
