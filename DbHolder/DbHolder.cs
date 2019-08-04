using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DbHolder
{
    static class ExtendBinaryReader
    {
        public static string ReadFixedString(this BinaryReader reader, int len)
        {
            var buffer = new byte[len];

            reader.Read(buffer, 0, len);

            return Encoding.ASCII.GetString(buffer.TakeWhile(x => x != 0).ToArray());
        }
    }

    public class DbHolder
    {
        public static int RecordsCount;

        Dictionary<object, Func<BinaryReader, object>> propTypeMap = new Dictionary<object, Func<BinaryReader, object>>()
        {
            { typeof(int), r => r.ReadInt32() },
            { typeof(ulong), r => r.ReadUInt64() },
            { typeof(uint), r => r.ReadUInt32() },
            { typeof(float), r => r.ReadSingle() },
            { typeof(IPAddress), r => new IPAddress(r.ReadUInt32()) },
            { "Name", r => r.ReadFixedString(32) },
            { "Country", r => r.ReadFixedString(8) },
            { "Region", r => r.ReadFixedString(12) },
            { "Postal", r => r.ReadFixedString(12) },
            { "City", r => r.ReadFixedString(24) },
            { "Organization", r => r.ReadFixedString(32) }
        };


        public void LoadDbFromFile(string path)
        {
            var bytes = File.ReadAllBytes(path);

            var header = ByteBufToObj<DbHeader>(bytes);

            var locations = Enumerable.Range(0, header.Records)
                                      .Select(i => ByteBufToObj<DbLocation>(bytes, (int)(header.Offset_locations + i * DbLocation.size), DbLocation.size))
                                      .ToArray();

            var ranges = Enumerable.Range(0, header.Records)
                                   .Select(i => ByteBufToObj<DbRange>(bytes, (int)(header.Offset_ranges + i * DbRange.size), DbRange.size, locations))
                                   .ToArray();

            var cities = Enumerable.Range(0, header.Records)
                                   .Select(i => ByteBufToObj<DbCity>(bytes, (int)(header.Offset_cities + i * 4), 4))
                                   .ToArray();
        }

        private void SetValue<T>(PropertyInfo p, T obj, BinaryReader reader, DbLocation[] locations = null)
        {
            Func<BinaryReader, object> valueGetter = null;

            if (p.Name == "Location")
            {
                var location = locations[(uint)(propTypeMap[typeof(uint)](reader))];

                p.SetValue(obj, location);
            }
            else
            {
                if (!propTypeMap.TryGetValue(p.PropertyType, out valueGetter))
                    valueGetter = propTypeMap[p.Name];

                p.SetValue(obj, valueGetter(reader));
            }
        }

        private T ByteBufToObj<T>(byte[] buf, int index = 0, int count = -1, DbLocation[] locations = null) where T : new()
        {
            using (var reader = new BinaryReader(new MemoryStream(buf, index, count == -1 ? buf.Length : count)))
            {
                var obj = new T();

                obj.GetType().GetProperties().ToList().ForEach(p => SetValue(p, obj, reader, locations));

                return obj;
            }
        }

        private T ByteBufToStruct<T>(byte[] buf) where T : struct
        {
            if (Marshal.SizeOf(typeof(T)) != buf.Length)
                throw new ArgumentOutOfRangeException();

            var handle = GCHandle.Alloc(buf, GCHandleType.Pinned);

            try
            {
                return Marshal.PtrToStructure<T>(handle.AddrOfPinnedObject());
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
