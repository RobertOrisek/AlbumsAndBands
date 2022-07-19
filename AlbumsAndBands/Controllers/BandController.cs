using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZavrsniTest.Repository.Interfaces;
using ZavrsniTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsniTest.Controllers
{
    [Route("api/bendovi")]
    [ApiController]
    public class BandController : ControllerBase
    {
        private readonly IBandRepository _bandRepository;

        public BandController(IBandRepository bandRepository)
        {
            _bandRepository = bandRepository;
        }

        // GET: api/Bands
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetBands()
        {
            return Ok(_bandRepository.GetAll().ToList());
        }

        // GET: api/Bands/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetBand(int id)
        {
            var band = _bandRepository.GetById(id);
            if (band == null)
            {
                return NotFound();
            }

            return Ok(band);
        }


        // PUT: api/Bands/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PutBand(int id, Band band)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != band.Id)
            {
                return BadRequest();
            }

            try
            {
                _bandRepository.Update(band);
            }
            catch
            {
                return BadRequest();
            }

            return Ok(band);
        }

        // POST: api/Bands
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PostBand(Band band)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bandRepository.Add(band);
            return CreatedAtAction("GetBand", new { id = band.Id }, band);
        }

        // DELETE: api/Bands/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteBand(int id)
        {
            var band = _bandRepository.GetById(id);
            if (band == null)
            {
                return NotFound();
            }

            _bandRepository.Delete(band);
            return NoContent();
        }
    }
}