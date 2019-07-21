using System;
using System.IO;

namespace BestCombo
{
    public class AthleteSort
    {
        private string[] lines;
        private double[] times;
        private double[] costs;
        private string[] names;

        private int maximumCost = 0;
        public int MaximumCost
        {
            get
            {
                if (maximumCost > 0)
                    return maximumCost;
                else
                    throw new Exception("Error: Maxium cost must be greater than zero");
            }
            private set
            {
                if (value > 0)
                    maximumCost = value;
                else
                    throw new Exception("Error: Maxium cost must be greater than zero");
            }
        }

        private int teamSize = 0;
        public int TeamSize
        {
            get
            {
                if ((teamSize > 0) && (teamSize < athleteCount))
                    return teamSize;
                else
                    throw new Exception("Error: Team size must be greater than zero and less than the total number of athletes");
            }
            private set
            {
                if ((value > 0) && (value < athleteCount))
                    teamSize = value;
                else
                    throw new Exception("Error: Team size must be greater than zero and less than the total number of athletes");
            }
        }

        private int athleteCount = 0;
        public int AthleteCount
        {
            get
            {
                if (athleteCount > 0)
                    return athleteCount;
                else
                    throw new Exception("Error: Athlete count must be greater than zero");
            }
            private set
            {
                if (value > 0)
                    athleteCount = value;
                else
                    throw new Exception("Error: Athlete count must be greater than zero");
            }
        }

        public AthleteSort(string fileName, int maxCost, int targetSize)
        {
            if (File.Exists(fileName))
            {
                lines = File.ReadAllLines(fileName);
                times = new double[lines.Length];
                costs = new double[lines.Length];
                names = new string[lines.Length];

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

                AthleteCount = lines.Length;
            }
            else
                throw new Exception("Error: File does not exist");

            MaximumCost = maxCost;
            TeamSize = targetSize;
        }

