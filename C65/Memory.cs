using System;
using System.Collections.Generic;
using System.Text;

namespace C65
{
    public class Memory
    {
		public C65IO MemoryIO = null;

		public byte[] GetMemory
		{
			get
			{
				return memory;
			}
		}
		private byte[] memory = new byte[65536];

		public Memory(ref C65IO IO)
		{
			MemoryIO = IO;
		}

		public byte Read(ushort location)
		{
			return memory[location];
		}

		public void Write(ushort location, byte data)
		{
			memory[location] = data;
		}
    }
}
