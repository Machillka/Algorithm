namespace Algorithms
{
    public static class QuickSelectFuntions
    {

        public static int QuickSelectKthLargest(int[] nums, int k)
        {
            if (nums.Length == 1)
                return nums[0];

            int pivot = nums[nums.Length / 2];

            var lows = new List<int>();
            var highs = new List<int>();
            var sames = new List<int>();

            foreach (var val in nums)
            {
                if (val == pivot) sames.Add(val);
                if (val < pivot) lows.Add(val);
                if (val > pivot) highs.Add(val);
            }

            if (k <= highs.Count)
            {
                // 第 k 大在 highs 中
                return QuickSelectKthLargest(highs.ToArray(), k);
            }
            else if (k <= highs.Count + sames.Count)
            {
                // 第 k 大在 pivots 区间
                return pivot;
            }
            else
            {
                // 第 k 大在 lows 中，递归查找
                return QuickSelectKthLargest(lows.ToArray(), k - highs.Count - sames.Count);
            }
        }
    }
}