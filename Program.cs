using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BestCombo
{
    class Program
    {
        static void Main(string[] args)
        {
            AthleteSort bestAthleteCombo = new AthleteSort("c:\\Users\\rhysz\\OneDrive\\Documents\\Coaching\\BestCombo\\BestCombo\\Sample-New.txt", 100, 3);
            TopTeamsQueue ttQueue = new TopTeamsQueue(5);
            bestAthleteCombo.CreateCombos(ttQueue);
            Console.WriteLine(ttQueue.ToString());
            Console.WriteLine("\n");
            /*
            if (args.Length == 3)
            {
                int athleteCount = int.Parse(args[1]);
                double maxCost = double.Parse(args[2]);
                string[] lines = File.ReadAllLines(args[0]);
                double[] times = new double[lines.Length];
                double[] costs = new double[lines.Length];
                string[] names = new string[lines.Length];

                for (int i = 0; i < lines.Length; i++)
                {
                    string[] values = lines[i].Split('\t');
                    if (values.Length > 2)
                    {
                        costs[i] = double.Parse(values[0]);
                        times[i] = double.Parse(values[1]);
                        names[i] = values[2];
                    }
                }

                double lowTime = 1000.0;
                double lowCost = 0.0;
                int[] low_Values = new int[athleteCount];
                int[] loop_Count = new int[athleteCount];
                for (int loop = 0; loop < athleteCount; loop++)
                {
                    low_Values[loop] = -1;
                    loop_Count[loop] = loop;
                }
                int posIncrement = athleteCount - 1;
                bool done = false;
                int comparisons = 0;

                while (!done)
                {
                    double tempCost = 0;
                    double tempTime = 0;
                    comparisons++;
                    for (int loop = 0; loop < athleteCount; loop++)
                    {
                        tempCost = tempCost + costs[loop_Count[loop]];
                        tempTime = tempTime + times[loop_Count[loop]];
                    }
                    if (tempCost <= maxCost)
                    {
                        if (tempTime <= lowTime)
                        {
                            lowTime = tempTime;
                            lowCost = tempCost;
                            for (int loop = 0; loop < athleteCount; loop++)
                                low_Values[loop] = loop_Count[loop];
                        }
                    }
                    bool doneIncement = false;
                    while (!doneIncement)
                    {
                        if (loop_Count[posIncrement] < (lines.Length - athleteCount + posIncrement))
                        {
                            loop_Count[posIncrement]++;
                            for (int loop = posIncrement + 1; loop < athleteCount; loop++)
                            {
                                loop_Count[loop] = loop_Count[loop - 1] + 1;
                            }
                            posIncrement = athleteCount - 1;
                            doneIncement = true;
                        }
                        else
                        {
                            posIncrement--;
                            if (posIncrement < 0)
                            {
                                doneIncement = true;
                                done = true;
                            }
                        }
                    }
                }
                Console.WriteLine("Results:");
                Console.WriteLine("Low Time: " + lowTime.ToString());
                Console.WriteLine("Low Cost: " + lowCost.ToString());
                for (int loop = 0; loop < athleteCount; loop++)
                {
                    Console.WriteLine("Athlete " + (loop + 1).ToString() + ":" + names[low_Values[loop]].ToString() + ", " + times[low_Values[loop]].ToString() + ", " + costs[low_Values[loop]].ToString());
                }
            }
            else
            {
                Console.WriteLine("Usage\nBestCombo.exe <Input File> <Number of Picks> <Maximum Cost>");
            }
            */
        }
    }
}
