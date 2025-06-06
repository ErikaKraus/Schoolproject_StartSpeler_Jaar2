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
    public class BestellingControllerAPI : ControllerBase
    {
        private readonly IUnitofWork _context;

        public BestellingControllerAPI(IUnitofWork context)
        {
            _context = context;
        }

        // GET: api/BestellingControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Bestelling>>> GetAllBestellingen()
        {
            var bestellingen = await _context.BestellingRepository.GetAllAsync();
            return Ok(bestellingen);


        }

        // GET: api/BestellingControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Bestelling>> GetBestelling(int id)
        {
          if (_context.BestellingRepository == null)
          {
              return NotFound();
          }
            var bestelling = await _context.BestellingRepository.GetByIdAsync(id);

            if (bestelling == null)
            {
                return NotFound();
            }

            return bestelling;
        }

        // PUT: api/BestellingControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBestelling(int id, Bestelling bestelling)
        {
            if (id != bestelling.Id)
            {
                return BadRequest();
            }

            _context.BestellingRepository.Update(bestelling);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BestellingExists(id))
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

        // POST: api/BestellingControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Bestelling>> CreateBestelling(Bestelling bestelling)
        {
          if (_context.BestellingRepository == null)
          {
              return Problem("Entity set 'StartspelerContext.Bestellingen'  is null.");
          }
            await _context.BestellingRepository.AddAsync(bestelling);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBestelling", new { id = bestelling.Id }, bestelling);
        }

        // DELETE: api/BestellingControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBestelling(int id)
        {
            if (_context.BestellingRepository == null)
            {
                return NotFound();
            }
            var bestelling = await _context.BestellingRepository.GetByIdAsync(id);
            if (bestelling == null)
            {
                return NotFound();
            }

            _context.BestellingRepository.Delete(bestelling);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BestellingExists(int id)
        {
            return (_context.BestellingRepository.GetByIdAsync(id).IsCompletedSuccessfully);
        }
    }
}
