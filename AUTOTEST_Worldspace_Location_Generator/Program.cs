using System;

class Program
{
    static void Main()
    {
        string worldspace = "";
        int x1 = 0;
        int x2 = 0;
        int y1 = 0;
        int y2 = 0;
        bool useSingleCoordinate = false;
        bool run = true;

        while (run)
        {
            Console.WriteLine("This executable is made to generate the AutoTest_Locations_Run.json to be used with AUTOTEST found on Nexus.\nIt will generate only Worldspace locations.\nRefer to the Nexus page on how to use AUTOTEST and where to put this file.\nThe file will generate wherever you put this executable.\n\nEnter the Worldspace name followed by the first and last X coordinate and the first and last Y coordinate with a space between each entry.\nFirst coordinate must be lower than last coordinate.\nExample = Tamriel -10 10 -10 10.\nYou can also input just a single X and Y coordinate if you only want one cell");

            bool validEntry = false;
            while (!validEntry)
            {
                validEntry = true;
                string entry = Console.ReadLine();
                entry = entry.Trim();

                if (entry.Contains(" ") && entry.Length > 0)
                {
                    worldspace = entry.Substring(0, entry.IndexOf(" "));
                    entry = entry.Substring(entry.IndexOf(" ") + 1);
                    entry = entry.Trim();
                }
                else
                    validEntry = false;

                if (entry.Contains(" ") && entry.Length > 0 && validEntry)
                {
                    try
                    {
                        x1 = Convert.ToInt32(entry.Substring(0, entry.IndexOf(" ")));
                    }
                    catch
                    {
                        validEntry = false;
                    }
                    if (validEntry)
                    {
                        entry = entry.Substring(entry.IndexOf(" ") + 1);
                        entry = entry.Trim();
                    }
                }
                else
                    validEntry = false;

                if (entry.Contains(" ") && entry.Length > 0 && validEntry)
                {
                    try
                    {
                        x2 = Convert.ToInt32(entry.Substring(0, entry.IndexOf(" ")));
                    }
                    catch
                    {
                        validEntry = false;
                    }
                    if (validEntry)
                    {
                        entry = entry.Substring(entry.IndexOf(" ") + 1);
                        entry = entry.Trim();
                    }
                }
                else if (entry.Length > 0 && validEntry)
                {
                    try
                    {
                        y1 = Convert.ToInt32(entry.Substring(0));
                    }
                    catch
                    {
                        validEntry = false;
                    }
                    if (validEntry)
                    {
                        useSingleCoordinate = true;
                    }
                }
                else
                    validEntry = false;

                if (entry.Contains(" ") && entry.Length > 0 && validEntry && !useSingleCoordinate)
                {
                    try
                    {
                        y1 = Convert.ToInt32(entry.Substring(0, entry.IndexOf(" ")));
                    }
                    catch
                    {
                        validEntry = false;
                    }
                    if (validEntry)
                    {
                        entry = entry.Substring(entry.IndexOf(" ") + 1);
                        entry = entry.Trim();
                    }
                }
                else if (!useSingleCoordinate)
                    validEntry = false;

                if (entry.Length > 0 && validEntry && !useSingleCoordinate)
                {
                    try
                    {
                        y2 = Convert.ToInt32(entry.Substring(0));
                    }
                    catch
                    {
                        validEntry = false;
                    }
                }
                else if (!useSingleCoordinate)
                    validEntry = false;

                if (!useSingleCoordinate)
                {
                    if (x1 > x2 || y1 > y2)
                        validEntry = false;
                    if (x1 == x2 && y1 == y2)
                        useSingleCoordinate = true;
                }

                if (!validEntry)
                    Console.WriteLine("Invalid entry");
            }
            try
            {
                Console.WriteLine("Starting..");
                using (System.IO.StreamWriter writer = new System.IO.StreamWriter(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\AutoTest_Locations_Run.json"))
                {
                    writer.WriteLine("{\n\"stringList\": {\n\"tests_to_run\": [");
                    if (useSingleCoordinate)
                    {
                        Console.WriteLine("\"" + worldspace + "/" + x1.ToString() + "/" + y1.ToString() + "\"");
                        writer.WriteLine("\"" + worldspace + "/" + x1.ToString() + "/" + y1.ToString() + "\"");
                    }
                    else
                    {
                        int x = x1;
                        int y = y1;
                        while (true)
                        {
                            if (x == x2 && y == y2)
                            {
                                Console.WriteLine("\"" + worldspace + "/" + x.ToString() + "/" + y.ToString() + "\"");
                                writer.WriteLine("\"" + worldspace + "/" + x.ToString() + "/" + y.ToString() + "\"");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("\"" + worldspace + "/" + x.ToString() + "/" + y.ToString() + "\",");
                                writer.WriteLine("\"" + worldspace + "/" + x.ToString() + "/" + y.ToString() + "\",");
                                if (x == x2)
                                {
                                    y++;
                                    x = x1;
                                }
                                else
                                    x++;
                            }
                        }
                    }
                    writer.WriteLine("]\n}\n}");
                    writer.Close();
                }
                Console.WriteLine("Finished");
            }
            catch
            {
                Console.WriteLine("Failed to write to file. If you already have the file \"AutoTest_Locations_Run.json\" in the same directory as this executable then try deleting it. Otherwise try moving the executable to a different location as Windows may be blocking it in the current directory");
                run = false;
            }
            if(run)
            {
                Console.WriteLine("Run again? (y/n)");
                if (Console.ReadLine().ToLower() != "y")
                    run = false;
            }
        }
    }
}

