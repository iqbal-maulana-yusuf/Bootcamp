using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dtos;
using Models;

namespace Api.Mappers
{
    public static class MahasiswaMappers
    {
        public static MahasiswaDto ToMahasiswaDto(this Mahasiswa mhs)
        {
            return new MahasiswaDto
            {
                Nama = mhs.Nama,
                Jurusan = mhs.Jurusan
            };
        }
    }
}