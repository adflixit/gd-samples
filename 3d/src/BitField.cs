namespace Iso
{
    public struct BitField
    {
        private ulong a, b;

        private static void AssertRange(int index)
        {
            if (index < 0 || index >= 63)
            {
                throw new System.ArgumentOutOfRangeException("index",
                    "Must be in range [0, 63].");
            }
        }

        public BitField(int index)
        {
            AssertRange(index);

            a = 0;
            b = 0;

            if (a <= 31)
            {
                a = 1UL << index;
            }
            else
            {
                b = 1UL << (index % 32);
            }
        }

        public static implicit operator BitField(int index) => new BitField(index);
        public static implicit operator bool(BitField flags) => (flags.a | flags.b) != 0U;

        public static BitField operator &(BitField left, BitField right)
        {
            left.a &= right.a;
            left.b &= right.b;

            return left;
        }

        public static BitField operator |(BitField left, BitField right)
        {
            left.a |= right.a;
            left.b |= right.b;

            return left;
        }

        public static BitField operator &(BitField left, int right)
        {
            AssertRange(right);

            if (right <= 31)
            {
                left.a &= 1UL << right;
            }
            else
            {
                left.b &= 1UL << (right % 32);
            }

            return left;
        }

        public static BitField operator |(BitField left, int right)
        {
            AssertRange(right);

            if (right <= 31)
            {
                left.a |= 1UL << right;
            }
            else
            {
                left.b |= 1UL << (right % 32);
            }

            return left;
        }
    }
}
