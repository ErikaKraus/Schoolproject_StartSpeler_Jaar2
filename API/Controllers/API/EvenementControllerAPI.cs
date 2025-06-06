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
using System.Diagnostics;

namespace API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvenementControllerAPI : ControllerBase
    {
        private readonly IUnitofWork _context;

        public EvenementControllerAPI(IUnitofWork context)
        {
            _context = context;
        }

        // GET: api/EvenementControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Evenement>>> GetAllEvenementen()
        {
            //community en eventgebruikers worden mee opgehaald
            var evenementen = await _context.EvenementRepository.GetAllAsync(
                e => e.Community,
                e => e.EventGebruikers
                );
            
            foreach (var evenement in evenementen)
            {
                Debug.WriteLine($"Evenement: {evenement.Naam}, Community: {evenement.Community?.Naam}");
            }

            return Ok(evenementen);
        }

        // GET: api/EvenementControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Evenement>> GetEvenement(int id)
        {
            if (_context.EvenementRepository == null)
            {
                return NotFound();
            }

            var evenement = await _context.EvenementRepository.GetByIdAsync(id);

            if (evenement == null)
            {
                return NotFound();
            }

            return Ok(evenement);
        }

        // PUT: api/EvenementControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvenement(int id, Evenement evenement)

        {
            if (id != evenement.Id)
            {
                return BadRequest();
            }
            _context.EvenementRepository.Update(evenement);

            try
            {
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EvenementExists(id))
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

        // POST: api/EvenementControllerAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Create")]
        public async Task<ActionResult<Evenement>> CreateEvenement(Evenement evenement)
        {
            if (_context.EvenementRepository == null)
            {
                return Problem("Entity set 'StartspelerContext.Evenementen'  is null.");
            }
            await _context.EvenementRepository.AddAsync(evenement);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetEvenement", new { id = evenement.Id }, evenement);
        }

        // DELETE: api/EvenementControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvenement(int id)
        {
            if (_context.EvenementRepository == null)
            {
                return NotFound();
            }
            var evenement = await _context.EvenementRepository.GetByIdAsync(id);
            if (evenement == null)
            {
                return NotFound();
            }

            _context.EvenementRepository.Delete(evenement);
            await _context.SaveChangesAsync();


            return NoContent();
        }

        private bool EvenementExists(int id)
        {
            return (_context.EvenementRepository.GetByIdAsync(id).IsCompletedSuccessfully);
        }
    }
}
