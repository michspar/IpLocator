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
        Int64 country_1;
        public string Country {  get { return Encoding.ASCII.GetString(BitConverter.GetBytes(country_1)).TrimEnd('\0'); } }

        Int64 region_1;
        Int32 region_2;
        public string Region { get {
                return (Encoding.ASCII.GetString(BitConverter.GetBytes(region_1))
                      + Encoding.ASCII.GetString(BitConverter.GetBytes(region_2))).TrimEnd('\0'); } }

        Int64 postal_1;
        Int32 postal_2;
        public string Postal {  get {
                return (Encoding.ASCII.GetString(BitConverter.GetBytes(postal_1))
                      + Encoding.ASCII.GetString(BitConverter.GetBytes(postal_2))).TrimEnd('\0'); } }

        Int64 city_1;
        Int64 city_2;
        Int64 city_3;
        public string City { get {
                return (Encoding.ASCII.GetString(BitConverter.GetBytes(city_1))
                      + Encoding.ASCII.GetString(BitConverter.GetBytes(city_2))
                      + Encoding.ASCII.GetString(BitConverter.GetBytes(city_3))).TrimEnd('\0'); } }

        Int64 organization_1;
        Int64 organization_2;
        Int64 organization_3;
        Int64 organization_4;
        public string Organization { get {
                return (Encoding.ASCII.GetString(BitConverter.GetBytes(organization_1))
                      + Encoding.ASCII.GetString(BitConverter.GetBytes(organization_2))
                      + Encoding.ASCII.GetString(BitConverter.GetBytes(organization_3))
                      + Encoding.ASCII.GetString(BitConverter.GetBytes(organization_4))).TrimEnd('\0'); } }

        public float latitude;          // широта
        public float longitude;         // долгота
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct DbCity
    {
        public uint index;         //список индексов записей местоположения отсортированный по названию города
    }
}
