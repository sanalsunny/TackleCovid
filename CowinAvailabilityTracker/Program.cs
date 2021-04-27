using CowinAvailabilityTracker.Mangers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CowinAvailabilityTracker
{
    public class Program
    {
        public static async Task Main()
        {
            Console.WriteLine("Fetching Details ...");

            while (1 == 1)
            { 
                var slotManager = new SlotManger();
                await slotManager.FindSlot();

                Thread.Sleep(AppContext.interval * 1000 * 60);
            }
        }

    }
}