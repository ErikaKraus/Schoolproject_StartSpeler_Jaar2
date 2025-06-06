using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models;
using API.Data.UnitofWork;

namespace API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtikelControllerAPI : ControllerBase
    {
        private readonly IUnitofWork _context;

        public ArtikelControllerAPI(IUnitofWork context)
        {
            _context = context;
        }

        // GET: api/ArtikelControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artikel>>> GetAllArtikels()
        {
            var artikels = await _context.ArtikelRepository.GetAllAsync();
            return Ok(artikels);
        }

        // GET: api/ArtikelControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Artikel>> GetArtikel(int id)
        {
            if (_context.ArtikelRepository == null)
            {
                return NotFound();
            }
            var artikels = await _context.ArtikelRepository.GetByIdAsync(id);

            if (artikels == null)
            {
                return NotFound();
            }

            return Ok(artikels);
        }

        // PUT: api/ArtikelControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArtikel(int id, Artikel artikel)
        {
            if (id != artikel.Id)
            {
                return BadRequest();
            }

            _context.ArtikelRepository.Update(artikel);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtikelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ArtikelControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Artikel>> CreateArtikel(Artikel artikel)
        {
          if (_context.ArtikelRepository == null)
          {
              return Problem("Entity set 'StartspelerContext.Artikels'  is null.");
          }
             await _context.ArtikelRepository.AddAsync(artikel);
             await _context.SaveChangesAsync();

            return CreatedAtAction("GetArtikel", new { id = artikel.Id }, artikel);
        }

        // DELETE: api/ArtikelControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArtikel(int id)
        {
            if (_context.ArtikelRepository == null)
            {
                return NotFound();
            }
            var artikel = await _context.ArtikelRepository.GetByIdAsync(id);
            if (artikel == null)
            {
                return NotFound();
            }

            _context.ArtikelRepository.Delete(artikel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtikelExists(int id)
        {
            return (_context.ArtikelRepository.GetByIdAsync(id).IsCompletedSuccessfully);
        }
    }
}
