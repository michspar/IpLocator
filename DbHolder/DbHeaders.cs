using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DbHolder
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DbHeader
    {
        public int version;           // версия база данных

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string name;          // название/префикс для базы данных sbyte[32]

        public ulong timestamp;         // время создания базы данных
        public int records;           // общее количество записей
        public uint offset_ranges;     // смещение относительно начала файла до начала списка записей с геоинформацией
        public uint offset_cities;     // смещение относительно начала файла до начала индекса с сортировкой по названию городов
        public uint offset_locations;  // смещение относительно начала файла до начала списка записей о местоположении
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct DbRange
    {
        public uint ip_from;           // начало диапазона IP адресов
        public uint ip_to;             // конец диапазона IP адресов
        public uint location_index;     // индекс записи о местоположении
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct DbLocation
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 8)]
        public string country;        //[8] название страны (случайная строка с префиксом "cou_")

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string region;        //[12] название области (случайная строка с префиксом "reg_")

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public string postal;        //[12] почтовый индекс (случайная строка с префиксом "pos_")

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 24)]
        public string city;          //[24] название города (случайная строка с префиксом "cit_")

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
        public string organization;  //[32] название организации (случайная строка с префиксом "org_")

        public float latitude;          // широта
        public float longitude;         // долгота
    }

    struct Locations
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100000)]
        public DbLocation[] locations;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct DbCity
    {
        public uint index;         //список индексов записей местоположения отсортированный по названию города
    }
}
