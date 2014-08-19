namespace Frost.Extensions
{
    public static class IntegerExtensions
    {
        public static int ToKiloBytes(this int value)
        {
            return value * 1024;
        }
        public static int ToMegabytes(this int value)
        {
            return value.ToKiloBytes() * 1024;
        }

        public static int ToGigaBytes(this int value)
        {
            return value.ToMegabytes() * 1024;
        }

        public static long ToTerraBytes(this int value)
        {
            return (long)value.ToGigaBytes() * (long)1024;
        }
    }
}