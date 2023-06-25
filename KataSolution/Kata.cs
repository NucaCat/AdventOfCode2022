using System;
using System.Collections.Generic;
using System.Linq;

namespace KataSolution;

public class Kata
{
    public int LongestWPI(int[] hours)
    {
        int wellPerformingThreshold = 8;

        return 1;
    }

    private class HoursQueue
    {
        private static int _wellPerformingThreshold = 8;
        
        private Queue<int> _queue = new();
        private int _tiringCount = 0;
        private int _nonTiringCount = 0;

        public void Add(int hour)
        {
            var isTiring = hour > _wellPerformingThreshold;
            if (isTiring)
                _tiringCount++;

            if (!isTiring)
                _nonTiringCount++;
            
            _queue.Enqueue(hour);
        }
    }
}