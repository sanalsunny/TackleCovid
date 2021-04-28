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

            while (true)
            { 
                var slotManager = new SlotManger();
                await slotManager.MonitorSlot();

                Thread.Sleep(AppContext.interval * 1000 * 60);
            }
        }
    }
}
