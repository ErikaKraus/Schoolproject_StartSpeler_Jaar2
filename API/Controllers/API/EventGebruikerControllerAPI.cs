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
using Newtonsoft.Json;

namespace API.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventGebruikerControllerAPI : ControllerBase
    {
        private readonly IUnitofWork _context;
        private readonly ILogger<EventGebruikerControllerAPI> _logger;


        public EventGebruikerControllerAPI(IUnitofWork context, ILogger<EventGebruikerControllerAPI> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/EventGebruikerControllerAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventGebruiker>>> GetAllEventGebruikers()
        {
            if (_context.EventGebruikerRepository == null)
            {
                return NotFound();
            }

            var eventGebruikers = await _context.EventGebruikerRepository.GetAllAsync();
            return Ok(eventGebruikers);
        }

        // GET: api/EventGebruikerControllerAPI/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventGebruiker>> GetEventGebruiker(int id)
        {
            if (_context.EventGebruikerRepository == null)
            {
                return NotFound();
            }
            var eventGebruiker = await _context.EventGebruikerRepository.GetByIdAsync(id);

            if (eventGebruiker == null)
            {
                return NotFound();
            }

            return eventGebruiker;
        }

        // PUT: api/EventGebruikerControllerAPI/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventGebruiker(int id, EventGebruiker eventGebruiker)
        {
            if (id != eventGebruiker.Id)
            {
                return BadRequest();
            }

            _context.EventGebruikerRepository.Update(eventGebruiker);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventGebruikerExists(id))
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
        

        [HttpPost]
        public async Task<ActionResult<EventGebruiker>> CreateEventGebruiker(EventGebruiker eventGebruiker)
        {
            _logger.LogInformation($"Ontvangen payload: {JsonConvert.SerializeObject(eventGebruiker)}");

            if (_context.EventGebruikerRepository == null)
            {
                _logger.LogError("EventGebruikerRepository is null.");
                return Problem("Entity set 'StartspelerContext.EventGebruikers' is null.");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState is not valid.");
                return BadRequest(ModelState);
            }

            try
            {
                var evenement = await _context.EvenementRepository.GetByIdAsync(eventGebruiker.EvenementId);
                var gebruiker = await _context.GebruikerRepository.GetByIdAsync(eventGebruiker.GebruikerId);

                if (evenement == null)
                {
                    _logger.LogWarning($"Evenement met ID {eventGebruiker.EvenementId} niet gevonden.");
                    return BadRequest("Ongeldige EvenementId.");
                }

                if (gebruiker == null)
                {
                    _logger.LogWarning($"Gebruiker met ID {eventGebruiker.GebruikerId} niet gevonden.");
                    return BadRequest("Ongeldige GebruikerId.");
                }

                _logger.LogInformation($"Evenement opgehaald: {evenement.Datum}, CommunityId: {evenement.CommunityId}");
                _logger.LogInformation($"Gebruiker opgehaald: {gebruiker.Id}, Email: {gebruiker.Email}");

                eventGebruiker.Evenement = evenement;
                eventGebruiker.Gebruiker = gebruiker;

                _logger.LogInformation($"EventGebruiker validatie: EvenementId={eventGebruiker.EvenementId}, GebruikerId={eventGebruiker.GebruikerId}");

                await _context.EventGebruikerRepository.AddAsync(eventGebruiker);
                _logger.LogInformation("EventGebruiker toegevoegd aan repository. Proberen op te slaan...");

                await _context.SaveChangesAsync();
                _logger.LogInformation("EventGebruiker succesvol aangemaakt.");
                return CreatedAtAction(nameof(GetEventGebruiker), new { id = eventGebruiker.Id }, eventGebruiker);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError($"Database update exception: {dbEx.Message}");
                if (dbEx.InnerException != null)
                {
                    _logger.LogError($"Inner exception: {dbEx.InnerException.Message}");
                }
                return StatusCode(500, "Database update error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Fout bij het aanmaken van EventGebruiker: {ex.Message}");
                if (ex.InnerException != null)
                {
                    _logger.LogError($"Inner exception: {ex.InnerException.Message}");
                }
                return StatusCode(500, "Interne serverfout");
            }
        }


        // DELETE: api/EventGebruikerControllerAPI/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventGebruiker(int id)
        {
            if (_context.EventGebruikerRepository == null)
            {
                return NotFound();
            }
            var eventGebruiker = await _context.EventGebruikerRepository.GetByIdAsync(id);
            if (eventGebruiker == null)
            {
                return NotFound();
            }

            _context.EventGebruikerRepository.Delete(eventGebruiker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EventGebruikerExists(int id)
        {
            return (_context.EventGebruikerRepository.GetByIdAsync(id).IsCompletedSuccessfully);
        }
    }
}