        public void CreateCombos(TopTeamsQueue ttQueue)
        {
            int[] loop_Count = new int[TeamSize];
            for (int loop = 0; loop < TeamSize; loop++)
                loop_Count[loop] = loop;

            int posIncrement = TeamSize - 1;
            bool done = false;

            while (!done)
            {
                AthleteCombo tempCombo = new AthleteCombo(TeamSize);

                for (int loop = 0; loop < TeamSize; loop++)
                    tempCombo.AddAthlete(loop, names[loop_Count[loop]], times[loop_Count[loop]], costs[loop_Count[loop]]);

                if (tempCombo.TotalCost <= MaximumCost)
                    if (tempCombo.TotalTime <= ttQueue.GetSlowestTime())
                        ttQueue.Insert(tempCombo);

                bool doneIncement = false;
                while (!doneIncement)
                {
                    if (loop_Count[posIncrement] < (AthleteCount - TeamSize + posIncrement))
                    {
                        loop_Count[posIncrement]++;
                        for (int loop = posIncrement + 1; loop < TeamSize; loop++)
                            loop_Count[loop] = loop_Count[loop - 1] + 1;
                        posIncrement = TeamSize - 1;
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

        }
    }

    public class TopTeamsQueue
    {
        private static int maxQueueSize = 100;
        private AthleteCombo[] teams;
        private int queueSize;
        private int queueFillLevel;

        public TopTeamsQueue(int tQueueSize)
        {
            if (tQueueSize < maxQueueSize)
            {
                queueSize = tQueueSize;
                teams = new AthleteCombo[queueSize];
                queueFillLevel = 0;
            }
            else
                throw new Exception("Error: Queue size is too large");
        }

        public double GetSlowestTime()
        {
            double retVal = double.MaxValue;
            if (queueFillLevel == queueSize)
                retVal = teams[queueSize - 1].TotalTime;
            return retVal;
        }

        public void Insert(AthleteCombo combo)
        {
            if (queueFillLevel == 0)
            {
                teams[0] = combo;
                if (queueFillLevel < queueSize)
                    queueFillLevel++;
            }
            else
            {
                int loop = 0;
                bool done = false;
                while ((loop < queueFillLevel) && (!done))
                {
                    if (teams[loop].TotalTime < combo.TotalTime)
                        loop++;
                    else
                        if ((teams[loop].TotalTime == combo.TotalTime) && (teams[loop].TotalCost < combo.TotalCost))
                            loop++;
                        else
                            done = true;
                }

                int startPos = 0;
                if (queueFillLevel < queueSize)
                    startPos = queueFillLevel;
                else
                    startPos = queueFillLevel - 1;

                    // I'm not sure about this for loop.  queueLevel should be changed after the insertion.  I'm adding it here as when revLoop = queueFillLevel (removing the -1) then
                    // the code breaks when the queue is full.  So it should be -1.  But If the queue isn't full, then it's not going far enough in the loop (i.e. +1 from current) to shift
                    // existing entries down in the queue.  
                    // For example: 1 3 6 8 9 <- Insert a 4.  This requires queueFillLevel -1 so that 9 becomes 8, 8 becomes 6, and 6 receives the 4 yeilding 1 3 4 6 8
                    // However, if the queue is 1 3 6 X X <- Insert a 4. If queueFillLevel (3-1) makes the 6 a 4, but the 6 then vanishes instead of taking the first blank X's spot.
                    // Not sure how to reconcile this cleanly yet.
                    //
                    // Added the variable startPos to make a determination on where to start the loop based on the above two scenarios.  Not sure if this fully works yet though.
                    // It *definitely* needs more testing.
                    //

                for (int revLoop = startPos; revLoop > loop; revLoop--)
                    teams[revLoop] = teams[revLoop - 1];
                teams[loop] = combo;

                if (queueFillLevel < queueSize)
                    queueFillLevel++;
            }
        }

        public override string ToString()
        {
            string retVal = "";
            for (int loop = 0; loop < queueFillLevel; loop++)
            {
                retVal += teams[loop].ToString() + "-----------------------------------------\nTotal Cost: $" + teams[loop].TotalCost + "\nTotal Time: " + teams[loop].TotalTime + "\n\n";
            }
            return retVal;
        }
    }

    public class AthleteCombo
    {
        private int teamSize = 0;
        private int[] positions;
        private double[] times;
        private double[] costs;
        private string[] names;

        private int count = 0;

        public AthleteCombo(int numberAthletes)
        {
            teamSize = numberAthletes;

            positions = new int[teamSize];
            times = new double[teamSize];
            costs = new double[teamSize];
            names = new string[teamSize];
        }

        public bool IsTeamFull()
        {
            if (count == teamSize)
                return true;
            else
                return false;
        }

        public void AddAthlete(int position, string name, double time, double cost)
        {
            if (!(IsTeamFull()))
            {
                positions[count] = position;
                times[count] = time;
                costs[count] = cost;
                names[count] = name;

                count++;
            }
            else
                throw new Exception("Error: Too many athletes have been added to the team");
        }

        public double TotalCost
        {
            get
            {
                if (IsTeamFull())
                {
                    double tempCost = 0;
                    for (int loop = 0; loop < teamSize; loop++)
                        tempCost += costs[loop];
                    return tempCost;
                }
                else
                    throw new Exception("Error: The team does not have enough athletes");
            }
        }

        public double TotalTime
        {
            get
            {
                if (IsTeamFull())
                {
                    double tempTime = 0;
                    for (int loop = 0; loop < teamSize; loop++)
                        tempTime += times[loop];
                    return tempTime;
                }
                else
                    throw new Exception("Error: The team does not have enough athletes");
            }
        }

        public override string ToString()
        {
            if (IsTeamFull())
            {
                string retVal = "";
                for (int loop = 0; loop < teamSize; loop++)
                {
                    retVal += names[loop] + ": " + times[loop] + " @ $" + costs[loop] + "\n";
                }
                return retVal;
            }
            else
                throw new Exception("Error: The team does not have enough athletes");
        }
    }
}
