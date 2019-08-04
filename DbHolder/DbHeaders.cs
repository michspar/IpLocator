using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DbHolder
{
    //60 bytes
    public class DbHeader
    {
        public int Version { get; set; }           // версия база данных

        public string Name { get; set; }          // название/префикс для базы данных sbyte[32]

        public ulong Timestamp { get; set; }         // время создания базы данных

        public int Records { get; set; }           // общее количество записей

        public uint Offset_ranges { get; set; }     // смещение относительно начала файла до начала списка записей с геоинформацией

        public uint Offset_cities { get; set; }     // смещение относительно начала файла до начала индекса с сортировкой по названию городов

        public uint Offset_locations { get; set; }  // смещение относительно начала файла до начала списка записей о местоположении

        public DbHeader(byte[] buf)
        {
            using (var initStream = new BinaryReader(new MemoryStream(buf)))
            {
                Version = initStream.ReadInt32();
                Name = initStream.ReadFixedString(32);
                Timestamp = initStream.ReadUInt64();
                Records = initStream.ReadInt32();
                Offset_ranges = initStream.ReadUInt32();
                Offset_cities = initStream.ReadUInt32();
                Offset_locations = initStream.ReadUInt32();
            }
        }
    }

    // 12 bytes
    class DbRange
    {
        public const int size = 12;

        public uint Ip_from { get; set; }           // начало диапазона IP адресов

        public uint Ip_to { get; set; }             // конец диапазона IP адресов

        public uint Location_index { get; set; }     // индекс записи о местоположении

        public DbRange(byte[] buf, int index, int count)
        {
            using (var br = new BinaryReader(new MemoryStream(buf, index, count)))
            {
                Ip_from = br.ReadUInt32();
                Ip_to = br.ReadUInt32();
                Location_index = br.ReadUInt32();
            }
        }
    }

    //96 bytes
    class DbLocation
    {
        public const int size = 96;

        public string Country { get; set; }        //[8] название страны (случайная строка с префиксом "cou_")

        public string Region { get; set; }        //[12] название области (случайная строка с префиксом "reg_")

        public string Postal { get; set; }        //[12] почтовый индекс (случайная строка с префиксом "pos_")

        public string City { get; set; }          //[24] название города (случайная строка с префиксом "cit_")

        public string Organization { get; set; }  //[32] название организации (случайная строка с префиксом "org_")

        public float latitude { get; set; }          // широта

        public float longitude { get; set; }         // долгота

        public DbLocation(byte[] buf, int index, int count)
        {
            using (var br = new BinaryReader(new MemoryStream(buf, index, count)))
            {
                Country = br.ReadFixedString(8);
                Region = br.ReadFixedString(12);
                Postal = br.ReadFixedString(12);
                City = br.ReadFixedString(24);
                Organization = br.ReadFixedString(32);
                latitude = br.ReadSingle();
                longitude = br.ReadSingle();
            }
        }
    }

    class DbCity
    {
        public int Index { get; set; }          //список индексов записей местоположения отсортированный по названию города

        public DbCity(byte[] buf, int index, int count)
        {
            using (var br = new BinaryReader(new MemoryStream(buf, index, count)))
            {
                Index = br.ReadInt32();
            }
        }
    }
}
