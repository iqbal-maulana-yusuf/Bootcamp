using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Dtos;
using Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Serilog;
using Serilog.Context;

namespace Api.Controllers
{
    [Route("api/mahasiswa")]
    [ApiController]
    public class MahasiswaController : ControllerBase
    {
        private ApplicationDbContext _context;
        public MahasiswaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]

        public async Task<IActionResult> GetAll()
        {
            Log.Information("Get All Mahasiswa dipanggil");
            var mhsModel = await _context.DataMahasiswa.ToListAsync();
            mhsModel.Select(s => s.ToMahasiswaDto());

            return Ok(mhsModel);

        }

        [Route("{id}")]
        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {

            using (LogContext.PushProperty("MahasiswaId", id))
            {
                Log.Information("Mencari data mahasiswa dengan ID {Id}", id);

                var mhsModel = await _context.DataMahasiswa.FirstOrDefaultAsync(s => s.Id == id);
                if (mhsModel == null)
                {
                    Log.Warning("Data mahasiswa tidak ditemukan");
                    return NotFound("Data tidak ditemukan");
                }

                Log.Information("Data mahasiswa ditemukan dan dikembalikan");
                return Ok(mhsModel.ToMahasiswaDto());
            }
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Mahasiswa createMhs)
        {
            var mhsModel = await _context.DataMahasiswa.AddAsync(createMhs);
            await _context.SaveChangesAsync();

            return Ok(createMhs.ToMahasiswaDto());
        }


    }
}