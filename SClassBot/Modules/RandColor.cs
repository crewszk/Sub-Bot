using System;

namespace SClassBot.Modules
{
    public static class RandColor
    {
        private static readonly uint[] colors = new uint[] {0xE629B2, 0xB0A8F5, 0xCEFFB6, 0xE2EB5F, 0xEBBC5F, 0xDF6949,
            0xE92222, 0x76C2BD, 0xA24EBE, 0xE20687};
        private static readonly Random random = new Random();
    
        public static uint NewColor()
        {
            return colors[random.Next(colors.Length)];
        }
    }
}