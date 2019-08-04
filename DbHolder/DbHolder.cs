using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DbHolder
{
    public class DbHolder
    {
        public void LoadDbFromFile(string path)
        {
            var fileBuf = File.ReadAllBytes(path);
            var header = ByteBufToStruct<DbHeader>(fileBuf);
            var ranges = ByteBufToArrayOfStructs<DbRange>(fileBuf, (int)header.offset_ranges, header.records);
            var cities = ByteBufToArrayOfStructs<DbCity>(fileBuf, (int)header.offset_cities, header.records);

            var locationSize = Marshal.SizeOf<DbLocation>();

            var locations = Enumerable.Range(0, header.records).Select(i => ByteBufToStruct<DbLocation>(fileBuf, (int)(header.offset_locations + i * locationSize))).ToArray();
        }

        private T ByteBufToStruct<T>(byte[] buf, int index = 0) where T : struct
        {
            var handle = GCHandle.Alloc(buf, GCHandleType.Pinned);

            try
            {
                return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject() + index);
            }
            finally
            {
                handle.Free();
            }
        }

        private T[] ByteBufToArrayOfStructs<T>(byte[] buf, int index, int count) where T: struct
        {
            var size = Marshal.SizeOf<T>();
            var localBuf = new T[count];            

            var handle = GCHandle.Alloc(localBuf, GCHandleType.Pinned);

            try
            {
                Marshal.Copy(buf, index, handle.AddrOfPinnedObject(), count * size);

                return localBuf;
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
