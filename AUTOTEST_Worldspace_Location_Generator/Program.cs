using System;

class Program
{
    static void Main(String[] inputCommands)
    {
        int inputCommandsCount = 0;
        bool strictMode = false;
        string directoryOutput = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        string entry = "";
        string worldspace = "";
        int x1 = 0;
        int x2 = 0;
        int y1 = 0;
        int y2 = 0;
        bool useSingleCoordinate = false;
        bool run = true;
        string[] outputList = new string[0];

        if (inputCommands.Length > 0)
        {
            if (System.IO.Directory.Exists(inputCommands[0]))
            {
                directoryOutput = inputCommands[0].Trim();
                inputCommandsCount++;
                Console.WriteLine("Output folder set to " + directoryOutput + ".\n");
            }
            else
                Console.WriteLine("Directory output is invalid. Using Same directory as executable.\n");
        }

        while (run)
        {
            Console.WriteLine("\nThis executable is made to generate the AutoTest_Locations_Run.json to be used with AUTOTEST found on Nexus.\nIt will generate only Worldspace locations.\nRefer to the Nexus page on how to use AUTOTEST and where to put this file.\n\nType in the Worldspace name followed by the X coordinate you want to start at, the x coordinate you want to end at and do the same for the y coordinates.\nExample = Tamriel -10 10 -10 10\nYou can also just input one x and y coordinate if you only want one cell.\n\nBy default the coordinates will automatically be spread out as cells are loaded in a 5x5 grid.\nIf for some reason you do not want to space out the coordinates you can type in strict and it will switch modes.");

            bool validEntry = false;
            while (!validEntry)
            {
                if (inputCommandsCount < inputCommands.Length)
                {
                    entry = inputCommands[inputCommandsCount].Trim();
                    inputCommandsCount++;
                }
                else
                    entry = Console.ReadLine().Trim();

                if (entry.ToLower() == "strict")
                {
                    strictMode = !strictMode;
                    Console.WriteLine("Strict mode set to " + strictMode.ToString() + ".\n");
                }
                else
                {
                    validEntry = true;

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
                        if (x1 > x2)
                        {
                            int tempX1 = x1;
                            x1 = x2;
                            x2 = tempX1;
                        }
                        if (y1 > y2)
                        {
                            int tempY1 = y1;
                            y1 = y2;
                            y2 = tempY1;
                        }
                        if (x1 == x2 && y1 == y2)
                            useSingleCoordinate = true;
                    }

                    if (!validEntry)
                        Console.WriteLine("Invalid entry.");
                }
            }

            int x = x1;
            int y = y1;
            if (!strictMode)
            {
                x = x + 2;
                while (x > x2)
                    x--;
                y = y + 2;
                while (y > y2)
                    y--;
            }

            while (true)
            {
                string[] tempOutputList = outputList;
                outputList = new string[tempOutputList.Length + 1];
                for (int item = 0; item < tempOutputList.Length; item++)
                    outputList[item] = tempOutputList[item];

                Console.WriteLine("\"" + worldspace + "/" + x.ToString() + "/" + y.ToString() + "\"");
                outputList[outputList.Length - 1] = "\"" + worldspace + "/" + x.ToString() + "/" + y.ToString() + "\"";

                if (!strictMode)
                {
                    if (x + 3 > x2)
                    {
                        if (y + 3 > y2)
                            break;
                        else
                        {
                            y = y + 5;
                            while (y > y2)
                                y--;
                        }
                        x = x1 + 2;
                    }
                    else
                        x = x + 5;
                    while (x > x2)
                        x--;
                }
                else
                {
                    if (x + 1 > x2)
                    {
                        if (y + 1 > y2)
                            break;
                        else
                            y++;
                        x = x1;
                    }
                    else
                        x++;
                }
            }

            while (true)
            {
                Console.WriteLine("\nEnter Number:\n1 Run new entry and add to the current list.\n2 Run new entry and clear current list.\n3 See current list.\n4 Save current list to file, clear current list then run a new entry.\n5(or anything else) Save current list to file and close program.");

                if (inputCommandsCount < inputCommands.Length)
                {
                    entry = inputCommands[inputCommandsCount].Trim();
                    inputCommandsCount++;
                }
                else
                    entry = Console.ReadLine().Trim();

                if (entry == "3")
                {
                    Console.WriteLine("");
                    foreach (string item in outputList)
                        Console.WriteLine(item);
                }
                else
                    break;
            }

            if (entry != "1" && entry != "2")
            {
                bool doneSaving = false;
                while (!doneSaving)
                {
                    doneSaving = true;
                    Console.WriteLine("\nSaving..");
                    try
                    {
                        using (System.IO.StreamWriter writer = new System.IO.StreamWriter(directoryOutput + "\\AutoTest_Locations_Run.json"))
                        {
                            writer.WriteLine("{\n\"stringList\": {\n\"tests_to_run\": [");
                            for (int item = 0; item < outputList.Length; item++)
                            {
                                if (item == outputList.Length - 1)
                                    writer.WriteLine(outputList[item]);
                                else
                                    writer.WriteLine(outputList[item] + ",");
                            }
                            writer.WriteLine("]\n}\n}");
                            writer.Close();
                        }
                        Console.WriteLine("Done saving!");
                    }
                    catch
                    {
                        while (true)
                        {
                            Console.WriteLine("\nFailed to write to file.\nIf you already have the file \"AutoTest_Locations_Run.json\" in the output directory then try deleting it.\nOtherwise try using a different directory as Windows may be blocking it in the current directory.\n\nTry again?(y/n) or type in d to set a new directory.");
                            string errEntry = Console.ReadLine().Trim().ToLower();
                            if (errEntry != "n" && errEntry != "d")
                            {
                                doneSaving = false;
                                break;
                            }

                            if (errEntry == "n")
                                break;

                            if (errEntry == "d")
                            {
                                Console.WriteLine("\nType in your new directory.");
                                errEntry = Console.ReadLine().Trim().ToLower();
                                if (System.IO.Directory.Exists(errEntry))
                                {
                                    directoryOutput = errEntry;
                                    doneSaving = false;
                                    break;
                                }
                                else
                                    Console.WriteLine("\nDirectory not found.");
                            }
                        }
                    }
                }
            }

            if (entry == "2" || entry == "4")
                outputList = new string[0];

            if (entry != "1" && entry != "2" && entry != "4")
                run = false;
            else
            {
                x2 = 0;
                y2 = 0;
                useSingleCoordinate = false;
            }
        }
    }
}


